using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct BulletGridSyncSystem : ISystem
{
    private const float CellSize = 5f;
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, gridCell) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<GridCell>>())
        {
            float3 pos = transform.ValueRO.Position;

            gridCell.ValueRW.Value = new int2(
                (int)math.floor(pos.x / CellSize),
                (int)math.floor(pos.z / CellSize)
            );
        }
    }
}
