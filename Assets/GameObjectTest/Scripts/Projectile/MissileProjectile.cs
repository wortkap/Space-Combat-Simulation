using UnityEngine;

public class MissileProjectile : Projectile
{
    protected override void UpdateDirection()
    {
        if (Target == null) 
            return;

        Vector3 desiredDirection = (Target.transform.position - transform.position).normalized;

        direction = desiredDirection;
    }
    protected override void UpdateRotation()
    {
        if (Target == null)
            return;

        transform.LookAt(Target.transform.position);
    }
}
