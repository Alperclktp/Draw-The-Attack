using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using UnityEngine.UI;

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

    [Header("UI Elements")]
    public Text currentHealthText;
    public override string damageableID { get { return typeof(TowerManager).Name; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckHealth();

        currentHealthText.text = currentTowerHealth.ToString();
    }

    private void EnemySpawner(string value)
    {
        GameObject obj = Instantiate(enemyPrefabs[value], enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, enemyPrefabs[value].transform.rotation);

        obj.transform.parent = enemySpawnHolder.transform;

        AddToEnemyList(obj);

    }

    public IEnumerator IEEnemySpawner()
    {
        if (canSpawn)
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
    }

    private void CheckHealth()
    {
        if (currentTowerHealth <= 0)
        {
            GetExplosionVFX();

            Destroy(gameObject);

            GameManager.Instance.levelComplete = true;

            GameManager.Instance.nextLevelButton.SetActive(true);

            GameManager.Instance.cardPanel.SetActive(false);

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

    public void TowerHýtAnimation()
    {
        transform.DOScaleX(20.2f, 0.1f).OnComplete(() =>
        {
            transform.DOScaleX(19.86586f, 0.1f);
        });
    }

    public override void TakeDamage(float damage)
    {
        currentTowerHealth -= (int)damage;

        TowerHýtAnimation();
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

    public void SetHardness(int hardness)
    {
        CardManager cardManager = CardManager.Instance;

        switch (hardness)
        {
            case 1:
                warrior = 10; 

                warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost;
                cardManager.cardList[0].cardSO.attackDamage = cardManager.cardList[0].cardSO.health / 3;
                cardManager.cardList[0].cardSO.health = (int)cardManager.cardList[0].cardSO.attackDamage;
               
                archer = 0; 

                archer = cardManager.maxMana / cardManager.cardList[1].currentManaCost;
                cardManager.cardList[1].cardSO.attackDamage = cardManager.cardList[1].cardSO.health / 3;
                cardManager.cardList[1].cardSO.health = (int)cardManager.cardList[1].cardSO.attackDamage;

                giant = 1; 

                giant = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                cardManager.cardList[2].cardSO.attackDamage = cardManager.cardList[2].cardSO.health / 2;
                cardManager.cardList[2].cardSO.health = (int)cardManager.cardList[2].cardSO.attackDamage;

                break;
            case 2:
                warrior = 15;

                warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost;
                cardManager.cardList[0].cardSO.attackDamage = cardManager.cardList[0].cardSO.health / 2;
                cardManager.cardList[0].cardSO.health = (int)((int)cardManager.cardList[0].cardSO.attackDamage * 1.5f);

                archer = 1;

                warrior = (int)(cardManager.maxMana / cardManager.cardList[1].currentManaCost * 0.5f);
                cardManager.cardList[1].cardSO.attackDamage = cardManager.cardList[1].cardSO.health / 2;
                cardManager.cardList[1].cardSO.health = (int)((int)cardManager.cardList[1].cardSO.attackDamage * 1.5f);

                giant = 3;

                warrior = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                cardManager.cardList[2].cardSO.attackDamage = cardManager.cardList[2].cardSO.health / 2;
                cardManager.cardList[2].cardSO.health = (int)((int)cardManager.cardList[2].cardSO.attackDamage * 1.5f);

                break;
            case 3:
                warrior = 25;

                warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost;
                cardManager.cardList[0].cardSO.attackDamage = cardManager.cardList[0].cardSO.health;
                cardManager.cardList[0].cardSO.health = (int)cardManager.cardList[0].cardSO.attackDamage * 2;

                archer = 3;

                warrior = (int)(cardManager.maxMana / cardManager.cardList[1].currentManaCost * 0.5f);
                cardManager.cardList[1].cardSO.attackDamage = cardManager.cardList[1].cardSO.health;
                cardManager.cardList[1].cardSO.health = (int)cardManager.cardList[1].cardSO.attackDamage * 2;

                giant = 4;

                giant = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                cardManager.cardList[2].cardSO.attackDamage = cardManager.cardList[2].cardSO.health;
                cardManager.cardList[2].cardSO.health = (int)cardManager.cardList[2].cardSO.attackDamage * 2;


                break;
            default:
                break;
        }
    }
}
