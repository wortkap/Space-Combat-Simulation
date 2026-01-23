using Unity.Entities;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject ShipPrefab;
    private EntityManager _em;
    public GameObject BulletPrefab;
    public GameObject MissileProjectilePrefab;
    public TargetingSystem TargetingSystem;
    public int ShipCount;

    private void Start()
    {
        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        SpawnFleet(Affiliation.Player, Vector3.zero, BulletPrefab);
        SpawnFleet(Affiliation.Enemy, new Vector3(0, 0, 3 * Mathf.Sqrt(ShipCount) * 5), BulletPrefab);
    }

    private void SpawnFleet(Affiliation affiliation, Vector3 startPosition, GameObject projectile)
    {
        int columnAmount = Mathf.CeilToInt(Mathf.Sqrt(ShipCount));
        int rowAmount = Mathf.CeilToInt((float)ShipCount / (float)columnAmount);
        int shipsSpawned = 0;

        for (int i = 0; i < rowAmount; i++)
        {
            for (int j = 0; j < columnAmount; j++)
            {
                if (shipsSpawned >= ShipCount)
                    break;

                GameObject shipGO = Instantiate(
                    ShipPrefab,
                    startPosition + new Vector3(i * 5, j * 5, 0),
                    Quaternion.identity
                );

                ;

                if (!shipGO.TryGetComponent<Ship>(out var ship))
                    continue;

                ship.Affiliation = affiliation;
                ship.MaxHealth = 100;
                ship.MaxSpeed = 10;
                ship.Cooldown = 0.1f;
                ship.MaxAmmo = 100;
                ship.ProjectilePrefab = projectile;

                Entity shipEntity = _em.CreateEntity(
                    typeof(ShipPosition),
                    typeof(ShipFaction),
                    typeof(ShipHealth),
                    typeof(GridCell)
                );

                _em.SetComponentData(shipEntity, new ShipPosition
                {
                    Value = shipGO.transform.position,
                });
                _em.SetComponentData(shipEntity, new ShipFaction
                {
                    Value = (int)affiliation,
                });
                _em.SetComponentData(shipEntity, new ShipHealth
                {
                    Value = ship.MaxHealth,
                });

                shipGO.GetComponent<ShipECSBridge>().ShipEntity = shipEntity;

                TargetingSystem.Ships.Add(shipGO);

                shipsSpawned++;
            }
        }
    }
}