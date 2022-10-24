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

    [Space(5)]
    public bool canSpawn;

    public LevelDifficulty levelDifficulty;
    public override string damageableID { get { return typeof(TowerManager).Name; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckHealth();
    }

    private void EnemySpawner(string value)
    {
        GameObject obj = Instantiate(enemyPrefabs[value], enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, enemyPrefabs[value].transform.rotation);

        obj.transform.parent = enemySpawnHolder.transform;

        AddToEnemyList(obj);

    }
    public IEnumerator IEEnemySpawner()
    {
        for (int i = 0; i < levelDifficulty.Warrior; i++)
        {
            yield return new WaitForSeconds(secondsBetweenSpawn);

            EnemySpawner("Warrior");

            //if
        }

        for (int i = 0; i < levelDifficulty.Archer; i++)
        {
            yield return new WaitForSeconds(secondsBetweenSpawn);

            EnemySpawner("Archer");

            //if()
        }

        for (int i = 0; i < levelDifficulty.Giant; i++)
        {
            yield return new WaitForSeconds(secondsBetweenSpawn);

            EnemySpawner("Giant");

            //if()
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

[System.Serializable]
public class LevelDifficulty
{
    public int maxLevelNumberOfEnemies;

    [SerializeField] private int warrior = 0;
    [SerializeField] private int archer = 0;
    [SerializeField] private int giant = 0;

    public int Warrior
    {
        get { return warrior; }
        set
        {
            if (value >= 0)
            {
                warrior = value;
            }
        }
    }
    public int Archer
    {
        get { return archer; }
        set
        {
            if (value >= 0)
            {
                archer = value;
            }
        }
    }
    public int Giant
    {
        get { return giant; }
        set
        {
            if (value >= 0)
            {
                giant = value;
            }
        }
    }
}
