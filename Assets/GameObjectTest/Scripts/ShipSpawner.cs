using Unity.VisualScripting;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject ShipPrefab;
    public GameObject BulletPrefab;
    public GameObject MissileProjectilePrefab;
    public TargetingSystem TargetingSystem;
    public int ShipCount;

    private void Start()
    {
        SpawnFleet(Affiliation.Player, Vector3.zero, MissileProjectilePrefab);
        SpawnFleet(Affiliation.Enemy, new Vector3(0, 0, ShipCount * 5), BulletPrefab);
    }

    private void SpawnFleet(Affiliation affiliation, Vector3 startPosition, GameObject projectile)
    {
        for (int i = 0; i < ShipCount; i++)
        {
            GameObject shipGO = Instantiate(
                ShipPrefab,
                startPosition + new Vector3(i * 5, 0, 0),
                Quaternion.identity
            );

            if (!shipGO.TryGetComponent<Ship>(out var ship))
                continue;

            ship.Affiliation = affiliation;
            ship.MaxHealth = 100;
            ship.MaxSpeed = 10;
            ship.Cooldown = 2;
            ship.MaxAmmo = 100;
            ship.ProjectilePrefab = projectile;

            TargetingSystem.Ships.Add(shipGO);
        }
    }
}
