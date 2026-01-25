using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ShipMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;

        foreach (var (transform, speed, accel, velocity, target) in
            SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRW<MoveSpeed>,
                RefRO<Acceleration>,
                RefRW<Velocity>,
                RefRO<Target>>()
                .WithAll<ShipTag>())
        {
            if (target.ValueRO.Entity == Entity.Null)
                continue;

            speed.ValueRW.Current = math.min(
                speed.ValueRO.Current + accel.ValueRO.Value * dt,
                speed.ValueRO.Max);

            float3 targetPos = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Entity).Position;
            float3 direction = math.normalize(targetPos - transform.ValueRO.Position);

            velocity.ValueRW.Value = direction * speed.ValueRO.Current;

            transform.ValueRW.Position += dt * velocity.ValueRO.Value;
        }
    }
}
