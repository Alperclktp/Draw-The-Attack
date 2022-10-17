using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    public float health;

    public int amountToGiveMana;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void Update()
    {
        CheckHealth();
    }

    private void CheckHealth() //Test function.
    {
        if (health <= 0)
        {
            DestroyEnemy();

            RemoveEnemyFromList();

            AddToMana(amountToGiveMana);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    private void RemoveEnemyFromList()
    {
        GameManager.Instance.enemyList.Remove(this.gameObject);
    }

    private void AddToMana(int amount)
    {
        CardManager.Instance.currentMana += amount;
    }
}
