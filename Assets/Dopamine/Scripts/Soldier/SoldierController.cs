using MoreMountains.NiceVibrations;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierController : BaseAttackController
{
    [Space(10)] public CardSO cardSO;

    [HideInInspector] public Animator anim;

    private NavMeshAgent agent;


    [SerializeField] private bool ignoreEnemies;

    [SerializeField] private bool canMove;
    [SerializeField] private bool canAttack;
    public bool canGoTower;

    [Header("Character Stats")]
    public float currentHealth;
    [SerializeField] private float currentSpeed;

    [Header("Follow Settings")]
    public float sensingDistance;
    public float stoppingDistance;
    public float towerStoppingDistance = 4.25f;
    public float lookRotateSpeed;

    public override string damageableID { get { return typeof(SoldierController).Name; } }

    private Rigidbody rb;

    private void Awake()
    {
        GetCardData();

        anim = GetComponentInChildren<Animator>();

        AnimationOffset();

        agent = GetComponent<NavMeshAgent>();

        agent.speed = currentSpeed;

        agent.stoppingDistance = stoppingDistance;

        rb = GetComponent<Rigidbody>();

        //print($"Your card health is {cardSO.health}");
        //print($"Your card damage is {cardSO.attackDamage}");

    }

    private void Update()
    {
        if (!GameManager.Instance.levelComplete)
        {
            GetClosesEnemy();
        }
        else
        {
            Win(); //Test function
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

        rb.isKinematic = (!canMove || agent.isStopped);
    }

    private void LateUpdate()
    {
        Vector3 rot = transform.localEulerAngles;
        rot.x = 0;
        transform.localEulerAngles = rot;
    }

    private void MoveTower()
    {
        nearestTarget = TowerManager.NearestTower(transform.position);
        agent.SetDestination(Vector3.Scale(TowerManager.NearestTower(transform.position).position, new Vector3(1, 0, 1)));

        //agent.isStopped = false;

        agent.stoppingDistance = stoppingDistance;

        canAttack = false;
        canMove = true;
        canGoTower = true;

        LookAtTower();

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, out RaycastHit hit, 1.5f, LayerMask.GetMask("Soldier"), QueryTriggerInteraction.Ignore)) {

            if (transform.position.x > 0)
                transform.position += Vector3.right * Time.deltaTime * 0.5f;
            else
                transform.position += Vector3.left * Time.deltaTime * 0.5f;
        }
    }

    private Transform GetClosesEnemy()
    {
        float minDist = sensingDistance;

        Vector3 currentPosition = transform.position;

        List<GameObject> enemyList = GameManager.Instance.enemyList;

        enemyList = enemyList.Where(_ => _ != null).ToList();

        if (ignoreEnemies)
            enemyList = enemyList.Where(_ => _.name.Contains("Tower")).ToList();

        foreach (GameObject obj in enemyList)
        {
            float dist = Vector3.Distance(obj.transform.position, currentPosition);

            if (dist <= minDist)
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
            float stopDist = stoppingDistance;


            if (nearestTarget.parent == TowerManager.Instance.transform) {

                stopDist = towerStoppingDistance;
                nearestTarget = nearestTarget.Find("GoTowerPosition");
            }

            bool move = Vector3.Distance(transform.position, new Vector3(nearestTarget.position.x, transform.position.y, nearestTarget.position.z)) >= stopDist;

            agent.stoppingDistance = stopDist - 0.1f;

            if (move)
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
                //agent.isStopped = true;
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
        transform.LookAt(TowerManager.NearestTower(transform.position));
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

    private void CheckEnemyList()
    {
        if (GameManager.Instance.enemyList.Count <= 0)
        {

        }
    }

    private void Win() 
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

    public override void TakeDamage(float damage, Vector3?pos = null)
    {
        currentHealth -= damage;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out SoldierController soldierController ))
        {
            currentSpeed = 5f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out SoldierController soldierController))
        {
            currentSpeed = 2f;
        }
    }
}
