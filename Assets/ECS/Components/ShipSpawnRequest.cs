using Unity.Entities;
using Unity.Mathematics;

public struct ShipSpawnRequest : IComponentData
{
    public int Count;
    public Entity ShipPrefab;
    public float3 StartPosition;
    public int Faction;
}