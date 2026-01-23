using System.Collections.Generic;
using Unity.Entities;

public static class ShipBridgeRegistry
{
    public static readonly Dictionary<Entity, ShipECSBridge> Bridges = new();

    public static void Register(Entity entity, ShipECSBridge bridge)
    {
        if (entity == Entity.Null) return;
        Bridges[entity] = bridge;
    }

    public static void Unregister(Entity entity)
    {
        if (entity == Entity.Null) return;
        Bridges.Remove(entity);
    }
}