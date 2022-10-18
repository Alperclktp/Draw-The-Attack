using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject arrowPrefab;

    public Transform arrowSpawnPosition;

    private BaseAttackController baseAttackController;

    private void Start()
    {
        baseAttackController = GetComponentInParent<BaseAttackController>();
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

            hit.TakeDamage(baseAttackController.currentAttackDamage);
        }
    }

    public void SetAttack()
    {
        GetComponent<Collider>().enabled = true;
    }

    public void SetAttackEnd()
    {
        GetComponent<Collider>().enabled = false;
    }

    public void SetThrowArrow()
    {
        GameObject obj = Instantiate(arrowPrefab, arrowSpawnPosition.position, arrowPrefab.transform.rotation);

        obj.GetComponent<Arrow>().currentArrowAttackDamage = baseAttackController.currentAttackDamage;

        obj.GetComponent<Arrow>().nearestTarget = baseAttackController.nearestTarget;
    }
}
