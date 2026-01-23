using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Affiliation Affiliation;
    public float damage;
    public float speed;
    public Vector3 direction;
    public GameObject Target;

    protected virtual void Start()
    {
        transform.LookAt(Target.transform.position);
    }

    protected virtual void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        UpdateDirection();
        Move();
        UpdateRotation();
    }

    protected virtual void UpdateDirection()
    {
        // Default behavior: do nothing
        // Bullets keep their initial direction
    }

    protected void Move() // virtual when missile becomes more realistic
    {
        transform.position += speed * Time.deltaTime * direction;
    }
    protected virtual void UpdateRotation()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Ship>(out var ship))
            return;

        if (ship.Affiliation != Affiliation)
        {
            ship.ReceiveDamage(damage);
            Destroy(gameObject);
        }
    }
}
