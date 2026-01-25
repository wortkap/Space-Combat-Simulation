using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ProjectileCollisionSystem : ISystem
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
                 SystemAPI.Query<RefRO<GridCell>, RefRO<Health>>().WithAll<ShipTag>().WithEntityAccess())
        {
            int key = HashCell(shipCell.ValueRO.Value);
            cellMap.Add(key, shipEntity);
        }

        foreach (var(projectileTransform, projectileDamage, projectileFaction, projectielCell, projectileEntity) in
                 SystemAPI.Query<
                     RefRO<LocalTransform>,
                     RefRO<Damage>,
                     RefRO<Faction>,
                     RefRO<GridCell>>().WithEntityAccess())
        {
            int2 bCell = projectielCell.ValueRO.Value;

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
                            var shipPos = state.EntityManager.GetComponentData<LocalTransform>(shipEntity).Position;
                            var shipFaction = state.EntityManager.GetComponentData<Faction>(shipEntity);
                            var shipHealth = state.EntityManager.GetComponentData<Health>(shipEntity);

                            if (shipFaction.Value == projectileFaction.ValueRO.Value)
                                continue;

                            float distance = math.distance(shipPos, projectileTransform.ValueRO.Position);

                            if (distance < CollisionRadius)
                            {
                                shipHealth.Current -= projectileDamage.ValueRO.Value;
                                state.EntityManager.SetComponentData(shipEntity, shipHealth);

                                // maybe later if needed
                                //if (!state.EntityManager.HasComponent<ShipHealthDirty>(shipEntity))
                                //    ecb.AddComponent<ShipHealthDirty>(shipEntity);

                                ecb.DestroyEntity(projectileEntity);
                                break;
                            }
                        }
                        while (cellMap.TryGetNextValue(out shipEntity, ref iterator));
                    }
                }
            }
        }
    }
    private static int HashCell(int2 cell)
    {
        return cell.x * 73856093 ^ cell.y * 19349663;
    }
}
