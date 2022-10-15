using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardSO cardSO;

    [HideInInspector] public Animator anim;

    [SerializeField] private float speed = 5f;

    [SerializeField] private bool canMove;

    [Header("Character Stats")]
    public float currentAttackDamage;
    public float currentPerAttackSpeed;
    public int currentHealth;

    private void Awake()
    {
        GetCardData();

        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (canMove)
        {
            Movement();
        }

        InýtAnimation();
    }

    private void Movement()
    {
        transform.position += -transform.forward * Time.deltaTime * speed;
    }

    private void InýtAnimation()
    {
        if (canMove)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
    private void GetCardData()
    {
        currentAttackDamage = cardSO.AttackDamage;
        currentPerAttackSpeed = cardSO.AttackPerSpeed;

        currentHealth = cardSO.Health;
    }
}
