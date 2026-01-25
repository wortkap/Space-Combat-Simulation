using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct ProjectileSpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var fireBuffer in SystemAPI.Query<DynamicBuffer<FireRequest>>())
        {
            for (int i = 0; i < fireBuffer.Length; i++)
            {
                FireRequest req = fireBuffer[i];

                Entity proj = ecb.Instantiate(req.Projectile);

                ecb.SetComponent(proj, new LocalTransform
                {
                    Position = req.FirePosition,
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                ecb.SetComponent(proj, new Velocity
                {
                    Value = req.FireDirection
                });

                // optional: pass target to missiles
                if (req.Target.Entity != Entity.Null || !math.all(req.Target.Location == float3.zero))
                {
                    ecb.SetComponent(proj, req.Target);
                }
            }

            fireBuffer.Clear();
        }

        ecb.Playback(state.EntityManager);
    }
}