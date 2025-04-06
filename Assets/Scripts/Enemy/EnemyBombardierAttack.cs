using UnityEngine;

public class EnemyBombardierAttack : MonoBehaviour, IPlayerInAttackRange
{
    [SerializeField] private Transform attackTransform;
    [SerializeField] private int[] rangeComboAttackMap = new int[8];
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    private bool enemyReadyToAttack = false;
    private int attackMapIndex;
    private EnemyStats stats;
    private EnemyPatrol patrol;
    
    void Start()
    {
        attackMapIndex = 0;
        MusicManager.Instance.Subscribe(AttackToBeat);
        stats = this.GetComponent<EnemyStats>();
        patrol = this.GetComponent<EnemyPatrol>();
    }

    private void AttackToBeat(int beatNum, float beatTime)
    {
        if (enemyReadyToAttack)
        {
            if (rangeComboAttackMap[attackMapIndex] == 1)
            {
                patrol.anim.SetBool("attack", true);
                attackMapIndex++;
            }
            else
            {
                attackMapIndex++;
            }
        }
        else if (attackMapIndex != 0)
        {
            attackMapIndex = 0;
        }

        if (attackMapIndex >= rangeComboAttackMap.Length)
        {
            attackMapIndex = 0;
        }
    }

    private void CreateProjectile()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        BombardierProjectile projectileScript = projectileInstance.GetComponent<BombardierProjectile>();
        if (projectileScript != null)
        {
            projectileScript.SetShooter(stats.UnitAttackDamage, firePoint.localPosition.x);
        }
        EnemyFinishedAttack();
    }

    public void EnemyReadyToAttack()
    {
        enemyReadyToAttack = true;
    }

    public void EnemyFinishedAttack()
    {
        enemyReadyToAttack = false;
        patrol.anim.SetBool("attack", false);
    }

    public bool EnemyAttacking()
    {
        return enemyReadyToAttack;
    }

    public bool PlayerInAttackRange()
    {
        return patrol.PlayerInSight();
    }
}
