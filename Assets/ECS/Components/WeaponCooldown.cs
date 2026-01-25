using Unity.Entities;

public struct WeaponCooldown : IComponentData
{
    public float Max;
    public float Current;
}