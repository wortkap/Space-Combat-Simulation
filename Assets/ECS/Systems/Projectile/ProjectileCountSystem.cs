using Unity.Burst;
using Unity.Entities;

partial struct ProjectileCountSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        int count = SystemAPI.QueryBuilder().WithAbsent<ShipTag>().Build().CalculateEntityCount();

        UnityEngine.Debug.Log("Projectile count: " + count);
    }
}
