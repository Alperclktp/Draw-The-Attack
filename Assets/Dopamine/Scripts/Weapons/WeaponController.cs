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

            GetHitVFX(other);
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

        Arrow arrow = obj.GetComponent<Arrow>();

        arrow.baseAttackController = baseAttackController;

        arrow.currentArrowAttackDamage = baseAttackController.currentAttackDamage;

        arrow.nearestTarget = baseAttackController.nearestTarget;

    }

    public void GetHitVFX(Collider other)
    {
        VFXManager.SpawnEffect(VFXType.CARD_HIT_EFFECT, other.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }
}
