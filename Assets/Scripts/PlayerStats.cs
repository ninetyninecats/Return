using System;
using System.Collections;
using TarodevController;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int startingHealth = 5;
    private int currentHealth;
    private GameObject spawnRoom;
    private Vector3 spawnPoint;
    private float iFramesSeconds = 1;

    private void Awake()
    {
        currentHealth = startingHealth;
        switch (SaveFile.GetSavePoint())
        {
            case 0:
                spawnRoom = (GameObject)Resources.Load("Levels/Room0");
                spawnPoint = new Vector3(-5, 5, 0);
                break;
            default: throw new NotImplementedException();
        }
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


}
