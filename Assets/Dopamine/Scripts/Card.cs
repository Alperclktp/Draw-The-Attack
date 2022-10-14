using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardSO cardSO;

    [HideInInspector] public GameObject cardObj;

    public GameObject cardPrefab;

    public bool IsSelected { get; set; }

    [SerializeField] private int currentID;
    [SerializeField] private string currentName;
    [SerializeField] private string currentDescription;
    [SerializeField] private Sprite currentArt;
    [SerializeField] private float currentManaCost;
    [SerializeField] private float currentAttackDamage;
    [SerializeField] private float currentPerAttackSpeed;
    [SerializeField] private int currentHealth;

    private void Awake()
    {
        cardObj = this.gameObject;

        GetCardData();
    }

    public void GetCardData()
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
