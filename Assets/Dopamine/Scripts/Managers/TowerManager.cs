using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using UnityEngine.UI;

public class TowerManager : BaseAttackController
{
    public static TowerManager Instance;

    [SerializeField] private List<Transform> enemySpawnPoints;

    [SerializeField] private GameObject enemySpawnHolder;

    public MyDictionarys enemyPrefabs = new MyDictionarys();

    [Header("Tower Stats")]
    public float totalHealth;

    public float currentTower1Health;
    public float currentTower2Health;
    public float currentTower3Health;

    public float secondsBetweenSpawn;

    [Space(5)]
    public bool canSpawn;

    public LevelDifficulty levelDifficulty;

    [Header("UI Elements")]
    public Text currentTowerHealth1Text;
    public Text currentTowerHealth2Text;
    public Text currentTowerHealth3Text;

    public static Transform NearestTower(Vector3 pos) { return Instance.GetComponentsInChildren<Transform>().Where(_ => _.name.Length == 6 && _.name != "Towers" && _.name.StartsWith("Tower")).OrderBy(_ => Vector3.Distance(pos, _.position)).First(); }

    public override string damageableID { get { return typeof(TowerManager).Name; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        totalHealth = currentTower1Health + currentTower2Health + currentTower3Health;

        CheckHealth();

        if (currentTowerHealth1Text) currentTowerHealth1Text.text = currentTower1Health.ToString();
        if (currentTowerHealth2Text) currentTowerHealth2Text.text = currentTower2Health.ToString();
        if (currentTowerHealth3Text) currentTowerHealth3Text.text = currentTower3Health.ToString();
    }

    private void EnemySpawner(string value)
    {
        if (canSpawn)
        {
            List<Transform> _enemySpawnPoints = enemySpawnPoints.Where(_ => _ != null).ToList();

            if (_enemySpawnPoints.Count > 0)
            {
                GameObject obj = Instantiate(enemyPrefabs[value], _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)].transform.position, enemyPrefabs[value].transform.rotation);

                obj.transform.parent = enemySpawnHolder.transform;

                AddToEnemyList(obj);
            }
        }
    }

    public IEnumerator IEEnemySpawner()
    {
        if (canSpawn)
        {
            for (int i = 0; i < levelDifficulty.Warrior; i++)
            {
                if (!canSpawn) break;

                yield return new WaitForSeconds(secondsBetweenSpawn);

                EnemySpawner("Warrior");
            }

            for (int i = 0; i < levelDifficulty.Archer; i++)
            {
                if (!canSpawn) break;

                yield return new WaitForSeconds(secondsBetweenSpawn);

                EnemySpawner("Archer");
            }

            for (int i = 0; i < levelDifficulty.Giant; i++)
            {
                if (!canSpawn) break;

                yield return new WaitForSeconds(secondsBetweenSpawn);

                EnemySpawner("Giant");
            }
        }
    }

    private void CheckHealth()
    {
        if (currentTower1Health <= 0) currentTower1Health = 0;
        if (currentTower2Health <= 0) currentTower2Health = 0;
        if (currentTower3Health <= 0) currentTower3Health = 0;

        for (int i = 0; i < 3; i++)
        {
            float towerHealth = i == 0 ? currentTower1Health : i == 1 ? currentTower2Health : currentTower3Health;
            string towerName = $"Tower{i + 1}";

            if (towerHealth <= 0 && transform.GetComponentsInChildren<Transform>().Any(_ => _.name == towerName) && towerName != null)
            {
                GetExplosionVFX(transform.GetComponentsInChildren<Transform>().First(_ => _.name == towerName));
                Destroy(transform.Find(towerName).gameObject);
                enemySpawnPoints[i] = null; //
            }
        }

        if (totalHealth <= 0)
        {
            GameManager.Instance.gameState = GameState.COMPLATE;

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

    private void GetExplosionVFX(Transform tower)
    {
        Destroy(VFXManager.SpawnEffect(VFXType.TOWER_EXPLOSION_EFFECT, tower.position + new Vector3(0, 4, 0), Quaternion.identity), 1f);
    }

    private void AddToEnemyList(GameObject obj)
    {
        GameManager.Instance.enemyList.Add(obj);
    }

    public void TowerHıtAnimation(Transform tower)
    {
        tower.DORotate(new Vector3(0, 180, 0.3f), 0.1f).OnComplete(() =>
        {
            tower.DORotate(new Vector3(0, 180, -0.3f), 0.1f);
        });
    }

    public override void TakeDamage(float damage, Vector3? pos = null)
    {
        int closestTowerIndex = int.Parse(NearestTower((Vector3)pos).name.Substring(5, 1));

        switch (closestTowerIndex)
        {
            case 1: currentTower1Health -= (int)damage; break;
            case 2: currentTower2Health -= (int)damage; break;
            case 3: currentTower3Health -= (int)damage; break;
        }

        TowerHıtAnimation(NearestTower((Vector3)pos));
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

    public void SetHardness()
    {
        CardManager cardManager = CardManager.Instance;

        float hardness = 1 + GameManager.Instance.level * LevelManager.Instance.levelSOTemplate.hardnessPerLevel;

        warrior = Mathf.FloorToInt(22.5f * hardness);
        enemyCardSO[0].attackDamage = cardManager.cardList[0].cardSO.attackDamage / 2.5f * hardness;
        enemyCardSO[0].health = (int)(cardManager.cardList[0].cardSO.health / 2.5f * hardness);

        archer = Mathf.FloorToInt(7.5f * hardness);
        enemyCardSO[1].attackDamage = cardManager.cardList[1].cardSO.attackDamage / 2.5f * hardness;
        enemyCardSO[1].health = (int)(cardManager.cardList[1].cardSO.health / 2.5f * hardness);

        giant = Mathf.FloorToInt(1.5f * hardness);
        enemyCardSO[2].attackDamage = (float)(cardManager.cardList[2].cardSO.attackDamage / 3 * hardness);
        enemyCardSO[2].health = (int)(cardManager.cardList[2].cardSO.health / 3 * hardness);

        return;

        switch (hardness)
        {
            case 1: //Hardness 1

                warrior = 48;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost; 

                enemyCardSO[0].attackDamage = cardManager.cardList[0].cardSO.attackDamage / 2.5f; //Enemy warrior attack power /3 Soldiers attack power
                enemyCardSO[0].health = (int)(cardManager.cardList[0].cardSO.health / 2.5f); //Enemy warrior health /3 Soldier health;

                archer = 10;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //archer = cardManager.maxMana / cardManager.cardList[1].currentManaCost;
                enemyCardSO[1].attackDamage = cardManager.cardList[1].cardSO.attackDamage / 2.5f; //Enemy archer attack power /3 Archer attack power
                enemyCardSO[1].health = (int)(cardManager.cardList[1].cardSO.health / 2.5f); //Enemy archer health /3 Archer health;


                giant = 4;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //giant = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                enemyCardSO[2].attackDamage = cardManager.cardList[2].cardSO.attackDamage / 2.5f; //Enemy giant attack power /3 giant attack power
                enemyCardSO[2].health = (int)(cardManager.cardList[2].cardSO.health / 2.5f); //Enemy giant health /3 Giant health;

                break;
            case 2: //Hardness 2

                warrior = 81;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost;
                enemyCardSO[0].attackDamage = cardManager.cardList[0].cardSO.attackDamage / 1.6f; //Enemy warrior power /2 Soldier attack power
                enemyCardSO[0].health = (int)(cardManager.cardList[0].cardSO.health / 1.6f); //Enemy warrior health /3 Soldier health;

                archer = 16;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //archer = (int)(cardManager.maxMana / cardManager.cardList[1].currentManaCost * 0.5f);
                enemyCardSO[1].attackDamage = cardManager.cardList[1].cardSO.attackDamage / 1.6f; //Enemy archer power /2 Archer attack power
                enemyCardSO[1].health = (int)(cardManager.cardList[1].cardSO.health / 1.6f); //Enemy archer health /2 Archer health;

                giant = 8;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //giant = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                enemyCardSO[2].attackDamage = cardManager.cardList[2].cardSO.attackDamage / 1.6f; //Enemy giant power /2 Giant attack power
                enemyCardSO[2].health = (int)(cardManager.cardList[2].cardSO.health / 1.6f); //Enemy giant health /2 Giant health;

                break;
            case 3: //Hardness 3

                warrior = 120;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //warrior = cardManager.maxMana / cardManager.cardList[0].currentManaCost;
                enemyCardSO[0].attackDamage = cardManager.cardList[0].cardSO.attackDamage / 1.2f; //Enemy warrior attack power /1.5 Soldier attack power
                enemyCardSO[0].health = (int)(cardManager.cardList[0].cardSO.health / 1.2f); //Enemy warrior health /1.5 Soldier health;

                archer = 20;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //archer = (int)(cardManager.maxMana / cardManager.cardList[1].currentManaCost * 0.5f);
                enemyCardSO[1].attackDamage = cardManager.cardList[1].cardSO.attackDamage / 1.2f;  //Enemy archer power /1.5 Archer attack power
                enemyCardSO[1].health = (int)(cardManager.cardList[1].cardSO.health / 1.2f); //Enemy archer health /1.5 Archer health;


                giant = 10;

                //If we want to spawn enemies according to the player's max mana value, turn this code on.
                // ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓
                //giant = (int)(cardManager.maxMana / cardManager.cardList[2].currentManaCost * 0.5f);
                enemyCardSO[2].attackDamage = cardManager.cardList[2].cardSO.attackDamage / 1.2f; //Enemy giant attack power /1.5 Giant attack power
                enemyCardSO[2].health = (int)(cardManager.cardList[2].cardSO.health / 1.2f); //Enemy giant health /1.5 Giant health;
                break;
            default:
                break;
        }
    }
}
