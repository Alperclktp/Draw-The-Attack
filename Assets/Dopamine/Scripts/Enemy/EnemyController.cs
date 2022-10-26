using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : BaseAttackController
{
    [Space(10)] public EnemyCardSO enemyCardSO;

    [HideInInspector] public Animator anim;

    private NavMeshAgent agent;

    [SerializeField] private bool canMove;
    [SerializeField] private bool canAttack;
    public bool canGoEndLine;

    [Header("Character Stats")]
    public float currentHealth;
    public float currentSpeed;

    [Header("Follow Settings")]
    public float sensingDistance;
    public float stoppingDistance;
    public float lookRotateSpeed;

    public int amountToGiveMana;

    [Header("UI")]
    public GameObject enemyCanvas;
    //public Text giveToManaCountText;

    public override string damageableID { get { return typeof(EnemyController).Name; } }

    private void Awake()
    {
        Upgrade();

        GetCardData();

        anim = GetComponentInChildren<Animator>();

        AnimationOffset();

        agent = GetComponent<NavMeshAgent>();

        agent.speed = currentSpeed;

        agent.stoppingDistance = stoppingDistance;

        //giveToManaCountText.text = "+" + amountToGiveMana.ToString();

    }

    private void Update()
    {
        GetClosesSoldier();

        if (nearestTarget != null)
        {
            FollowSoldier();
        }
        else
        {
            MoveEndLine();
        }

        CheckHealth();

        CheckSoldierList();

        InýtAnimation();
    }

    private void OnEnable()
    {
        EventManager.onUpgrade += Upgrade;
    }

    private void OnDisable()
    {
        EventManager.onUpgrade -= Upgrade;
    }

    public void Upgrade()
    {
        enemyCardSO.attackDamage += enemyCardSO.powerAdd * UpgradeManager.Instance.upgradeCount;
        enemyCardSO.health += (int)enemyCardSO.powerHealthAdd * UpgradeManager.Instance.upgradeCount;
        enemyCardSO.movementSpeed += enemyCardSO.powerSpeedAdd * UpgradeManager.Instance.upgradeCount;
    }

    private void MoveEndLine()
    {
        //transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.endLinePosition.position, currentSpeed * Time.deltaTime);

        agent.SetDestination(GameManager.Instance.failLinePosition.position);

        agent.isStopped = false;

        canAttack = false;
        canMove = true;
        canGoEndLine = true;

        LookAtEndPosition();
    }

    private Transform GetClosesSoldier()
    {
        float minDist = sensingDistance;

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
        Destroy(VFXManager.SpawnEffect(VFXType.EXPLOSION_EFFECT, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity), 1);

        Destroy(this.gameObject);
    }

    private void RemoveEnemyFromList()
    {
        GameManager.Instance.enemyList.Remove(this.gameObject);
    }

    private void AddToMana(int amount)
    {
        CardManager.Instance.currentMana += amount;

        //CardManager.Instance.currentMana += Mathf.Lerp(CardManager.Instance.currentMana, amount, Time.deltaTime);

        Destroy(Instantiate(enemyCanvas, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity), 1);
    }

    private void FollowSoldier()
    {
        if (nearestTarget != null)
        {
            if (Vector3.Distance(transform.position, nearestTarget.position) >= agent.stoppingDistance)
            {
                //transform.position = Vector3.MoveTowards(transform.position, nearestTarget.position, currentSpeed * Time.deltaTime);

                agent.SetDestination(nearestTarget.position);

                canAttack = false;
                canGoEndLine = false;
                canMove = true;

                LookAtSoldier();
            }
            else
            {
                LookAtSoldier();

                canMove = false;
                canGoEndLine = false;
                canAttack = true;
                agent.isStopped = true;
            }
        }
        
        /*
        agent.SetDestination(nearestTarget.position);

        if ((transform.position - nearestTarget.position).magnitude < agent.stoppingDistance)
        {
            LookAtSoldier();

            canMove = false;
            canGoEndLine = false;
            canAttack = true;
        }
        else
        {
            LookAtSoldier();

            canAttack = false;
            canGoEndLine = true;
            canMove = true;
        }
        */
    }

    private void LookAtSoldier()
    {
        //transform.LookAt(nearestTarget);

        Quaternion lookRotation = Quaternion.LookRotation(nearestTarget.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotateSpeed);
    }

    private void LookAtEndPosition()
    {
        transform.LookAt(GameManager.Instance.failLinePosition);
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

    private void AnimationOffset()
    {
        anim.SetFloat("Offset", Random.Range(0f, 1f));
    }

    private void CheckSoldierList()
    {
        if (GameManager.Instance.soldierList.Count <= 0)
        {
        
        }
    }

    private void GetCardData()
    {
        currentAttackDamage = enemyCardSO.attackDamage;

        currentPerAttackSpeed = enemyCardSO.AttackPerSpeed;

        currentSpeed = enemyCardSO.movementSpeed;

        currentHealth = enemyCardSO.health;
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}

