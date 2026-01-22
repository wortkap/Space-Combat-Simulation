using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Affiliation Affiliation;
    private float damage;
    private float speed;
    public Vector3 direction;
    public GameObject Target;

    private void Start()
    {
        damage = 10f;
        speed = 10f;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Ship>(out var Ship))
            return;

        if (Ship.Affiliation != Affiliation)
        {
            Ship.ReceiveDamage(damage);
            Debug.Log("Projectile has hit an enemy ship");
            Destroy(gameObject);
        }
    }
}
