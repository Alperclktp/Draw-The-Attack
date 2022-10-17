using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{
    public WeaponController weaponController;

    private Animator animator;
    public Animator Animator { get { if (animator == null) animator = GetComponent<Animator>(); return animator; } }

    public void Attack()
    {
        weaponController.SetAttack();
    }

    public void AttackEnd()
    {
        weaponController.SetAttackEnd();
    }

    public void ThrowArrow()
    {
        weaponController.SetThrowArrow();
    }
}
