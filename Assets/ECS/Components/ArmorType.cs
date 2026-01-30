using Unity.Collections;
using Unity.Entities;

public struct ArmorType : IComponentData
{
    public FixedString64Bytes Id;
    public float MassPerUnit;
    public float HealthPerUnit;
}
