using Unity.Collections;
using Unity.Entities;

public struct ModuleId : IComponentData
{
    public FixedString64Bytes Id;
}
