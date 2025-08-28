using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] public GameObject room;
    void OnTriggerEnter2D(Collider2D player)
    {
        var currentRoom = transform.parent.gameObject;
        LevelController.LoadLevel(currentRoom, room, player.gameObject);
    }
}
