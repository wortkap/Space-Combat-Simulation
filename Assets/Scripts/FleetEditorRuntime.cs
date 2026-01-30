using Unity.Entities;
using UnityEngine;

public class FleetEditorRuntime : MonoBehaviour
{
    public ShipDataBase shipDb;
    public FleetDefinition fleet;
    public ModuleLibrary moduleLibrary;
    public HullDefinition[] hulls;

    public void AddShipToFleet(string shipId, int count, int faction)
    {
        fleet.Entries.Add(new FleetEntry { ShipId = shipId, Count = count, faction = faction });
    }

    public void ClearFleet() => fleet.Entries.Clear();

    public void SpawnFleet()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (var entry in fleet.Entries)
        {
            var shipData = shipDb.Get(entry.ShipId);
            if (shipData == null)
                continue;
            var hull = System.Array.Find(hulls, h => h.HullId == shipData.HullId);
            if (hull == null)
                continue;

            var prefab = ShipFactory.BuildShipPrefab(shipData, hull, moduleLibrary, em);

            for (int i = 0; i < entry.Count; i++)
            {
                var instance = ShipFactory.InstantiateShip(prefab, em, entry.SpawnLocation);
                em.AddComponentData(instance, new Faction { Value = entry.faction });
                Debug.Log($"Spawned ship instance {instance.Index}");
            }
        }
    }
}