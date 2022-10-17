using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardSO cardSO;

    [HideInInspector] public GameObject cardObj;

    [HideInInspector] public GameObject cardPrefab;
    public bool IsSelected { get; set; }

    [Header("Card Data")]
    public int currentID;
    public string currentName;
    public string currentDescription;
    public int currentManaCost;
    
    [Header("UI")]
    public Sprite currentArt;

    public Text nameText;
    private Text manaCostText;

    private void Awake()
    {
        GetCardData();
        GetTextData();
    }

    private void GetCardData()
    {
        cardPrefab = cardSO.cardPrefab;

        currentID = cardSO.ID;

        currentName = cardSO.Name;
        currentDescription = cardSO.Description;

        currentArt = cardSO.ArtWork;

        currentManaCost = cardSO.ManaCost;

        currentArt = cardSO.ArtWork;

        cardObj = this.gameObject;
    }

    private void GetTextData()
    {
        manaCostText = GetComponentInChildren<Text>();

        nameText.text = cardSO.Name.ToString();

        manaCostText.text = cardSO.ManaCost.ToString();
    }
}