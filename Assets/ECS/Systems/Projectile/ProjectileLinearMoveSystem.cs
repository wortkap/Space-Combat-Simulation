using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

// Transform + Velocity
// Move forward
partial struct ProjectileLinearMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        foreach (var (transform, velocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Velocity>>())
        {
            transform.ValueRW.Position += dt * velocity.ValueRO.Value;
        }
    }
}
