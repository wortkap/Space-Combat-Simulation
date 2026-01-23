using Unity.Entities;

public struct ShipHealth : IComponentData
{
    public float Value;
}

public struct ShipHealthDirty : IComponentData { }