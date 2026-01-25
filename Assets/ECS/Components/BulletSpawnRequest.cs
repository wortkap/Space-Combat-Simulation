using Unity.Entities;
using Unity.Mathematics;

public struct BulletSpawnRequest : IComponentData
{
    public float3 Position;
    public int Faction;
    public float3 Velocity;
    public float Damage;
    public float Lifetime;
}
