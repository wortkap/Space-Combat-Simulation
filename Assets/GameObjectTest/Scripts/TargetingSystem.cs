using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TargetingSystem : MonoBehaviour
{
    public List<GameObject> Ships = new();

    private void Update()
    {
        foreach (var sh in Ships)
        {
            if (sh == null)
            {
                Ships.Remove(sh);
                continue;
            }

            if (!sh.TryGetComponent<Ship>(out var ship))
                continue;

            if (ship.Target != null)
                continue;

            List<GameObject> availableShips = new();

            foreach (var other in Ships)
            {
                if (other == sh)
                    continue;
                if (!other.TryGetComponent<Ship>(out var otherShip))
                    continue;
                if (otherShip.Affiliation == ship.Affiliation)
                    continue;

                availableShips.Add(other);
            }
            if (availableShips.Count > 0)
            {
                ship.Target = availableShips[0];
            }
        }
    }
}
