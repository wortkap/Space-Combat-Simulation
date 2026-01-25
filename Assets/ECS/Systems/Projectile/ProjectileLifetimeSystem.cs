using Unity.Burst;
using Unity.Entities;

partial struct ProjectileLifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (lifetime, entity) in SystemAPI.Query<RefRW<Lifetime>>().WithEntityAccess())
        {
            lifetime.ValueRW.Value -= dt;

            if (lifetime.ValueRW.Value <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
    }
}
