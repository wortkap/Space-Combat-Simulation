using Unity.Burst;
using Unity.Entities;

partial struct ProjectileCountSystem : ISystem
{
    //public void OnUpdate(ref SystemState state)
    //{
    //    int count = SystemAPI.QueryBuilder().WithAll<ProjectileTag>().Build().CalculateEntityCount();
    //    if (count > 0 )
    //        UnityEngine.Debug.Log("Projectile count: " + count);
    //}
}
