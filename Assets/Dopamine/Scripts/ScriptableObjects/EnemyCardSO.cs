using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Card", menuName = "Card/Create Enemy Card", order = 1)]
public class EnemyCardSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject cardPrefab;

    [Header("Card Properties")]

    public int level;

    [SerializeField] private int id = default;
    public int ID { get { return id; } }

    [SerializeField] private new string name;
    public string Name { get { return name; } }

    [SerializeField] [TextArea(4, 4)] private string description;
    public string Description { get { return description; } }

    public float attackDamage;

    [SerializeField] private float attackPerSpeed;
    public float AttackPerSpeed { get { return attackPerSpeed; } }

    public float movementSpeed;

    public int health;
    
    /*
    [Header("Power-up Per Upgrade")]
    public float powerAdd;
    public float powerHealthAdd;
    public float powerSpeedAdd;
    */
}
