using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public Text currentMoneyText;

    private void Awake()
    {
        GetMoney();
    }

    private void Start()
    {
        GetLevelValues();        
    }

    private void Update()
    {
        currentMoneyText.text = "$" + currentMoney.ToString();

        SetMoney();
    }

    public void StartGame() //Test
    {
        gameState = GameState.START;

        TowerManager.Instance.canSpawn = true;

        StartCoroutine(TowerManager.Instance.IEEnemySpawner());

        upgradeManagerPanel.SetActive(false);

        cardPanel.GetComponent<Animator>().enabled = true;

        startButton.SetActive(false);

        moneyPanel.SetActive(false);
    }   

    public void FailLevel()
    {
        gameState = GameState.FAIL;

        cardPanel.SetActive(false);

        restartButton.SetActive(true);

        TowerManager.Instance.canSpawn = false;

    }

    public void RestartGame()
    {
        string currentSceneLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneLevel);
    }

    public void NextLevel() 
    {
        int currentLevel;
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);

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


    public void GetLevelValues()
    {
        TowerManager.Instance.levelDifficulty = LevelManager.Instance.levelSOTemplate.levels[PlayerPrefs.GetInt("CurrentLevel")].difficulty;
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
}
