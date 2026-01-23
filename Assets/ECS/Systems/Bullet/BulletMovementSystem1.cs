using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
partial struct BulletMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        foreach (var (transform, bullet, direction) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Bullet>, RefRO<Direction>>())
        {
            transform.ValueRW.Position += dt * bullet.ValueRO.Speed * direction.ValueRO.Value;
        }
    }
}
