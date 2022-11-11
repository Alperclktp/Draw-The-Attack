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
        if(GameManager.Instance.towerPosition != null)
        {
            GameObject obj = Instantiate(enemyPrefabs[value], enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, enemyPrefabs[value].transform.rotation);

            obj.transform.parent = enemySpawnHolder.transform;

            AddToEnemyList(obj);
        }
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
            GameManager.Instance.gameState = GameState.COMPLATE;

            GetExplosionVFX();

            Destroy(gameObject);

            GameManager.Instance.levelComplete = true;

            GameManager.Instance.nextLevelButton.SetActive(true);

            GameManager.Instance.cardPanel.SetActive(false);

            GameManager.Instance.selectTutorialHand.SetActive(false);

            Debug.Log("You Won!");

            for (int i = 0; i < GameManager.Instance.enemyList.Count; i++)
            {
                Destroy(GameManager.Instance.enemyList[i].gameObject);
            }
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

    public void TowerHıtAnimation()
    {
        transform.DORotate(new Vector3(0, 180, 0.3f), 0.1f).OnComplete(() => 
        {
            transform.DORotate(new Vector3(0, 180, -0.3f), 0.1f);
        }); 
    }

    public override void TakeDamage(float damage)
    {
        currentTowerHealth -= (int)damage;

        TowerHıtAnimation();
    }
}

[System.Serializable]
public class LevelDifficulty
{
    //public int maxLevelNumberOfEnemies;

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

    public List<EnemyCardSO> enemyCardSO = new List<EnemyCardSO>();

    public void SetHardness(int hardness)
    {
        CardManager cardManager = CardManager.Instance;

        switch (hardness)
        {
            case 1: //Hardness 1

                warrior = 24;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost; 

                enemyCardSO[0].attackDamage = cardManager.cardList[0].cardSO.attackDamage /3f; //Enemy warrior attack power /3 Soldiers attack power
                enemyCardSO[0].health = (int)(cardManager.cardList[0].cardSO.health / 3f); //Enemy warrior health /3 Soldier health;

                archer = 5;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //archer = cardManager.maxMana / cardManager.cardList[1].currentManaCost;
                enemyCardSO[1].attackDamage = cardManager.cardList[1].cardSO.attackDamage /3f; //Enemy archer attack power /3 Archer attack power
                enemyCardSO[1].health = (int)(cardManager.cardList[1].cardSO.health / 3f); //Enemy archer health /3 Archer health;


                giant = 2;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //giant = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                enemyCardSO[2].attackDamage = cardManager.cardList[2].cardSO.attackDamage /3f; //Enemy giant attack power /3 giant attack power
                enemyCardSO[2].health = (int)(cardManager.cardList[2].cardSO.health / 3f); //Enemy giant health /3 Giant health;

                break;
            case 2: //Hardness 2

                warrior = 36;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost;
                enemyCardSO[0].attackDamage = cardManager.cardList[0].cardSO.attackDamage / 2f; //Enemy warrior power /2 Soldier attack power
                enemyCardSO[0].health = (int)(cardManager.cardList[0].cardSO.health / 2f); //Enemy warrior health /3 Soldier health;

                archer = 8;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //archer = (int)(cardManager.maxMana / cardManager.cardList[1].currentManaCost * 0.5f);
                enemyCardSO[1].attackDamage = cardManager.cardList[1].cardSO.attackDamage / 2f; //Enemy archer power /2 Archer attack power
                enemyCardSO[1].health = (int)(cardManager.cardList[1].cardSO.health / 2f); //Enemy archer health /2 Archer health;

                giant = 6;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //giant = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                enemyCardSO[2].attackDamage = cardManager.cardList[2].cardSO.attackDamage / 2f; //Enemy giant power /2 Giant attack power
                enemyCardSO[2].health = (int)(cardManager.cardList[2].cardSO.health / 2f); //Enemy giant health /2 Giant health;

                break;
            case 3: //Hardness 3

                warrior = 54;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost;
                enemyCardSO[0].attackDamage = cardManager.cardList[0].cardSO.attackDamage / 1.5f; //Enemy warrior attack power /1.5 Soldier attack power
                enemyCardSO[0].health = (int)(cardManager.cardList[0].cardSO.health / 1.5f); //Enemy warrior health /1.5 Soldier health;

                archer = 12;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //archer = (int)(cardManager.maxMana / cardManager.cardList[1].currentManaCost * 0.5f);
                enemyCardSO[1].attackDamage = cardManager.cardList[1].cardSO.attackDamage / 1.5f;  //Enemy archer power /1.5 Archer attack power
                enemyCardSO[1].health = (int)(cardManager.cardList[1].cardSO.health / 1.5f); //Enemy archer health /1.5 Archer health;


                giant = 6;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //giant = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                enemyCardSO[2].attackDamage = cardManager.cardList[2].cardSO.attackDamage / 1.5f; //Enemy giant attack power /1.5 Giant attack power
                enemyCardSO[2].health = (int)(cardManager.cardList[2].cardSO.health / 1.5f); //Enemy giant health /1.5 Giant health;
                break;
            default:
                break;
        }
    }
}
