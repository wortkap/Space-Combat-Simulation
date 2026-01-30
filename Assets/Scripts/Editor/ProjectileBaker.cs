using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ProjectileBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<ProjectileTag>(entity);
        AddComponent(entity, new Damage { Value = authoring.Damage });
        AddComponent(entity, new Lifetime { Value = authoring.Lifetime });
        AddComponent(entity, new MoveSpeed { Max = authoring.Speed });
        AddComponent<LocalTransform>(entity);

        if (authoring.Prefab != null)
        {
            var prefabEntity = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectilePrefab { Value = prefabEntity }) ;
        }
    }
}