using UnityEngine;
using System.Collections.Generic;

public class ProjectileTracker : MonoBehaviour
{
    public static ProjectileTracker Instance;

    private Dictionary<ProjectileType, int> counts = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (ProjectileType type in System.Enum.GetValues(typeof(ProjectileType)))
            counts[type] = 0;
    }

    public void Register(ProjectileType type)
    {
        counts[type]++;
    }

    public void Unregister(ProjectileType type)
    {
        counts[type]--;
    }

    public int GetCount(ProjectileType type)
    {
        return counts[type];
    }

    public int GetTotal()
    {
        int total = 0;
        foreach (var c in counts.Values)
            total += c;
        return total;
    }

    private void Update()
    {
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log(
                $"Projectiles → Total: {GetTotal()} | " +
                $"Bullets: {GetCount(ProjectileType.Bullet)} | " +
                $"Missiles: {GetCount(ProjectileType.Missile)}"
            );
        }
    }
}
