using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TowerManager : BaseAttackController  
{
    public static TowerManager Instance;

    [SerializeField] private Transform[] enemySpawnPoints;

    [SerializeField] private GameObject enemySpawnHolder;

    public MyDictionarys enemyPrefabs = new MyDictionarys();

    [Header("Tower Stats")]
    public float currentTowerHealth;

    [SerializeField] private float secondsBetweenSpawn;

    [Tooltip("Total number of enemies to spawn")]
    [SerializeField] private int maxLevelNumberOfEnemies;

    [ReadOnly] public float totalNumberOfSpawnedEnemies;
 
    [ReadOnly] public float elapsedTime = 0.0f;

    [Space(5)]
    public bool canSpawn;

    public override string damageableID { get { return typeof(TowerManager).Name; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (canSpawn)
        {
            EnemySpawner("Warrior");
        }

        CheckHealth();
    }

    private void EnemySpawner(string value)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > secondsBetweenSpawn && totalNumberOfSpawnedEnemies < maxLevelNumberOfEnemies)
        {
            for (int i = 0; i < 1; i++)
            {
                GameObject obj = Instantiate(enemyPrefabs[value], enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, enemyPrefabs[value].transform.rotation);

                obj.transform.parent = enemySpawnHolder.transform;

                AddToEnemyList(obj);

                elapsedTime = 0f;

                totalNumberOfSpawnedEnemies++;
            }
        }
    }
    private void CheckHealth()
    {
        if (currentTowerHealth <= 0)
        {
            GetExplosionVFX();

            Destroy(gameObject);

            GameManager.Instance.levelComplete = true;

            Debug.Log("You Won!");
        }
    }

    private void GetExplosionVFX()
    {
        Destroy(VFXManager.SpawnEffect(VFXType.TOWER_EXPLOSION_EFFECT, transform.position + new Vector3(0, 4, 0), Quaternion.identity), 1f);
    }

    private void AddToEnemyList(GameObject obj)
    {
        GameManager.Instance.enemyList.Add(obj);
    }

    public override void TakeDamage(float damage)
    {
        currentTowerHealth -= damage;
    }
}
