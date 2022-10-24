using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
    public GameObject moneyPanel;
    public Text currentMoneyText;

    private void Start()
    {
       //
    }

    private void Update()
    {
        currentMoneyText.text = "$" + currentMoney.ToString();
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
    public void MoneySpendAnimation()
    {
        moneyPanel.transform.DOScale(1.02f, 0.1f).OnComplete(() => 
        {
            moneyPanel.transform.DOScale(1f, 0.1f);
        });
    }
}
