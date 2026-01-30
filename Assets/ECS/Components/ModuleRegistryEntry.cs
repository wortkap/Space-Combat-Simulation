using Unity.Entities;
using Unity.Collections;

public struct ModuleRegistryEntry : IBufferElementData
{
    public FixedString64Bytes Id;
    public Entity Prefab;
}