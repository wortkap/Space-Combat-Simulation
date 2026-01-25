using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    public float Speed;
    public float Damage;
    public float Lifetime;
    public int ProjectileType; // 0=bullet, 1=missile
}

public class ProjectileBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<ProjectileTag>(entity);
        AddComponent(entity, new Damage { Value = authoring.Damage });
        AddComponent(entity, new Lifetime { Value = authoring.Lifetime });
        AddComponent(entity, new MoveSpeed { Max = authoring.Speed });
        AddComponent<LocalTransform>(entity);
    }
}