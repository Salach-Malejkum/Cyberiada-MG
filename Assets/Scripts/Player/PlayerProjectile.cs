using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private LayerMask enemyLayer;
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
        print(other.gameObject.layer);
        print(LayerMask.NameToLayer("Enemy"));
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
            EnemyPatrol enemyPatrol = other.gameObject.GetComponent<EnemyPatrol>();
            BossBehaviour bossBehaviour = other.gameObject.GetComponent<BossBehaviour>();
            DestructibleObject destructibleObject = other.gameObject.GetComponent<DestructibleObject>();
            print(other.name);
            if (enemyPatrol != null)
            {
                enemyPatrol.onHitChangeColor();
            }
            else if (bossBehaviour != null)
            {
                bossBehaviour.onHitChangeColor();
            }

            if (destructibleObject != null)
            {
                destructibleObject.TakeDamage();
            }

            if (enemyStats != null)
            {
                enemyStats.RemoveHealthOnAttack(this.damageAmount, this.gameObject);
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
