using System;
using Unity.Entities;
using UnityEngine;

public class ShipEditorRuntime : MonoBehaviour
{
    public ShipDataBase shipDb;
    public HullDefinition[] hulls; // list of hull definitions in project
    public ModuleLibrary moduleLibrary;

    public ShipInstanceData CreateDefaultShip(string id, string displayName, HullDefinition hull)
    {
        var s = new ShipInstanceData
        {
            Id = id,
            Name = displayName,
            HullId = hull.HullId,
            ThrusterModuleId = "",
            WeaponModuleIds = new string[hull.WeaponSlotCount],
            UtilityModuleIds = new string[hull.UtilitySlotCount],
            ArmorFrontCount = 0,
            ArmorBackCount = 0,
            ArmorBodyCount = 0,
            WeaponLocalPositions = new Vector3[hull.WeaponSlotCount],
            WeaponLocalEuler = new Vector3[hull.WeaponSlotCount],
        };

        return s;
    }

    public void SaveShip(ShipInstanceData data)
    {
        shipDb.AddOrReplace(data);
        Debug.Log($"Saved ship {data.Id}");
    }

    public void DeleteShip(string id)
    {
        shipDb.Remove(id);
    }

    // Simple spawn single ship button for testing
    public void SpawnShipNow(string shipId)
    {
        var s = shipDb.Get(shipId);
        if (s == null)
        {
            Debug.LogError($"No ship with id {shipId}");
            return;
        }
        var hull = Array.Find(hulls, h => h.HullId == s.HullId);
        if (hull == null)
        {
            Debug.LogError($"Hull not found: {s.HullId}");
            return;
        }

        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        var prefab = ShipFactory.BuildShipPrefab(s, hull, moduleLibrary, em);
        var instance = ShipFactory.InstantiateShip(prefab, em, Vector3.zero);
        Debug.Log($"Spawned ship instance {instance.Index}");
    }
}