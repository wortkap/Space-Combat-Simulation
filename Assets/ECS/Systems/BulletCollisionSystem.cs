using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct BulletCollisionSystem : ISystem
{
    [BurstCompile]

    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        var ships = GameObject.FindObjectsByType<Ship>(FindObjectsSortMode.None);

        foreach (var (bulletTransform, bullet, direction, faction, entity)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO<Bullet>,
                RefRO<Direction>,
                RefRO<Faction>>()
                .WithEntityAccess())
        {
            float3 bulletPos = bulletTransform.ValueRO.Position;

            foreach (var ship in ships)
            {
                if (ship == null || (int)ship.Affiliation == faction.ValueRO.Value)
                    continue;

                float distance = math.distance(bulletPos, ship.transform.position);

                if (distance < 0.1f)
                {
                    ship.ReceiveDamage(bullet.ValueRO.Damage);
                    ecb.DestroyEntity(entity);
                    break;
                }
            }
        }

        ecb.Playback(state.EntityManager);
    }
}
