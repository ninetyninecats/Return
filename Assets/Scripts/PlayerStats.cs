using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TarodevController;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int startingHealth = 5;
    private int currentHealth;
    [SerializeField] private GameObject spawnRoom;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] GameObject healthBar;
    GameObject heart0;
    GameObject heart1;
    GameObject heart2;
    GameObject heart3;
    GameObject heart4;
    private float iFramesSeconds = 1;

    private void Awake()
    {
        currentHealth = startingHealth;
#if UNITY_STANDALONE
        switch (SaveFile.GetSavePoint())
        {
            case 0:
                spawnRoom = (GameObject)Resources.Load("Levels/Room0");
                spawnPoint = new Vector3(-5, 5, 0);
                break;
            default: throw new NotImplementedException();
        }
#endif
    }
    private void Start()
    {
        heart0 = healthBar.transform.GetChild(0).gameObject;
        heart1 = healthBar.transform.GetChild(1).gameObject;
        heart2 = healthBar.transform.GetChild(2).gameObject;
        heart3 = healthBar.transform.GetChild(3).gameObject;
        heart4 = healthBar.transform.GetChild(4).gameObject;
    }
    private void Update()
    {
        SetHealthBar(); 
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("Ow");
        if (damage > currentHealth)
        {
            Die();
            return;
        }
        currentHealth = currentHealth - damage;
        StartCoroutine(Invulnerability());

    }
    public void Heal(int health)
    {
        if (health > startingHealth - currentHealth) currentHealth = startingHealth;
        else currentHealth = currentHealth + health;
    }
    public void Die()
    {
        LevelController.LoadLevel(spawnRoom, spawnPoint, gameObject);
        Debug.Log("I'm dead");
        currentHealth = startingHealth;
    }
    public IEnumerator Invulnerability()
    {
        Debug.Log("Invincible");
        Physics2D.IgnoreLayerCollision(6, 9, true);
        yield return new WaitForSeconds(iFramesSeconds);
        Physics2D.IgnoreLayerCollision(6, 9, false);
    }
    private void SetHealthBar()
    {
        if (currentHealth == 1)
        {
            heart0.SetActive(true);
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
            heart4.SetActive(false);
        }
        if (currentHealth == 2)
        {
            heart0.SetActive(true);
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
            heart4.SetActive(false);
        }
        if (currentHealth == 3)
        {
            heart0.SetActive(true);
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
            heart4.SetActive(false);
        }
        if (currentHealth == 4)
        {
            heart0.SetActive(true);
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(false);
        }
        if (currentHealth >= 5)
        {
            heart0.SetActive(true);
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(true);
        }
    }


}