using Unity.Burst;
using Unity.Entities;

partial struct BulletCountSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        int count = SystemAPI.QueryBuilder().WithAll<Bullet>().Build().CalculateEntityCount();

        UnityEngine.Debug.Log("Bullet count: " + count);
    }
}
