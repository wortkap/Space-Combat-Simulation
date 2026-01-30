using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

public static class ShipFactory
{
    // Build a ship *prefab entity* (with Prefab tag) from data.
    // The function returns the prefab entity.

    public static Entity BuildShipPrefab(ShipInstanceData data, HullDefinition hull, ModuleLibrary library, EntityManager em)
    {
        var ship = em.CreateEntity();

        em.AddComponentData(ship, new Health
        {
            Max = hull.BaseHealth,
            Current = hull.BaseHealth
        });
        em.AddComponentData(ship, new MoveSpeed
        {
            Max = 0f,
            Current = 0f
        });

        em.AddComponentData(ship, new ArmorCounts
        {
            Front = math.clamp(data.ArmorFrontCount, 0, hull.MaxArmorPerSlot),
            Back = math.clamp(data.ArmorBackCount, 0, hull.MaxArmorPerSlot),
            Body = math.clamp(data.ArmorBodyCount, 0, hull.MaxArmorPerSlot)
        });

        // Faction empty by default, to be set by fleet spawner or external system
        // em.AddComponentData(ship, new Faction { value = Null });

        em.AddComponent<IsShipPrefab>(ship);
        em.AddComponent<Prefab>(ship);

        if (!string.IsNullOrEmpty(data.ThrusterModuleId))
        {
            var thrusterPrefab = library.GetModule(data.ThrusterModuleId);
            em.AddComponentData(ship, new ThrusterSlot
            {
                ModulePrefab = thrusterPrefab,
                LocalPosition = data.ThrusterLocalPosition,
                LocalRotation = quaternion.EulerYZX(math.radians(data.ThrusterLocalEuler))
            });
        }

        var weaponBuffer = em.AddBuffer<ModuleSlot>(ship);
        if (data.WeaponModuleIds != null)
        {
            for (int i = 0; i < data.WeaponModuleIds.Length; i++)
            {
                var id = data.WeaponModuleIds[i];
                var prefab = library.GetModule(id);
                float3 pos = float3.zero;
                quaternion rot = quaternion.identity;
                if (data.WeaponLocalPositions != null && i < data.WeaponLocalPositions.Length)
                    pos = data.WeaponLocalPositions[i];
                if (data.WeaponLocalEuler != null && i < data.WeaponLocalEuler.Length)
                    rot = quaternion.EulerXYZ(math.radians(data.WeaponLocalEuler[i]));
                ModuleSlot slot = new ModuleSlot
                {
                    ModulePrefab = prefab,
                    LocalPosition = pos,
                    LocalRotation = rot,
                    ModuleId = id
                };
                weaponBuffer.Add(slot);
            }
        }

        var utilityBuffer = em.AddBuffer<ModuleSlot>(ship);
        if (data.UtilityModuleIds != null)
        {
            foreach (var id in data.UtilityModuleIds)
            {
                var prefab = library.GetModule(id);
                utilityBuffer.Add(new ModuleSlot
                {
                    ModulePrefab = prefab,
                    LocalPosition = Vector3.zero,
                    LocalRotation = Quaternion.identity,
                    ModuleId = id
                });
            }
        }

        // ArmorStats will be computed separately by a system (needs Module armor type data)
        em.AddComponentData(ship, new ArmorStats { TotalMass = 0, AddedHealth = 0f });

        return ship;
    }

    // Instantiate a ship prefab into a scene (will instantiate child module prefabs and parent them)
    // This method expects a ship prefab entity created by BuildShipPrefab.
    public static Entity InstantiateShip(Entity prefab, EntityManager em, Vector3 spawnLocation)
    {
        var instance = em.Instantiate(prefab);

        // --- Thruster ---
        if (em.HasComponent<ThrusterSlot>(instance))
        {
            var ts = em.GetComponentData<ThrusterSlot>(instance);
            if (ts.ModulePrefab != Entity.Null)
            {
                var moduleInstance = em.Instantiate(ts.ModulePrefab);
                em.AddComponentData(moduleInstance, new Parent { Value = instance });
                em.AddComponentData(moduleInstance,
                    LocalTransform.FromPositionRotationScale(
                        ts.LocalPosition,
                        ts.LocalRotation,
                        1f
                    ));
            }
        }

        // --- Weapons / Utility Modules in ModuleSlot Buffer ---
        if (em.HasBuffer<ModuleSlot>(instance))
        {
            var prefabBuffer = em.GetBuffer<ModuleSlot>(instance);

            // Copy before structural changes
            NativeList<ModuleSlot> slotCopy = new NativeList<ModuleSlot>(prefabBuffer.Length, Allocator.Temp);

            for (int i = 0; i < prefabBuffer.Length; i++)
                slotCopy.Add(prefabBuffer[i]);

            // Instantiate modules
            for (int i = 0; i < slotCopy.Length; i++)
            {
                var slot = slotCopy[i];

                if (slot.ModulePrefab != Entity.Null)
                {
                    var moduleInstance = em.Instantiate(slot.ModulePrefab);

                    em.AddComponentData(moduleInstance, new Parent { Value = instance });
                    em.AddComponentData(moduleInstance,
                        LocalTransform.FromPositionRotationScale(
                            slot.LocalPosition,
                            slot.LocalRotation,
                            1f
                        ));
                }
            }

            slotCopy.Dispose();
        }
        return instance;
    }
}