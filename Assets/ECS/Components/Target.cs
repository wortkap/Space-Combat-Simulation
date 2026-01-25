using Unity.Entities;
using Unity.Mathematics;

public struct Target : IComponentData
{
    public Entity Entity;
    public float3 Location;
}