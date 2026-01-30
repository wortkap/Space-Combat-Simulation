using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct WeaponFireSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (weaponSlots, target, shipTransform, ship) in
            SystemAPI.Query<
                DynamicBuffer<WeaponSlot>,
                RefRO<Target>,
                RefRO<LocalTransform>>()
            .WithEntityAccess())
        {
            var fireBuffer = SystemAPI.GetBuffer<FireRequest>(ship);

            float3 shipPos = shipTransform.ValueRO.Position;

            foreach (var slot in weaponSlots)
            {
                Entity weapon = slot.WeaponPrefab;

                var cooldown = SystemAPI.GetComponentRW<WeaponCooldown>(weapon);
                cooldown.ValueRW.Current -= dt;

                if (cooldown.ValueRO.Current > 0f)
                    continue;

                float3 targetPos;

                if (target.ValueRO.Entity != Entity.Null &&
                    SystemAPI.Exists(target.ValueRO.Entity))
                {
                    targetPos = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Entity).Position;
                }
                else
                {
                    targetPos = target.ValueRO.Location;
                }

                float3 dir = math.normalize(targetPos - shipPos);

                fireBuffer.Add(new FireRequest
                {
                    Projectile = SystemAPI.GetComponent<ProjectilePrefab>(weapon).Value,
                    FirePosition = shipPos,
                    FireDirection = dir,
                    Target = target.ValueRO
                });

                cooldown.ValueRW.Current = cooldown.ValueRO.Max;
            }
        }

        ecb.Playback(state.EntityManager);
    }
}