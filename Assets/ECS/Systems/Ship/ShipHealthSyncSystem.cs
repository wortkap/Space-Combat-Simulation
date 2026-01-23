using Unity.Burst;
using Unity.Entities;

[UpdateAfter(typeof(BulletCollisionSystem))]
partial struct ShipHealthSyncSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (health, entity) in SystemAPI.Query<RefRO<ShipHealth>>()
                 .WithAll<ShipHealthDirty>().WithEntityAccess())
        {
            if (ShipBridgeRegistry.Bridges.TryGetValue(entity, out var bridge))
            {
                bridge.GetComponent<Ship>().CurrentHealth = health.ValueRO.Value;
            }

            ecb.RemoveComponent<ShipHealthDirty>(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}