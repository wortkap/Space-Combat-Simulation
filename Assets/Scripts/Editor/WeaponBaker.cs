using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class WeaponBaker : Baker<WeaponAuthoring>
{
    public override void Bake(WeaponAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new ModuleId { Id = authoring.ModuleId });

        Entity projectileEntity = Entity.Null;
        if (authoring.ProjectilePrefab != null)
            projectileEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.None);

        AddComponent(entity, new WeaponModule
        {
            Cooldown = authoring.FireCooldown,
            Range = authoring.Range,
            ProjectilePrefab = projectileEntity,
        });

        AddComponent(entity, LocalTransform.FromPositionRotation(authoring.LocalPosition, quaternion.EulerXYZ(math.radians(authoring.LocalEuler))));
    }
}