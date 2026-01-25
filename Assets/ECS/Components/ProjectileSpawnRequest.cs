using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileSpawnRequest : IComponentData
{
    public Entity ProjectilePrefab;
    public float3 Position;
    public float3 Direction;
    public float3 Target;
}
