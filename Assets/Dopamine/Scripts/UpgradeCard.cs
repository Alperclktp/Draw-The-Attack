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
    [SerializeField] private Text currentHealthText;
    [SerializeField] private Text currentAttackDamageText;
    [SerializeField] private Text neededUpgradeMoneyText;

    [SerializeField] private Image moneyIcon;

    [ReadOnly] [SerializeField] private int currentLevel = 1;
    [ReadOnly] [SerializeField] private int currentHealth;
    [ReadOnly] [SerializeField] private float currentAttackDamage;

    [ReadOnly] [SerializeField] private Color moneyDefaultColor;
    [ReadOnly] [SerializeField] private Color moneyAlphaColor;

    private void Start()
    {
        GetCardData();
    }

    private void Update()
    {
        CheckUpgradeButtonInteractable();

        currentLevelText.text = "LEVEL " + cardSO.level.ToString();
        currentHealthText.text = cardSO.health.ToString();
        currentAttackDamageText.text = cardSO.attackDamage.ToString();

        neededUpgradeMoneyText.text = neededUpgradeMoney.ToString();
    }

    [Button("Reset Card Data")]
    public void ResetCardLevel()
    {
        cardSO.level = 1;
        cardSO.health = 100;
        cardSO.attackDamage = 10;

        GetCardData();
    }

    public void UpgradeCardButton() //Upgrade card.
    {
        //Bizim kartlar�m�z y�kseldik�e kar�� d��man�n da hasar� bizden 0.5 az olacak �ekilde artacak.
        /*
        for (int i = 0; i < UpgradeManager.Instance.enemyCardSOs.Count; i++)
        {
            UpgradeManager.Instance.enemyCardSOs[i].level += 1;
            UpgradeManager.Instance.enemyCardSOs[i].attackDamage += 10;
            UpgradeManager.Instance.enemyCardSOs[i].health += 10;
        }
        */

        cardSO.level += 1;  
        cardSO.attackDamage += 10;
        cardSO.health += 10;

        UpgradeCardAnimation();

        GameManager.Instance.MoneySpendAnimation();

        GameManager.Instance.currentMoney -= neededUpgradeMoney;

        neededUpgradeMoney += 15;

        GetCardData();
    }

    private void GetCardData()
    {
        currentLevel = cardSO.level;
        currentHealth = cardSO.health;
        currentAttackDamage = cardSO.attackDamage;
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
        transform.DOScale(1.25f, 0.1f).OnComplete(() =>
        {
            transform.DOScale(1.2f, 0.1f);
        });
    }

}
