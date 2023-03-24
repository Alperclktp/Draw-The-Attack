using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using NaughtyAttributes;
using MoreMountains.NiceVibrations;

[DefaultExecutionOrder(-1)]
public class UpgradeCard : MonoBehaviour
{
    public CardSO cardSO;

    [Header("UI Elements")]
    [SerializeField] private Text currentLevelText;
    [SerializeField] private Text currentLevelBoxText;
    [SerializeField] private Text nextLevelBoxText;
    [SerializeField] private Text currentHealthText;
    [SerializeField] private Text currentAttackDamageText;
    [SerializeField] private Text neededUpgradeMoneyText;

    [SerializeField] private Image artWorkImage;
    [SerializeField] private Image cardIcon;
    [SerializeField] private Image moneyIcon;

    [SerializeField] private GameObject levelCheckBox;
    [SerializeField] private GameObject maxLevelObj;

    [SerializeField] private GameObject currentLevelBox;
    [SerializeField] private GameObject nextLevelBox;

    [SerializeField] private GameObject upgradeUpArrow;

    [Header("Read Only Values")]
    [ReadOnly] [SerializeField] private int currentLevel = 1;
    [ReadOnly] public int nextLevel; 
    [ReadOnly] [SerializeField] private int maxLevel;
    [ReadOnly] [SerializeField] private int levelUpCounter;
    [ReadOnly] [SerializeField] private int currentHealth;
    [ReadOnly] [SerializeField] private int neededUpgradeMoney;

    [ReadOnly] [SerializeField] private float currentAttackDamage;

    [ReadOnly] [SerializeField] private Color moneyDefaultColor;
    [ReadOnly] [SerializeField] private Color moneyAlphaColor;
    [ReadOnly] [SerializeField] private Color neededTextDefaultColor;
    [ReadOnly] [SerializeField] private Color neededTextAlphaColor;

    public List<GameObject> levelBoxes;

    private void Awake()
    {
        cardSO.level = 1;
        cardSO.nextLevel = 2;
        cardSO.attackDamage = cardSO.defaultAttackDamage;
        cardSO.health = cardSO.defaultHealth;
        cardSO.neededUpgradeMoney = cardSO.defaultNeededUpgradeMoney;
        cardSO.levelUpCounter = 0;
        cardSO.currentCardPrefab = cardSO.cardPrefabs[0];
    }

    private void Start()
    {
        MaxLevel();
        GetCardData();
        LoadCardData();

        if (PlayerPrefs.GetInt("CurrentLevel") == 0)
        {
            SaveCardDataSO();
            SaveCardData();
        }

        //Debug.Log(PlayerPrefs.GetInt("CurrentCardLevel") + gameObject.name);    

        SaveCardData(); //Test 
        SaveCardDataSO();
    }

    private void Update()
    {
        CheckUpgradeButtonInteractable();

        CheckMaxLevel();

        currentLevelText.text = "LEVEL " + currentLevel.ToString();
        currentLevelBoxText.text = currentLevel.ToString();
        nextLevelBoxText.text = nextLevel.ToString();
        currentHealthText.text = currentHealth.ToString();
        currentAttackDamageText.text = currentAttackDamage.ToString();

        if(currentLevel != maxLevel)
        {
            neededUpgradeMoneyText.text = neededUpgradeMoney.ToString();
        }
        else
        {
            neededUpgradeMoneyText.text = "MAX";
        }   

        if (levelUpCounter <= 0)
        {
            for (int i = 0; i < levelBoxes.Count; i++)
            {
                levelBoxes[i].GetComponent<Image>().color = Color.white;
            }
        }

        for (int i = 0; i < cardSO.levelUpCounter; i++)
        {
            levelBoxes[i].GetComponent<Image>().color = Color.green;
        }

        /*
        for (int i = 0; i < cardSO.cardPrefabs.Length; i++)
        {
            cardSO.currentCardPrefab = cardSO.cardPrefabs[currentLevel -1];
        }
        */

        //Test
        if (currentLevel == 1)
        {
            cardSO.currentCardPrefab = cardSO.cardPrefabs[0];
        }
        else if (currentLevel == 2 || currentLevel == 3)
        {
            cardSO.currentCardPrefab = cardSO.cardPrefabs[1];
        }
        else if (currentLevel >= 4)
        {
            cardSO.currentCardPrefab = cardSO.cardPrefabs[2];
        }
    }

    [Button("Reset Card Level")]
    public void ResetCardLevel()
    {
        cardSO.level = 1;
        cardSO.health = 100;
        cardSO.attackDamage = 10;
     
        GetCardData();
    }

    public void UpgradeCardButton() 
    {
        LoadCardData();

        levelUpCounter += 1;

        SaveCardDataSO();

        if (cardSO.levelUpCounter >= GetComponentInChildren<LevelCheckBoxGenerator>().levelBoxesCount)
        {
            LevelUp();
            SaveCardDataSO();
        }

        for (int i = 0; i < cardSO.levelUpCounter; i++)
        {
            levelBoxes[i].GetComponent<Image>().color = Color.green;
        }

        UpgradeManager.Instance.IncreaseUpgradeCount();

        //EventManager.onUpgrade?.Invoke();

        /*
        cardSO.attackDamage += 10;
        cardSO.health += 10;
        */

        UpgradeCardAnimation();

        GameManager.Instance.MoneySpendAnimation();

        GameManager.Instance.DecreaseMoney(neededUpgradeMoney);

        GameManager.Instance.upgradeTutorialHand.SetActive(false);

        neededUpgradeMoney += 75;

        SaveCardData();
    }

    private void CheckMaxLevel()
    {
        if(currentLevel == maxLevel)
        {
            maxLevelObj.SetActive(true);
            levelCheckBox.SetActive(false);
            currentLevelBox.SetActive(false);
            nextLevelBox.SetActive(false);
        }
    }

    private void LevelUp()
    {
        currentLevel += 1;
        levelUpCounter = 0;
        nextLevel += 1;

        currentAttackDamage += cardSO.addAttackDamage;
        currentHealth += cardSO.addHealth;

        MMVibrationManager.Haptic(HapticTypes.MediumImpact, true, this);

        for (int i = 0; i < 1; i++)
        {
            artWorkImage.sprite = cardSO.artWorks[currentLevel - 1];
            cardIcon.sprite = cardSO.artWorks[currentLevel - 1];

            GetComponentInChildren<LevelCheckBoxGenerator>().AddBoxes(+1);
            //GetComponentInChildren<LevelCheckBoxGenerator>().levelBoxesCount += 1;
        }
    }

    //int clampedLvl => Mathf.FloorToInt(Mathf.Clamp(currentLevel - 1, 0, Mathf.Infinity));

    private void MaxLevel()
    {
        maxLevel = cardSO.artWorks.Length;
    }

    private void GetCardData()
    {
        currentLevel = cardSO.level;
        nextLevel = cardSO.nextLevel;
        levelUpCounter = cardSO.levelUpCounter;

        currentHealth = cardSO.health;
        currentAttackDamage = cardSO.attackDamage;
        neededUpgradeMoney = cardSO.neededUpgradeMoney;

        artWorkImage.sprite = cardSO.artWorks[currentLevel - 1];
        cardIcon.sprite = cardSO.artWorks[currentLevel - 1];
    }

    private void CheckUpgradeButtonInteractable()
    {
        if (GameManager.Instance.currentMoney >= cardSO.neededUpgradeMoney && currentLevel != maxLevel)
        {
            GetComponentInChildren<Button>().interactable = true;

            moneyIcon.color = moneyDefaultColor;

            neededUpgradeMoneyText.color = neededTextDefaultColor;

            upgradeUpArrow.SetActive(true);
        }
        else
        {
            GetComponentInChildren<Button>().interactable = false;

            moneyIcon.color = moneyAlphaColor;

            neededUpgradeMoneyText.color = neededTextAlphaColor;

            upgradeUpArrow.SetActive(false);

        }
    }

    private void UpgradeCardAnimation()
    {
        transform.DOScale(0.85f, 0.1f).OnComplete(() =>
        {
            transform.DOScale(0.8f, 0.1f);
        });
    }

    private void SaveCardData()
    {
        PlayerPrefs.SetInt("CurrentCardLevel" + gameObject.name, currentLevel);
        PlayerPrefs.SetInt("NextLevel" + gameObject.name, nextLevel);
        PlayerPrefs.SetInt("LevelUpCounter" + gameObject.name, levelUpCounter);

        PlayerPrefs.SetInt("CurrentHealth" + gameObject.name, currentHealth);
        PlayerPrefs.SetFloat("CurrentAttackDamage" + gameObject.name, currentAttackDamage);

        PlayerPrefs.SetInt("NeededUpgradeMoney" + gameObject.name, neededUpgradeMoney);

        PlayerPrefs.SetInt("LevelBoxCount" + gameObject.name, levelCheckBox.GetComponent<LevelCheckBoxGenerator>().levelBoxesCount);


        //print($"S {levelCheckBox.GetComponent<LevelCheckBoxGenerator>().levelBoxesCount}");

    }
    private void SaveCardDataSO()
    {
        cardSO.level = currentLevel;
        cardSO.nextLevel = nextLevel;
        cardSO.levelUpCounter = levelUpCounter;

        cardSO.health = currentHealth;
        cardSO.attackDamage = currentAttackDamage;

        cardSO.neededUpgradeMoney = neededUpgradeMoney;

        artWorkImage.sprite = cardSO.artWorks[currentLevel - 1];
        cardIcon.sprite = cardSO.artWorks[currentLevel - 1];
    }

    private void LoadCardData()
    {       
        currentLevel = PlayerPrefs.GetInt("CurrentCardLevel" + gameObject.name, currentLevel);
        nextLevel = currentLevel + 1;

        //nextLevel = PlayerPrefs.GetInt("NextLevel" + gameObject.name, currentLevel);

        levelUpCounter = PlayerPrefs.GetInt("LevelUpCounter" + gameObject.name, levelUpCounter);

        currentHealth = PlayerPrefs.GetInt("CurrentHealth" + gameObject.name, currentHealth);
        currentAttackDamage = PlayerPrefs.GetFloat("CurrentAttackDamage" + gameObject.name, currentAttackDamage);

        neededUpgradeMoney = PlayerPrefs.GetInt("NeededUpgradeMoney" + gameObject.name, neededUpgradeMoney);

        int _levelBoxCount = PlayerPrefs.GetInt("LevelBoxCount" + gameObject.name);

        levelCheckBox.GetComponent<LevelCheckBoxGenerator>().SetBoxes(_levelBoxCount == 0 ? 3 : _levelBoxCount);

        //levelCheckBox.GetComponent<LevelCheckBoxGenerator>().levelBoxesCount = levelBoxCount == 0 ? 3 : levelBoxCount;
    }
}
