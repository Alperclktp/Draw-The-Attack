using UnityEngine;

[System.Serializable]
public abstract class BaseAttackController : MonoBehaviour, IDamageable
{
    [Header("Target")]
    public Transform nearestTarget;

    [Header("Attack Stats")]
    public float currentAttackDamage;
    public float currentPerAttackSpeed;

    public abstract string damageableID { get; }

    public abstract void TakeDamage(float damage);


}
