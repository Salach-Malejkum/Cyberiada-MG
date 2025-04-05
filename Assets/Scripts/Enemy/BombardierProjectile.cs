using UnityEngine;

public class BombardierProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    private GameObject shooter;
    private float damageAmount;
    private float shootingDirection;
    private Rigidbody projectileRB;

    private void Start()
    {
        Destroy(this.gameObject, lifetime);
        projectileRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        projectileRB.linearVelocity = new Vector3(speed * shootingDirection, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.RemoveHealthOnAttack(damageAmount, this.gameObject);
            }
        }

        Destroy(this.gameObject);
    }

    public void SetShooter(float damageAmount, float shootingDirection)
    {
        this.shootingDirection = shootingDirection;
        this.damageAmount = damageAmount;
    }
}
