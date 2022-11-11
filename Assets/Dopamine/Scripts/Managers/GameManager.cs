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
public class GameManager : Singleton<GameManager>
{
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

    [Header("UI Elements")]
    public GameObject upgradeManagerPanel;
    public GameObject cardPanel;
    public GameObject startButton;
    public GameObject restartButton;
    public GameObject nextLevelButton;
    public GameObject moneyPanel;

    public Slider manaSliderBar;

    public Text currentManaText;
    public Text maxManaText;

    public GameObject spawnAreaIndicator;
    public GameObject selectTutorialHand;
    public GameObject drawCardTutorialHand;
    public GameObject upgradeTutorialHand;

    public Text currentMoneyText;
    public Text drawToCardText;

    public bool tutorial;

    private void Start()
    {
        GetMoney();

        currentMoneyText.text = "$" + currentMoney.ToString();

        currentMana = maxMana;

        manaSliderBar.maxValue = maxMana;
        manaSliderBar.value = maxMana;

        level = PlayerPrefs.GetInt("Level");

        if (level == 2)
        {
            Invoke("OnUpgradeTutorialCardHand", 1f);
        }
        else if(level > 2)
        {
            upgradeTutorialHand.SetActive(false);
            upgradeTutorialHand.SetActive(false);
        }
        else
        {
            upgradeManagerPanel.SetActive(false);
            upgradeTutorialHand.SetActive(false);
        }
    }

    private void Update()
    {
        currentMoneyText.text = "$" + currentMoney.ToString();

        SetMoney();

        CheckMana();
    }

    public void StartGame() 
    {
        gameState = GameState.START;

        TowerManager.Instance.canSpawn = true;                  //---CAN SPAWN ENEMY CONTROL--

        StartCoroutine(TowerManager.Instance.IEEnemySpawner());

        upgradeManagerPanel.SetActive(false);

        cardPanel.GetComponent<Animator>().enabled = true;

        startButton.SetActive(false);

        moneyPanel.SetActive(false);

        Tutorial();

        GetLevelHardness();

        maxMana += level * 20;
        currentMana = maxMana;
    }

    public void FailLevel()
    {
        gameState = GameState.FAIL;

        TowerManager.Instance.canSpawn = false;

        cardPanel.SetActive(false);

        selectTutorialHand.SetActive(false);

        restartButton.SetActive(true);
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

        EanrMoney(Random.Range(750,800)); //Amount of money to be earned

        IncreaseMaxMana(20);

        MMVibrationManager.Haptic(HapticTypes.Success, true, this);

        RestartGame();
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
        try {
        TowerManager.Instance.levelDifficulty.SetHardness(LevelManager.Instance.levelSOTemplate.levels[PlayerPrefs.GetInt("CurrentLevel")].hardness);       //---HARDNESS CONTROL---
        } catch { }
    }

    public void MoneySpendAnimation()
    {
        moneyPanel.transform.DOScale(1.02f, 0.1f).OnComplete(() => 
        {
            moneyPanel.transform.DOScale(1f, 0.1f);
        });
    }


    public void EanrMoney(int amount)
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
    public void Tutorial()
    {
        if (tutorial)
        {
            Invoke("OnTutorialSelectCardHand", 1f);
            
        }
        else
        {
            drawToCardText.gameObject.SetActive(false);
            spawnAreaIndicator.SetActive(false);
            drawCardTutorialHand.SetActive(false);
            selectTutorialHand.SetActive(false);
        }
    }

    private void OnTutorialSelectCardHand()
    {
        selectTutorialHand.SetActive(true);
    }

    private void OnUpgradeTutorialCardHand()
    {
        upgradeTutorialHand.SetActive(true);
    }

    public void DrawCardTutorialHand()
    {  
        drawCardTutorialHand.SetActive(true);

        spawnAreaIndicator.SetActive(true);

        drawToCardText.gameObject.SetActive(true);

        selectTutorialHand.SetActive(false);
    }
}
