using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using NaughtyAttributes;

public class UpgradeCard : MonoBehaviour
{
    public CardSO cardSO;

    public int neededUpgradeMoney;

    [Header("UI Elements")]
    [SerializeField] private Text currentLevelText;
    [SerializeField] private Text currentLevelBoxText;
    [SerializeField] private Text nextLevelBoxText;
    [SerializeField] private Text currentHealthText;
    [SerializeField] private Text currentAttackDamageText;
    [SerializeField] private Text neededUpgradeMoneyText;

    [SerializeField] private Image artWorkImage;
    [SerializeField] private Image moneyIcon;

    [Header("Read Only Values")]
    [ReadOnly] [SerializeField] private int currentLevel = 1;
    [ReadOnly] [SerializeField] private int nextLevel;
    [ReadOnly] [SerializeField] private int levelUpCounter;
    [ReadOnly] [SerializeField] private int currentHealth;

    [ReadOnly] [SerializeField] private float currentAttackDamage;

    [ReadOnly] [SerializeField] private Color moneyDefaultColor;
    [ReadOnly] [SerializeField] private Color moneyAlphaColor;

    [SerializeField] private Image[] levelBoxes;

    private void Start()
    {
        nextLevel += currentLevel +1;
        GetCardData();
    }

    private void Update()
    {
        CheckUpgradeButtonInteractable();

        currentLevelText.text = "LEVEL " + cardSO.level.ToString();
        currentLevelBoxText.text = cardSO.level.ToString();
        nextLevelBoxText.text = nextLevel.ToString();
        currentHealthText.text = cardSO.health.ToString();
        currentAttackDamageText.text = cardSO.attackDamage.ToString();

        neededUpgradeMoneyText.text = neededUpgradeMoney.ToString();
    
        if(levelUpCounter <= 0)
        {
            for (int i = 0; i < levelBoxes.Length; i++)
            {
                levelBoxes[i].GetComponent<Image>().color = Color.white;
            }
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
        levelUpCounter+= 1;

        if(levelUpCounter >= 3)
        {
            LevelUp();
        }

        for (int i = 0; i < levelUpCounter; i++)
        {
            levelBoxes[i].GetComponent<Image>().color = Color.green;
        }

        UpgradeManager.Instance.IncreaseUpgradeCount();

        EventManager.onUpgrade?.Invoke();

        /*
        cardSO.attackDamage += 10;
        cardSO.health += 10;
        */

        UpgradeCardAnimation();

        GameManager.Instance.MoneySpendAnimation();

        GameManager.Instance.DecreaseMoney(neededUpgradeMoney);

        neededUpgradeMoney += 15;

        GetCardData();
    }


    private void LevelUp()
    {
        cardSO.level += 1;
        levelUpCounter = 0;
        nextLevel += 1;

        cardSO.attackDamage += 10;
        cardSO.health += 10;

        for (int i = 0; i < 1; i++)
        {
            artWorkImage.sprite = cardSO.artWorks[currentLevel -1];
        }
    }

    private void GetCardData()
    {
        currentLevel = cardSO.level;
        currentHealth = cardSO.health;
        currentAttackDamage = cardSO.attackDamage;

        artWorkImage.sprite = cardSO.artWorks[currentLevel - 1];
    }

    private void CheckUpgradeButtonInteractable()
    {
        if (GameManager.Instance.currentMoney >= neededUpgradeMoney)
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

}
