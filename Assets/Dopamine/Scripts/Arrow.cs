using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform nearestTarget;

    public float speed = 10f;

    public float currentArrowAttackDamage;

    private void Update()
    {
        //GetClosesEnemy();

        FollowEnemy();
    }

    private void FollowEnemy()
    {
        if(nearestTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, nearestTarget.position + new Vector3(0, 1, 0), speed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /*
    private Transform GetClosesEnemy()
    {
        float minDist = Mathf.Infinity;

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
    */


    private void OnTriggerEnter(Collider other)
    {
        IDamageable hit = other.GetComponent<IDamageable>();

        if (hit != null)
        {
            hit.TakeDamage(currentArrowAttackDamage);

            Destroy(this.gameObject);
        }
    }
}
