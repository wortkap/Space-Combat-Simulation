using UnityEngine;

public class BulletPr : Projectile
{
    private void Awake()
    {
        Type = ProjectileType.Bullet;
    }
}
