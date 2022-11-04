using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using NaughtyAttributes;
using MoreMountains.NiceVibrations;

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
    [SerializeField] private Image moneyIcon;

    [SerializeField] private GameObject levelCheckBox;
    [SerializeField] private GameObject maxLevelObj;

    [Header("Read Only Values")]
    [ReadOnly] [SerializeField] private int currentLevel = 1;
    [ReadOnly] [SerializeField] private int nextLevel;
    [ReadOnly] [SerializeField] private int maxLevel;
    [ReadOnly] [SerializeField] private int levelUpCounter;
    [ReadOnly] [SerializeField] private int currentHealth;
    [ReadOnly] [SerializeField] private int neededUpgradeMoney;

    [ReadOnly] [SerializeField] private float currentAttackDamage;

    [ReadOnly] [SerializeField] private Color moneyDefaultColor;
    [ReadOnly] [SerializeField] private Color moneyAlphaColor;

    [SerializeField] private Image[] levelBoxes;

    private void Start()
    {
        MaxLevel();

        if (PlayerPrefs.GetInt("CurrentLevel") == 0)
        {
            GetCardData();
            SaveCardDataSO();
            SaveCardData();
        }

        LoadCardData();

        //Debug.Log(PlayerPrefs.GetInt("CurrentCardLevel") + gameObject.name);    

        currentLevel = PlayerPrefs.GetInt("CurrentCardLevel" + gameObject.name);

        nextLevel = currentLevel + 1;

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
            for (int i = 0; i < levelBoxes.Length; i++)
            {
                levelBoxes[i].GetComponent<Image>().color = Color.white;
            }
        }

        for (int i = 0; i < cardSO.levelUpCounter; i++)
        {
            levelBoxes[i].GetComponent<Image>().color = Color.green;
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

        if (cardSO.levelUpCounter >= 3)
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

        neededUpgradeMoney += 15;

        SaveCardData();
    }

    private void CheckMaxLevel()
    {
        if(currentLevel == maxLevel)
        {
            maxLevelObj.SetActive(true);
            levelCheckBox.SetActive(false);
        }
    }

    private void LevelUp()
    {
        currentLevel += 1;
        levelUpCounter = 0;
        nextLevel += 1;

        currentAttackDamage += 10;
        currentHealth += 10;

        MMVibrationManager.Haptic(HapticTypes.MediumImpact, true, this);

        for (int i = 0; i < 1; i++)
        {
            artWorkImage.sprite = cardSO.artWorks[currentLevel -1];
        }
    }

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
    }

    private void CheckUpgradeButtonInteractable()
    {
        if (GameManager.Instance.currentMoney >= cardSO.neededUpgradeMoney && currentLevel != maxLevel)
        {
            GetComponentInChildren<Button>().interactable = true;

            moneyIcon.color = moneyDefaultColor;
        }
        else
        {
            GetComponentInChildren<Button>().interactable = false;

            moneyIcon.color = moneyAlphaColor;
        }
    }

    private void UpgradeCardAnimation()
    {
        transform.DOScale(1.17f, 0.1f).OnComplete(() =>
        {
            transform.DOScale(1.2f, 0.1f);
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
    }

    private void LoadCardData()
    {       
        currentLevel = PlayerPrefs.GetInt("CurrentCardLevel" + gameObject.name);
        nextLevel = PlayerPrefs.GetInt("NextLevel" + gameObject.name);
        levelUpCounter = PlayerPrefs.GetInt("LevelUpCounter" + gameObject.name);

        currentHealth = PlayerPrefs.GetInt("CurrentHealth" + gameObject.name);
        currentAttackDamage = PlayerPrefs.GetFloat("CurrentAttackDamage" + gameObject.name);

        neededUpgradeMoney = PlayerPrefs.GetInt("NeededUpgradeMoney" + gameObject.name, neededUpgradeMoney);
    }
}
