using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    public Transform[] enemySpawnPoints;

    [SerializeField] private GameObject enemySpawnHolder;

    public GameObject enemyPrefab;

    public float intervalSpawnTimer;

    public bool canSpawn;

    private void Update()
    {
        StartCoroutine(IEEnemySpawner());
    }

    private void EnemySpawner()
    {
        GameObject obj = Instantiate(enemyPrefab, enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, enemyPrefab.transform.rotation);

        obj.transform.parent = enemySpawnHolder.transform;

        AddToEnemyList();
    }

    private void AddToEnemyList()
    {
        GameManager.Instance.enemyList.Add(enemyPrefab);
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
}
