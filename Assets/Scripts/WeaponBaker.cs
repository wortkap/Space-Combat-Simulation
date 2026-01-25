using Unity.Entities;

public class WeaponBaker : Baker<WeaponAuthoring>
{
    public override void Bake(WeaponAuthoring authoring)
    {
        Entity weapon = GetEntity(TransformUsageFlags.None);

        AddComponent(weapon, new WeaponCooldown
        {
            Max = authoring.FireCooldown,
            Current = authoring.FireCooldown
        });

        AddComponent(weapon, new Ammo
        {
            Current = authoring.Ammo,
            Max = authoring.Ammo
        });

        //AddComponent(weapon, new Range
        //{
        //    Value = authoring.Range
        //});

        AddComponent(weapon, new ProjectilePrefab
        {
            Value = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic)
        });
    }
}