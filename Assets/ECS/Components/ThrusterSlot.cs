using Unity.Entities;
using Unity.Mathematics;

public struct ThrusterSlot : IComponentData
{
    public Entity ModulePrefab;
    public float3 LocalPosition;
    public quaternion LocalRotation;
}
