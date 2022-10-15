using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Create Card", order = 1)]
public class CardSO : ScriptableObject
{
    public GameObject cardPrefab;

    [SerializeField] private int id = default;
    public int ID { get { return id; } }

    [SerializeField] private new string name;
    public string Name { get { return name; } }

    [SerializeField] private string description;
    public string Description { get { return description; } }

    [SerializeField] private Sprite artwork;
    public Sprite ArtWork { get { return artwork; } }

    [SerializeField] private int manaCost;
    public int ManaCost { get { return manaCost; } }

    [SerializeField] private float attackDamage;
    public float AttackDamage { get { return attackDamage; } }

    [SerializeField] private float attackPerSpeed;
    public float AttackPerSpeed { get { return attackPerSpeed; } }

    [SerializeField] private int health;
    public int Health { get { return health; } }
}
