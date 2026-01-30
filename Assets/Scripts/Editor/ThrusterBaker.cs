using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ThrusterBaker : Baker<ThrusterAuthoring>
{
    public override void Bake(ThrusterAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new ModuleId { Id = authoring.ModuleId });

        AddComponent(entity, new ThrusterModule
        {
            MaxSpeed = authoring.MaxSpeed,
            CurrentSpeed = 0f,
            Thrust = authoring.Thrust,
            TurnRate = authoring.TurnRate
        });

        AddComponent(entity, LocalTransform.FromPositionRotation(authoring.LocalPosition, quaternion.EulerXYZ(math.radians(authoring.LocalEuler))));
    }
}
