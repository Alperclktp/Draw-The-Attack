using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject arrowPrefab;

    public Transform arrowSpawnPosition;

    private SoldierController soldierController;

    private void Start()
    {
        soldierController = GetComponentInParent<SoldierController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable hit = other.GetComponent<IDamageable>();

        if (hit != null)
        {
            hit.TakeDamage(soldierController.currentAttackDamage);
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

        obj.GetComponent<Arrow>().currentArrowAttackDamage =  soldierController.currentAttackDamage;

        obj.GetComponent<Arrow>().nearestTarget = soldierController.nearestTarget;
    }
}
