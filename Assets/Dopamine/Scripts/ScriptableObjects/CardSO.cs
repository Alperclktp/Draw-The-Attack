using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Create Card", order = 1)]
public class CardSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject cardPrefab;

    [Header("Card Properties")]
    [SerializeField] private int id = default;
    public int ID { get { return id; } }

    [SerializeField] private new string name;
    public string Name { get { return name; } }

    [SerializeField] [TextArea(4,4)] private string description;
    public string Description { get { return description; } }

    [SerializeField] private int manaCost;
    public int ManaCost { get { return manaCost; } }

    [SerializeField] private float attackDamage;
    public float AttackDamage { get { return attackDamage; } }

    [SerializeField] private float attackPerSpeed;
    public float AttackPerSpeed { get { return attackPerSpeed; } }

    [SerializeField] private float movementSpeed;
    public float MovementSpeed { get { return movementSpeed; } }

    [SerializeField] private int health;
    public int Health { get { return health; } }

    [Header("Card UI")]
    [SerializeField] private Sprite artwork;
    public Sprite ArtWork { get { return artwork; } }
}
