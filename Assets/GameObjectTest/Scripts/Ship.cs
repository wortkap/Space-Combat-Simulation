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
        CurrentSpeed = 1;
        CooldownTimer = Cooldown;
        CurrentAmmo = MaxAmmo;
    }

    private void Update()
    {
        if (Target != null)
        {
            Vector3 targetDirection = (Target.transform.position - transform.position).normalized;

            CooldownTimer -= Time.deltaTime;
            if (CooldownTimer < 0)
            {
                CooldownTimer = Cooldown;
                Fire(targetDirection);
            }

            Move(targetDirection);
        }
    }

    private void Fire(Vector3 targetDirection)
    {
        GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Projectile _projectile = projectile.GetComponent<Projectile>();
        _projectile.Affiliation = Affiliation;
        _projectile.direction = targetDirection;
        _projectile.Target = Target;
        CurrentAmmo--;
    }

    private void Move(Vector3 targetDirection)
    {
        transform.position += CurrentSpeed * Time.deltaTime * targetDirection;
    }

    public void ReceiveDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}