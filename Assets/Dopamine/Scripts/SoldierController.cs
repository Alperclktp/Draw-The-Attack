using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierController : BaseAttackController
{
    [Space(10)] public CardSO cardSO;

    [HideInInspector] public Animator anim;

    private NavMeshAgent agent;

    [SerializeField] private bool canMove;
    [SerializeField] private bool canAttack;
    public bool canGoTower;

    [Header("Character Stats")]
    public float currentHealth;
    [SerializeField] private float currentSpeed;

    [Header("Follow Settings")]
    public float sensingDistance;
    public float stoppingDistance;
    public float lookRotateSpeed;

    public override string damageableID { get { return typeof(SoldierController).Name; } }

    private void Awake()
    {
        GetCardData();

        anim = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();

        agent.speed = currentSpeed;

        agent.stoppingDistance = stoppingDistance;

    }

    private void Update()
    {
        if (!GameManager.Instance.levelComplete)
        {
            GetClosesEnemy();
        }
        else
        {
            TestWin(); //Test function
        }

        if (nearestTarget != null)
        {
            FollowTarget();
        }
        else
        {
            if (!GameManager.Instance.levelComplete)
            {
                MoveTower();
            }
        }

        CheckHealth();

        InýtAnimation();

        CheckEnemyList();
    }

    private void MoveTower()
    {
        agent.SetDestination(new Vector3(GameManager.Instance.towerPosition.position.x, 0, GameManager.Instance.towerPosition.position.z));

        agent.isStopped = false;

        agent.stoppingDistance = stoppingDistance;

        canAttack = false;
        canMove = true;
        canGoTower = true;

        LookAtTower();
    }

    private Transform GetClosesEnemy()
    {
        float minDist = sensingDistance;

        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in GameManager.Instance.enemyList)
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
            DestroySoldier();

            RemoveSoldierFromlist();
        }
    }

    private void DestroySoldier()
    {
        Destroy(VFXManager.SpawnEffect(VFXType.EXPLOSION_EFFECT, transform.position + new Vector3(0, 1, 0), Quaternion.identity), 1);

        Destroy(this.gameObject);
    }

    private void RemoveSoldierFromlist()
    {
        GameManager.Instance.soldierList.Remove(this.gameObject);
    }

    private void FollowTarget()
    {
        if (nearestTarget != null)
        {
            agent.stoppingDistance = stoppingDistance;

            if (Vector3.Distance(transform.position, new Vector3(nearestTarget.position.x, transform.position.y, nearestTarget.position.z)) > stoppingDistance)
            {
                //transform.position = Vector3.MoveTowards(transform.position, nearestTarget.position, currentSpeed * Time.deltaTime);

                agent.SetDestination(nearestTarget.position);

                canMove = true;
                canGoTower = false;
                canAttack = false;

                LookAtEnemy();
            }
            else
            {
                LookAtEnemy();

                canMove = false;
                canGoTower = false;
                canAttack = true;
                agent.isStopped = true;
            }
        }

        /*
        agent.SetDestination(nearestTarget.position);

        if ((transform.position - nearestTarget.position).magnitude < agent.stoppingDistance)
        {
            LookAtEnemy();

            canMove = false;
            canAttack = true;
        }
        else
        {
            LookAtEnemy();

            canAttack = false;
            canMove = true;
        }
        */
    }

    private void LookAtEnemy()
    {
        //transform.LookAt(nearestTarget);

        Quaternion lookRotation = Quaternion.LookRotation(nearestTarget.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotateSpeed);
    }

    private void LookAtTower()
    {
        transform.LookAt(GameManager.Instance.towerPosition);
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

    private void CheckEnemyList()
    {
        if (GameManager.Instance.enemyList.Count <= 0)
        {

        }
    }

    private void TestWin() //Test fuction.
    {
        anim.Play("Win");
        agent.isStopped = true;
    }

    private void GetCardData()
    {
        currentAttackDamage = cardSO.attackDamage;

        currentPerAttackSpeed = cardSO.AttackPerSpeed;

        currentSpeed = cardSO.MovementSpeed;

        currentHealth = cardSO.health;
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
