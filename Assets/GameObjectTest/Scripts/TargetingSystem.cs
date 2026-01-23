using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class TargetingSystem : MonoBehaviour
{
    public List<GameObject> Ships = new();

    private void Update()
    {
        for (int i = Ships.Count - 1; i >= 0; i--)
        {
            if (Ships[i] == null)
            {
                Ships.RemoveAt(i);
                continue;
            }

            var sh = Ships[i];

            if (!sh.TryGetComponent<Ship>(out var ship))
                continue;

            if (ship.Target != null)
                continue;

            List<GameObject> availableShips = new();

            foreach (var other in Ships)
            {
                if (other == null || other == sh)
                    continue;

                if (!other.TryGetComponent<Ship>(out var otherShip))
                    continue;

                if (otherShip.Affiliation == ship.Affiliation)
                    continue;

                availableShips.Add(other);
            }

            if (availableShips.Count > 0)
            {
                ship.Target = NearestTarget(ship, availableShips);
            }
        }
    }

    private GameObject NearestTarget(Ship ship, List<GameObject> ships)
    {
        GameObject nearest = ships[0];

        float nearestDistance = Vector3.Distance(nearest.transform.position, ship.transform.position);
        foreach (var sh in ships)
        {
            float distance = Vector3.Distance(sh.transform.position, ship.transform.position);
            if (distance < nearestDistance)
            {
                nearest = sh;
                nearestDistance = distance;
            }
        }

        return nearest;
    }
}