using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardSO cardSO;

    [HideInInspector] public GameObject cardObj;

    public GameObject cardPrefab;
    public bool IsSelected { get; set; }

    public int currentID;
    public string currentName;
    public string currentDescription;
    public Sprite currentArt;
    public int currentManaCost;
    public float currentAttackDamage;
    public float currentPerAttackSpeed;
    public int currentHealth;

    private Text manaCostText;

    private void Awake()
    {
        cardObj = this.gameObject;

        GetCardData();

        manaCostText = GetComponentInChildren<Text>();

        manaCostText.text = cardSO.ManaCost.ToString();
    }

    private void GetCardData()
    {
        cardPrefab = cardSO.cardPrefab;

        currentID = cardSO.ID;

        currentName = cardSO.Name;
        currentDescription = cardSO.Description;

        currentArt = cardSO.ArtWork;

        currentManaCost = cardSO.ManaCost;

        currentAttackDamage = cardSO.AttackDamage;
        currentPerAttackSpeed = cardSO.AttackPerSpeed;

        currentHealth = cardSO.Health;
    }
}
