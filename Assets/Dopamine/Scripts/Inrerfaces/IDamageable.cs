using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage, Vector3? pos = null);

    public string damageableID { get; }

}
