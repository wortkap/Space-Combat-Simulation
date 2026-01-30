using Unity.Entities;

public struct WeaponModule : IComponentData
{
    public float Cooldown;
    public float Range;
    public Entity ProjectilePrefab;
}
