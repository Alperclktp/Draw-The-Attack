using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : BaseAttackController
{
    [Space(10)] public CardSO cardSO;

    [HideInInspector] public Animator anim;

    [SerializeField] private bool canMove;
    [SerializeField] private bool canAttack;

    [Header("Character Stats")]
    public float currentHealth;
    [SerializeField] private float currentSpeed;

    [Header("Follow Settings")] 
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

        CheckHealth();

        InýtAnimation();

        GetClosesEnemy();

        FollowTarget();

        CheckEnemyList();
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

    private void CheckHealth()
    {
        if(currentHealth <= 0)
        {
            DestroySoldier();

            RemoveSoldierFromlist();
        }
    }

    private void DestroySoldier()
    {
        Destroy(this.gameObject);
    }

    private void RemoveSoldierFromlist()
    {
        GameManager.Instance.soldierList.Remove(this.gameObject);
    }

    private void FollowTarget()
    {
        if(nearestTarget != null)
        {
            if (Vector3.Distance(transform.position, nearestTarget.position) >= stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, nearestTarget.position, currentSpeed * Time.deltaTime);

                canMove = true;
                canAttack = false;

                LookAtEnemy();
            }
            else
            {
                LookAtEnemy();

                canMove = false;
                canAttack = true;
            }
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

    private void CheckEnemyList()
    {
        if(GameManager.Instance.enemyList.Count <= 0)
        {
            canAttack = false;
            canMove = false;

            anim.Play("Win");
        }
    }

    private void GetCardData()
    {
        currentAttackDamage = cardSO.AttackDamage;

        currentPerAttackSpeed = cardSO.AttackPerSpeed;

        currentSpeed = cardSO.MovementSpeed;

        currentHealth = cardSO.Health;
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
