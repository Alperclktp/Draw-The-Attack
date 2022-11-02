using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;

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

    public int currentLevel;

    public int currentMoney;

    [Header("UI Elements")]
    public GameObject upgradeManagerPanel;
    public GameObject cardPanel;
    public GameObject startButton;
    public GameObject restartButton;
    public GameObject nextLevelButton;
    public GameObject moneyPanel;
    public GameObject spawnAreaIndicator;
    public GameObject tutorialHand;

    public Text currentMoneyText;
    public Text tapToSpawnText;

    public bool tutorial;

    private void Start()
    {
        GetMoney();

        currentMoneyText.text = "$" + currentMoney.ToString();
    }

    private void Update()
    {
        currentMoneyText.text = "$" + currentMoney.ToString();

        SetMoney();
    }

    public void StartGame() 
    {
        gameState = GameState.START;

        TowerManager.Instance.canSpawn = true;

        StartCoroutine(TowerManager.Instance.IEEnemySpawner());

        upgradeManagerPanel.SetActive(false);

        cardPanel.GetComponent<Animator>().enabled = true;

        startButton.SetActive(false);

        moneyPanel.SetActive(false);

        Tutorial();

        GetLevelHardness();
    }

    public void FailLevel()
    {
        gameState = GameState.FAIL;

        TowerManager.Instance.canSpawn = false;

        cardPanel.SetActive(false);

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
        int currentLevel;
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);

        currentMoney += 250; 

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
        TowerManager.Instance.levelDifficulty.SetHardness(LevelManager.Instance.levelSOTemplate.levels[PlayerPrefs.GetInt("CurrentLevel")].hardness);
    }

    public void MoneySpendAnimation()
    {
        moneyPanel.transform.DOScale(1.02f, 0.1f).OnComplete(() => 
        {
            moneyPanel.transform.DOScale(1f, 0.1f);
        });
    }

    public void DecreaseMoney(int amount)
    {
        currentMoney -= amount;

        SetMoney();
    }

    public void Tutorial()
    {
        if (tutorial)
        {
            tapToSpawnText.gameObject.SetActive(true);
            spawnAreaIndicator.SetActive(true);
            Invoke("OnTutorialHandEnable", 1f);
            
        }
        else
        {
            tapToSpawnText.gameObject.SetActive(false);
            spawnAreaIndicator.SetActive(false);
            tutorialHand.SetActive(false);

        }
    }

    private void OnTutorialHandEnable()
    {
        tutorialHand.SetActive(true);
    }
}
