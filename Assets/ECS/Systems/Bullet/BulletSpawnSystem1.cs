using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct BulletSpawnSystem : ISystem
{
    private EntityArchetype _bulletArchetype;

    public void OnCreate(ref SystemState state)
    {
        _bulletArchetype = state.EntityManager.CreateArchetype(
            typeof(LocalTransform),
            typeof(Bullet),
            typeof(Lifetime),
            typeof(Direction),
            typeof(Faction),
            typeof(GridCell)
        );
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (request, entity) in SystemAPI.Query<RefRO<BulletSpawnRequest>>().WithEntityAccess())
        {
            Entity bullet = ecb.CreateEntity(_bulletArchetype);

            ecb.SetComponent(bullet, new LocalTransform
            {
                Position = request.ValueRO.Position,
                Rotation = quaternion.identity,
                Scale = 1f
            });
            ecb.SetComponent(bullet, new Bullet
            {
                Speed = request.ValueRO.Speed,
                Damage = request.ValueRO.Damage,
            });
            ecb.SetComponent(bullet, new Lifetime
            {
                Value = request.ValueRO.Lifetime,
            });
            ecb.SetComponent(bullet, new Direction
            {
                Value = request.ValueRO.Direction,
            });
            ecb.SetComponent(bullet, new Faction
            {
                Value = request.ValueRO.Faction,
            });

            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}
