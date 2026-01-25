using Unity.Entities;
using Unity.Mathematics;

public struct FireRequest : IBufferElementData
{
    public Entity Projectile;
    public float3 FirePosition;
    public float3 FireDirection;
    public Target Target;
}