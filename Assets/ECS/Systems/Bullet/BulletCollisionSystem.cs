using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct BulletCollisionSystem : ISystem
{
    private const float CellSize = 5f;
    private const float CollisionRadius = 0.5f;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        // Build a map of cells -> ship entities
        NativeParallelMultiHashMap<int, Entity> cellMap =
            new NativeParallelMultiHashMap<int, Entity>(1024, Allocator.Temp);

        foreach (var (shipCell, shipHealth, shipEntity) in
                 SystemAPI.Query<RefRO<GridCell>, RefRO<ShipHealth>>().WithEntityAccess())
        {
            int key = HashCell(shipCell.ValueRO.Value);
            cellMap.Add(key, shipEntity);
        }

        foreach (var (bulletTransform, bullet, bulletFaction, bulletCell, bulletEntity) in
                 SystemAPI.Query<
                     RefRO<LocalTransform>,
                     RefRO<Bullet>,
                     RefRO<Faction>,
                     RefRO<GridCell>>().WithEntityAccess())
        {
            int2 bCell = bulletCell.ValueRO.Value;

            // Check 9 cells around bullet
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int2 neighbor = new int2(bCell.x + dx, bCell.y + dy);
                    int key = HashCell(neighbor);

                    if (cellMap.TryGetFirstValue(key, out Entity shipEntity, out var iterator))
                    {
                        do
                        {
                            var shipPos = state.EntityManager.GetComponentData<ShipPosition>(shipEntity);
                            var shipFaction = state.EntityManager.GetComponentData<ShipFaction>(shipEntity);
                            var shipHealth = state.EntityManager.GetComponentData<ShipHealth>(shipEntity);

                            if (shipFaction.Value == bulletFaction.ValueRO.Value)
                                continue;

                            float distance = math.distance(shipPos.Value, bulletTransform.ValueRO.Position);

                            if (distance < CollisionRadius)
                            {
                                shipHealth.Value -= bullet.ValueRO.Damage;
                                state.EntityManager.SetComponentData(shipEntity, shipHealth);

                                if (!state.EntityManager.HasComponent<ShipHealthDirty>(shipEntity))
                                    ecb.AddComponent<ShipHealthDirty>(shipEntity);

                                ecb.DestroyEntity(bulletEntity);
                                break;
                            }
                        }
                        while (cellMap.TryGetNextValue(out shipEntity, ref iterator));
                    }
                }
            }
        }

        ecb.Playback(state.EntityManager);
        cellMap.Dispose();
    }

    private static int HashCell(int2 cell)
    {
        return cell.x * 73856093 ^ cell.y * 19349663;
    }
}