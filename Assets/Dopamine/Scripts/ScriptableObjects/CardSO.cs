using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Create Soldier Card", order = 1)]
public class CardSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject currentCardPrefab;

    public GameObject[] cardPrefabs;

    [Header("Card Properties")]

    public int level;

    public int nextLevel;

    public int levelUpCounter;

    [SerializeField] private int id = default;
    public int ID { get { return id; } }

    [SerializeField] private new string name;
    public string Name { get { return name; } }

    [SerializeField] [TextArea(4,4)] private string description;
    public string Description { get { return description; } }

    [SerializeField] private int manaCost;
    public int ManaCost { get { return manaCost; } }

    public float attackDamage;

    [SerializeField] private float attackPerSpeed;
    public float AttackPerSpeed { get { return attackPerSpeed; } }

    [SerializeField] private float movementSpeed;
    public float MovementSpeed { get { return movementSpeed; } }

    public int health;

    public int neededUpgradeMoney;

    public int addAttackDamage;
    public int addHealth;

    [Header("Card UI")]
    [SerializeField] private Sprite artwork;
    public Sprite ArtWork { get { return artwork; } }

    public Sprite[] artWorks;

    [Header("Default Values")]
    public int defaultAttackDamage;
    public int defaultHealth;
    public int defaultNeededUpgradeMoney;
}
