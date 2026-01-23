using Unity.Entities;
using Unity.Mathematics;

public struct BulletSpawnRequest : IComponentData
{
    public float3 Position;
    public float3 Direction;
    public float Speed;
    public float Damage;
    public int Faction;
    public float Lifetime;
}
