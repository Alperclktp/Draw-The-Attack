using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public CardSO cardSO;

    [HideInInspector] public Animator anim;

    [SerializeField] private float speed = 5f;

    [SerializeField] private bool canMove;
    [SerializeField] private bool canAttack;

    [Header("Character Stats")]
    public float currentAttackDamage;
    public float currentPerAttackSpeed;
    public int currentHealth;
    
    [Header("Follow Settings")]
    public Transform nearestTarget = null;

    public float stoppingDistance;

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

        GetClosesEnemy();

        FollowTarget();
    }

    private void Movement()
    {
        //transform.position += -transform.forward * Time.deltaTime * speed;
    }
    private Transform GetClosesEnemy()
    {
        float minDist = Mathf.Infinity;

        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in GameManager.Instance.enemyList)
        {
            float dist = Vector3.Distance(obj.transform.position, currentPosition);

            if(dist < minDist)
            {
                nearestTarget = obj.transform;
                minDist = dist;
            }
        }
        return nearestTarget;
    }

    private void FollowTarget()
    {
        if(Vector3.Distance(transform.position, nearestTarget.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, nearestTarget.position, speed * Time.deltaTime);

            canMove = true;
            canAttack = false;

            LookAtEnemy();
        }
        else
        {
            LookAtEnemy();

            //Attack enemy; 

            Debug.Log("Attack!");

            canMove = false;
            canAttack = true;
        }
    }
    private void LookAtEnemy()
    {
        transform.LookAt(nearestTarget);
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

        if (canAttack)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Attack",true);
        }
        else
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);
        }
    }

    private void GetCardData()
    {
        currentAttackDamage = cardSO.AttackDamage;
        currentPerAttackSpeed = cardSO.AttackPerSpeed;

        currentHealth = cardSO.Health;
    }

}
