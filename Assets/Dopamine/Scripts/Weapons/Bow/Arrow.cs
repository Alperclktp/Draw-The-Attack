using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform nearestTarget;

    [Header("Arrow Settings")]
    public float throwSpeed;
    public float currentArrowAttackDamage;

    public BaseAttackController baseAttackController;  
    private void Update()
    {
        FollowEnemy();
    }
    private void FollowEnemy()
    {
        if(nearestTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, nearestTarget.position + new Vector3(0,1,0), throwSpeed * Time.deltaTime);     
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable hit = other.GetComponent<IDamageable>();

        if (hit != null)
        {
            if(hit.damageableID == baseAttackController.damageableID)
            {
                return;
            }

            hit.TakeDamage(currentArrowAttackDamage);

            GetHitVFX(other);

            Destroy(this.gameObject);
        }
    }
    public void GetHitVFX(Collider other)
    {
        VFXManager.SpawnEffect(VFXType.CARD_HIT_EFFECT, other.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }

}
