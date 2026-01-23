using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ShipShooter : MonoBehaviour
{
    public float BulletSpeed = 10f;
    public float BulletDamage = 10f;
    public float BulletLifetime = 5f;

    public void Fire(Vector3 direction, Affiliation faction)
    {
        var world = World.DefaultGameObjectInjectionWorld;
        var em = world.EntityManager;

        Entity request = em.CreateEntity(
            typeof(BulletSpawnRequest)
        );

        em.SetComponentData(request, new BulletSpawnRequest
        {
            Position = transform.position,
            Direction = direction,
            Speed = BulletSpeed,
            Damage = BulletDamage,
            Faction = (int)faction,
            Lifetime = BulletLifetime
        });
    }
}
