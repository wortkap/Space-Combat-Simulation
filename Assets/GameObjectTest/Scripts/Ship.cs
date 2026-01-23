using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Ship : MonoBehaviour
{
    public Affiliation Affiliation;
    public float MaxHealth;
    public float CurrentHealth;
    public float MaxSpeed;
    public float CurrentSpeed;
    public float CooldownTimer;
    public float Cooldown;
    public float MaxAmmo;
    public float CurrentAmmo;

    public GameObject ProjectilePrefab;
    public GameObject Target;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentSpeed = 5;
        CooldownTimer = Cooldown;
        CurrentAmmo = MaxAmmo;
    }

    private void Update()
    {
        if (Target == null)
        {
            Target = null;
            return;
        }

        Vector3 targetDirection = (Target.transform.position - transform.position).normalized;

        CooldownTimer -= Time.deltaTime;
        if (CooldownTimer < 0)
        {
            CooldownTimer = Cooldown;
            if (TryGetComponent<ShipShooter>(out var shooter))
            {
                shooter.Fire(targetDirection, Affiliation);
            }
        }

        Move(targetDirection);

        if (CurrentHealth <= 0)
        {
            var bridge = GetComponent<ShipECSBridge>();
            if (bridge != null && bridge.ShipEntity != Entity.Null)
            {
                World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(bridge.ShipEntity);
            }

            Destroy(gameObject);
        }
    }

    private void Move(Vector3 targetDirection)
    {
        transform.position += CurrentSpeed * Time.deltaTime * targetDirection;
    }

    public void ReceiveDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    private void OnDestroy()
    {
        TargetingSystem ts = FindAnyObjectByType<TargetingSystem>();
        if (ts != null)
            ts.Ships.Remove(gameObject);
    }
}