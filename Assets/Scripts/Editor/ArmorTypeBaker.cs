using Unity.Entities;

public class ArmorTypeBaker : Baker<ArmorTypeAuthoring>
{
    public override void Bake(ArmorTypeAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new ArmorType
        {
            Id = authoring.ArmorId,
            MassPerUnit = authoring.MassPerUnit,
            HealthPerUnit = authoring.HealthPerUnit,
        });
    }
}