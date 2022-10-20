using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>, IDamageable
{
    public Transform[] enemySpawnPoints;

    [SerializeField] private GameObject enemySpawnHolder;

    public GameObject enemyPrefab;

    public float intervalSpawnTimer;

    public bool canSpawn;

    [Header("Tower Stats")]
    public float currentHealth;
    public string damageableID { get { return typeof(TowerManager).Name; } }

    private void Update()
    {
        StartCoroutine(IEEnemySpawner());

        CheckHealth();
    }

    private void EnemySpawner()
    {
        GameObject obj = Instantiate(enemyPrefab, enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, enemyPrefab.transform.rotation);

        obj.transform.parent = enemySpawnHolder.transform;

        AddToEnemyList(obj);
    }

    private void AddToEnemyList(GameObject obj)
    {
        GameManager.Instance.enemyList.Add(obj);
    }

    private IEnumerator IEEnemySpawner()
    {
        while (canSpawn)
        {
            EnemySpawner();

            canSpawn = false;

            yield return new WaitForSeconds(intervalSpawnTimer);

            canSpawn = true;
        }
    }

    private void CheckHealth()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);

            Debug.Log("You Won!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
