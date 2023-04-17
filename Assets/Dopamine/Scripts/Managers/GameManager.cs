using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
using NaughtyAttributes;

public enum GameState
{
    DEFAULT,
    START,
    PAUSE,
    FAIL,
    COMPLATE
}

[DefaultExecutionOrder(-2)]
public class GameManager : Singleton<GameManager>
{
    const bool SPAWN_ENEMIES = true;

    /////////////////////////////////////////////////////

    public List<GameObject> soldierList = new List<GameObject>();

    public List<GameObject> enemyList = new List<GameObject>();

    public Transform failLinePosition;

    public Transform towerPosition;

    [Header("Game State")]
    public GameState gameState;

    public bool levelComplete;

    public int level, currentLevel;

    public int currentMoney;

    [Header("Mana Settings")]
    public int currentMana;
    public int maxMana;
    [ReadOnly] public int limitedMaxMana;

    [Header("UI Elements")]
    public GameObject upgradeManagerPanel;
    public GameObject cardPanel;
    public GameObject startButton;
    public GameObject restartButton;
    public GameObject restartGameButton;
    public GameObject nextLevelButton;
    public GameObject moneyPanel;

    public Slider manaSliderBar;

    public Text currentManaText;
    public Text maxManaText;

    [Header("Tutorial UI")]
    public GameObject spawnAreaIndicator;
    public GameObject selectTutorialHand;
    public GameObject selectTutorialHand2;
    public GameObject selectTutorialHand3;
    public GameObject drawCardTutorialHand;
    public GameObject upgradeTutorialHand;

    public Text currentMoneyText;
    public Text drawToCardText1;
    public Text drawToCardText2;
    public Text drawToCardText3;
    public Text currentLevelText;

    public bool tutorial;

    public bool canManaUpgrade;

    public bool isFinishReported;

    private void Start()
    {
        GetMoney();

        level = PlayerPrefs.GetInt("Level");

        currentMoneyText.text = "$" + currentMoney.ToString();

        currentLevelText.text = $"Level {level + 1}";

        currentMana = maxMana;

        manaSliderBar.maxValue = maxMana;
        manaSliderBar.value = maxMana;

        limitedMaxMana = 600;

        currentLevelText.gameObject.SetActive(true);

        if (level == 0)
        {
            maxMana = 60;
            tutorial = true;
        }
        else
        {
            maxMana = 60;
            tutorial = false;
        }
        if (level == 2)
        {
            Invoke("OnUpgradeTutorialCardHand", 1f);
        }
        else if (level > 2)
        {
            upgradeTutorialHand.SetActive(false);
            upgradeTutorialHand.SetActive(false);
        }
        else
        {
            upgradeManagerPanel.SetActive(false);
            upgradeTutorialHand.SetActive(false);
        }

        //SetTowerHealth();
    }


    private void Update()
    {
        currentMoneyText.text = "$" + currentMoney.ToString();

        SetMoney();

        CheckMoney();

        CheckMana();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //TowerManager.Instance.currentTowerHealth = 0;

            TowerManager.Instance.transform.GetChild(0).GetComponentInParent<TowerManager>().currentTower1Health = 0;
            TowerManager.Instance.transform.GetChild(1).GetComponentInParent<TowerManager>().currentTower2Health = 0;
            TowerManager.Instance.transform.GetChild(2).GetComponentInParent<TowerManager>().currentTower3Health = 0;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            currentMoney = 999999;
        }

        if (maxMana >= limitedMaxMana)
        {
            maxMana = limitedMaxMana;
            //LevelManager.Instance.levelSOTemplate.hardnessPerLevel = 0;
        }
    }

    public void StartGame()
    {
        gameState = GameState.START;

        TowerManager.Instance.canSpawn = true;                  //---CAN SPAWN ENEMY CONTROL--

        if (SPAWN_ENEMIES)
            StartCoroutine(TowerManager.Instance.IEEnemySpawner());

        upgradeManagerPanel.SetActive(false);

        cardPanel.GetComponent<Animator>().enabled = true;

        startButton.SetActive(false);

        moneyPanel.SetActive(false);

        currentLevelText.gameObject.SetActive(false);

        upgradeTutorialHand.SetActive(false);

        StartCoroutine(Tutorial());

        if (maxMana < limitedMaxMana)
        {
            maxMana += level * 15;
        }

        if (maxMana >= limitedMaxMana)
        {
            if (!PlayerPrefs.HasKey("MaxedLevel"))
            {
                PlayerPrefs.SetInt("MaxedLevel", level);
            }
        }

        GetLevelHardness();

        //maxMana += level * level / (int) 1f; //Test mana.
        currentMana = maxMana;

        //AppMetrica

        if (PlayerPrefs.GetInt("am_level_number_defeatblock", 0) == 0)
            PlayerPrefs.SetInt("am_level_number", PlayerPrefs.GetInt("am_level_number", 0) + 1);
        else PlayerPrefs.SetInt("am_level_number_defeatblock", 0);

        PlayerPrefs.SetString("am_result", "win");
        PlayerPrefs.SetInt("am_level_count", PlayerPrefs.GetInt("am_level_count", 0) + 1);

        ReportAppMetricaEvents();
    }


    public void FailLevel()
    {
        gameState = GameState.FAIL;

        TowerManager.Instance.canSpawn = false;

        cardPanel.SetActive(false);

        selectTutorialHand.SetActive(false);

        restartButton.SetActive(true);

        EarnMoney(Random.Range(3, 6));

        MMVibrationManager.Haptic(HapticTypes.Failure, true, this);
    }

    public void RestartGame()
    {
        string currentSceneLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneLevel);

        MMVibrationManager.Haptic(HapticTypes.Failure, true, this);
    }

    public void NextLevel()
    {
        level++;
        PlayerPrefs.SetInt("Level", level);

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);

        EarnMoney(Random.Range(5, 10)); //Amount of money to be earned

        IncreaseMaxMana(10);

        MMVibrationManager.Haptic(HapticTypes.Success, true, this);

        RestartGame();
    }

    public void ReportAppMetricaEvents() {

        AppMetrica.Instance.ReportEvent("level_start ", new Dictionary<string, object> {

            { "level_number", PlayerPrefs.GetInt("am_level_number") },
            { "am_level_count", PlayerPrefs.GetInt("am_level_count") }
        });

        AppMetrica.Instance.ReportEvent("level_finish", new Dictionary<string, object> {

            { "level_number", PlayerPrefs.GetInt("am_level_number") },
            { "am_level_count", PlayerPrefs.GetInt("am_level_count") },
            { "result", PlayerPrefs.GetString("am_result", "win") }
        });

        Debug.Log("<b><color=green>am_level_number:</color></b> " + PlayerPrefs.GetInt("am_level_number"));
        Debug.Log("<b><color=green>am_level_count:</color></b> " + PlayerPrefs.GetInt("am_level_count"));

        AppMetrica.Instance.SendEventsBuffer();
    }

    /*
    public void AppMetricaReportStartEvent()
    {
        AppMetrica.Instance.ReportEvent("level_start ", new Dictionary<string, object>
        {
            { "level_number", level + 1 }
        });

        AppMetrica.Instance.SendEventsBuffer();

        Debug.Log("AppMetricaLevelData: Level Started : " + (level + 1));     
    }

    public void AppMetricaReportFinishEvent()
    {
        AppMetrica.Instance.ReportEvent("level_finished", new Dictionary<string, object>
        {
             { "level_number", level + 1 }
        });

        AppMetrica.Instance.SendEventsBuffer();

        Debug.Log("AppMetricaLevelData: Level Finished : " + (level + 1));
    }
    */

    public void SetTowerHealth()
    {
        TowerManager towerManager = TowerManager.Instance;

        float hardness = 1 + level * LevelManager.Instance.levelSOTemplate.hardnessPerLevel;

        towerManager.currentTower1Health = 300 * hardness;
        towerManager.currentTower2Health = 500 * hardness;
        towerManager.currentTower3Health = 300 * hardness;
    }

    public void SetMoney()
    {
        PlayerPrefs.SetInt("CurrentMoney", currentMoney);
    }

    public void GetMoney()
    {
        currentMoney = PlayerPrefs.GetInt("CurrentMoney");
    }

    public void GetLevelHardness()
    {
        try
        {
            TowerManager.Instance.levelDifficulty.SetHardness();       //---HARDNESS CONTROL---
                                                                       //TowerManager.Instance.levelDifficulty.SetHardness(LevelManager.Instance.levelSOTemplate.levels[PlayerPrefs.GetInt("CurrentLevel")].hardness);       //---HARDNESS CONTROL---
        }
        catch { }
    }

    public void MoneySpendAnimation()
    {
        moneyPanel.transform.DOScale(1.02f, 0.1f).OnComplete(() =>
        {
            moneyPanel.transform.DOScale(1f, 0.1f);
        });
    }

    public void EarnMoney(int amount)
    {
        currentMoney += amount;
    }

    public void IncreaseMaxMana(int amount)
    {
        maxMana += amount;
    }

    public void DecreaseMoney(int amount)
    {
        currentMoney -= amount;

        SetMoney();
    }

    private void CheckMana()
    {
        manaSliderBar.value = currentMana;

        manaSliderBar.maxValue = maxMana;

        currentManaText.text = currentMana.ToString();
        maxManaText.text = maxMana.ToString();

        if (currentMana <= 0)
        {
            currentMana = 0;
        }

        if (currentMana >= maxMana)
        {
            currentMana = maxMana;
        }
    }

    private void CheckMoney()
    {
        if (currentMoney <= 0)
        {
            currentMoney = 0;
        }
    }

    private void OnUpgradeTutorialCardHand()
    {
        upgradeTutorialHand.SetActive(true);
    }

    const bool CONSTANT_SPAWN_AREA_INDICATOR = false;

    private IEnumerator Tutorial()
    {
        if (CONSTANT_SPAWN_AREA_INDICATOR)
            spawnAreaIndicator.SetActive(true);

        if (tutorial)
        {
            SetCards(false, false, false);

            CardManager.Instance.onTutorial = true;

            yield return new WaitForSeconds(0.65f);

            SetCards(true, false, false);
            selectTutorialHand.SetActive(true);

            yield return new WaitUntil(() => IsCardselected(0));

            CardManager.Instance.tutorialLock = false;
            SetCards(false, false, false);
            selectTutorialHand.SetActive(false);
            drawCardTutorialHand.SetActive(true);
            drawToCardText1.gameObject.SetActive(true);

            if (!CONSTANT_SPAWN_AREA_INDICATOR)
                spawnAreaIndicator.SetActive(true);

            yield return new WaitUntil(() => SoliderCount("Warrior") >= 10);

            CardManager.Instance.tutorialLock = true;
            SetCards(false, true, false);
            selectTutorialHand2.SetActive(true);
            drawCardTutorialHand.SetActive(false);
            drawToCardText1.gameObject.SetActive(false);
            DisableCard(0);

            if (!CONSTANT_SPAWN_AREA_INDICATOR)
                spawnAreaIndicator.SetActive(false);


            yield return new WaitUntil(() => IsCardselected(1));

            CardManager.Instance.tutorialLock = false;
            SetCards(false, false, false);
            selectTutorialHand2.SetActive(false);
            drawCardTutorialHand.SetActive(true);
            drawToCardText2.gameObject.SetActive(true);

            if (!CONSTANT_SPAWN_AREA_INDICATOR)
                spawnAreaIndicator.SetActive(true);

            yield return new WaitUntil(() => SoliderCount("Archer") >= 7);

            CardManager.Instance.tutorialLock = true;
            SetCards(false, false, true);
            selectTutorialHand3.SetActive(true);
            drawCardTutorialHand.SetActive(false);
            drawToCardText2.gameObject.SetActive(false);
            DisableCard(1);

            if (!CONSTANT_SPAWN_AREA_INDICATOR)
                spawnAreaIndicator.SetActive(false);

            yield return new WaitUntil(() => IsCardselected(2));

            CardManager.Instance.tutorialLock = false;
            SetCards(false, false, false);
            selectTutorialHand3.SetActive(false);
            drawCardTutorialHand.SetActive(true);
            drawToCardText3.gameObject.SetActive(true);


            if (!CONSTANT_SPAWN_AREA_INDICATOR)
                spawnAreaIndicator.SetActive(true);

            yield return new WaitUntil(() => SoliderCount("Giant") >= 3);

            SetCards(true, true, true);
            drawCardTutorialHand.SetActive(false);
            drawToCardText3.gameObject.SetActive(false);
            DisableCard(2);

            if (!CONSTANT_SPAWN_AREA_INDICATOR)
                spawnAreaIndicator.SetActive(false);

            CardManager.Instance.onTutorial = false;

            bool IsAnySoldier(string soldier) { return FindObjectsOfType<SoldierController>().Any(_ => _.name.Contains(soldier)); }
            bool IsCardselected(int index) { return CardManager.Instance.cardList[index].IsSelected; }

            int SoliderCount(string soldier) { return FindObjectsOfType<SoldierController>().Where(_ => _.name.Contains(soldier)).Count(); }

            void SetCards(bool card0, bool card1, bool card2)
            {
                CardManager.Instance.cardList[0].GetComponent<Button>().enabled = card0;
                CardManager.Instance.cardList[1].GetComponent<Button>().enabled = card1;
                CardManager.Instance.cardList[2].GetComponent<Button>().enabled = card2;
            }

            void DisableCard(int index)
            {
                //CardManager.Instance.cardList[index].GetComponent<Image>().color = CardManager.Instance.defaultColor;

                CardManager.Instance.cardList[index].IsSelected = false;

                //cardList[i].transform.DOScale(1.05f, 0.2f);
                CardManager.Instance.cardList[index].transform.DOScale(1.2f, 0.2f);
                CardManager.Instance.selectedCard = null;
            }
        }

        if (!CONSTANT_SPAWN_AREA_INDICATOR)
        {
            while (true)
            {
                spawnAreaIndicator.SetActive(CardManager.Instance.cardList.Any(_ => _.IsSelected));
                yield return null;
            }
        }
    }
}