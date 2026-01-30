using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct ModuleSlot : IBufferElementData
{
    public Entity ModulePrefab;
    public float3 LocalPosition;
    public quaternion LocalRotation;
    public FixedString64Bytes ModuleId;
}
