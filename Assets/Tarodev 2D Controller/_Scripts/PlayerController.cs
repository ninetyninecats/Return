using System;
using System.Collections;
using UnityEngine;

namespace TarodevController
{
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// I have a premium version on Patreon, which has every feature you'd expect from a polished controller. Link: https://www.patreon.com/tarodev
    /// You can play and compete for best times here: https://tarodev.itch.io/extended-ultimate-2d-controller
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/tarodev
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D rigidBody2D;
        private CapsuleCollider2D _col;
        private Sprite sprite;
        private Animator animator;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        private float attackCooldown = 1;
        private float attackCooldownTimer;
        private float dashCooldown = 1;
        private float dashCooldownTimer;
        private bool isDashing;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            sprite = GetComponent<Sprite>();
            animator = GetComponent<Animator>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            Debug.Log(SaveFile.GetSavePoint());
            SaveFile.SetDoubleJump(true);
            SaveFile.SaveToFile();
        }
        private void Start() {
            gameObject.GetComponent<PlayerStats>().Die();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            dashCooldownTimer += Time.deltaTime;
            attackCooldownTimer += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Z),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.Z),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                Debug.Log("Jump pressed");
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();
            if (!isDashing)
            {
                HandleJump();
                HandleDirection();
                HandleGravity();

                ApplyMovement();
                if (Input.GetKeyDown(KeyCode.X) && attackCooldownTimer >= attackCooldown) Attack();
                if (this._frameInput.Move.x > 0.001f) transform.localScale = Vector3.one;
                else if (this._frameInput.Move.x < -0.001f) transform.localScale = new Vector3(-1, 1, 1);
                animator.SetBool("isWalking", _frameInput.Move != Vector2.zero);
                animator.SetBool("isGrounded", _grounded);
            }
            if (Input.GetKeyDown(KeyCode.C)) StartCoroutine(Dash());
        }

        #region Collisions

        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                doubleJumped = false;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;
        private bool doubleJumped;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && rigidBody2D.linearVelocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;
            Debug.Log("Handling jump, " + _grounded + ", " + CanUseCoyote + ", " + doubleJumped + ", " + SaveFile.GetDoubleJump());
            if (_grounded || CanUseCoyote) ExecuteJump(false);
            else if (!doubleJumped && SaveFile.GetDoubleJump()) ExecuteJump(true);

            _jumpToConsume = false;
        }

        private void ExecuteJump(bool doubleJump)
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            doubleJumped = doubleJump;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region My stuff
        public void WarpTo(Vector3 position)
        {
            var rigidBody2D = GetComponent<Rigidbody2D>();
            rigidBody2D.position = position;
        }
        public IEnumerator Dash()
        {
            if (dashCooldownTimer < dashCooldown) yield return null;
            var rigidBody2D = GetComponent<Rigidbody2D>();
            isDashing = true;
            float startTime = Time.time;
            rigidBody2D.linearVelocity = Vector2.zero;
            rigidBody2D.gravityScale = 0;
            rigidBody2D.linearDamping = 0;
            Vector2 direction = new Vector2(transform.localScale.x, 0);

            while (Time.time < startTime + 0.1f)
            {
                rigidBody2D.linearVelocity = direction.normalized * 30;
                yield return null;
            }
            dashCooldownTimer = 0;
            isDashing = false;
        }
        public void Attack()
        {
            animator.SetBool("isSlashing", true);
            attackCooldown = 0;
            animator.SetBool("isSlashing", false);
        }
        #endregion

        private void ApplyMovement() => rigidBody2D.linearVelocity = _frameVelocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}