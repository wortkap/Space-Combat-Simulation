using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    public float Speed;
    public float Damage;
    public float Lifetime;
    public int ProjectileType; // 0=bullet, 1=missile
    public GameObject Prefab;
}