using UnityEngine;

public class Bullet : Projectile
{
    private void Awake()
    {
        Type = ProjectileType.Bullet;
    }
}
