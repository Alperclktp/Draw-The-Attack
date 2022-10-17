using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseAttackController
{
    [HideInInspector] public Animator anim;

    [SerializeField] private bool canMove;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canGoEndLine;

    [Header("Character Stats")]
    public float currentHealth;
    public float currentSpeed;

    [Header("Follow Settings")]
    public float stoppingDistance;

    public int amountToGiveMana;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        GetClosesSoldier();

        if(nearestTarget != null)
        {
            FollowSoldier();
        }

        CheckHealth();

        CheckSoldierList();

        if (canGoEndLine & canGoEndLine)
        {
            MovementEndLine();
        }

        InýtAnimation();
    }

    private void MovementEndLine()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.endLinePosition.position, currentSpeed * Time.deltaTime);

        LookAtEndPosition();
    }

    private Transform GetClosesSoldier()
    {
        float minDist = Mathf.Infinity;

        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in GameManager.Instance.soldierList)
        {
            float dist = Vector3.Distance(obj.transform.position, currentPosition);

            if (dist < minDist)
            {
                nearestTarget = obj.transform;
                minDist = dist;
            }
        }
        return nearestTarget;
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            DestroyEnemy();

            RemoveEnemyFromList();

            AddToMana(amountToGiveMana);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    private void RemoveEnemyFromList()
    {
        GameManager.Instance.enemyList.Remove(this.gameObject);
    }

    private void AddToMana(int amount)
    {
        CardManager.Instance.currentMana += amount;
    }

    private void FollowSoldier()
    {
        if (Vector3.Distance(transform.position, nearestTarget.position) >= stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, nearestTarget.position, currentSpeed * Time.deltaTime);

            canAttack = false;
            canGoEndLine = false;

            LookAtSoldier();
        }
        else
        {
            LookAtSoldier();

            canMove = false;
            canGoEndLine = false;
            canAttack = true;
        }
    }

    private void LookAtSoldier()
    {
        transform.LookAt(nearestTarget);
    }

    private void LookAtEndPosition()
    {
        transform.LookAt(GameManager.Instance.endLinePosition);
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
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);
        }
    }

    private void CheckSoldierList()
    {
        if(GameManager.Instance.soldierList.Count <= 0)
        {
            canAttack = false;
            canMove = true;
            canGoEndLine = true;
        }
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}

