using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IPlayerInAttackRange
{
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float meleeAttackRadius = 0.1f;
    [SerializeField] private float meleeAttackDamageRange;
    [SerializeField] private int[] meleeComboAttackMap = new int[8];
    [SerializeField] private LayerMask playerLayer;
    private bool enemyReadyToAttack = false;
    private int attackMapIndex;
    private int attackNumber;
    private EnemyStats stats;
    private Animator anim;

    void Start()
    {
        attackNumber = 0;
        attackMapIndex = 0;
        anim = this.GetComponent<Animator>();
        MusicManager.Instance.Subscribe(AttackToBeat);
        stats = this.GetComponent<EnemyStats>();
    }

    public void EnemyReadyToAttack()
    {
        enemyReadyToAttack = true;
    }

    public void EnemyFinishedAttack()
    {
        enemyReadyToAttack = false;
    }

    public bool EnemyAttacking()
    {
        return enemyReadyToAttack;
    }

    public bool PlayerInAttackRange()
    {
        RaycastHit[] hits = Physics.SphereCastAll(attackTransform.position, meleeAttackRadius, transform.right * transform.localScale.x, 0f, playerLayer);

        if (hits.Length > 0)
        {
            return true;
        }
        return false;
    }

    private void AttackToBeat(int beatNum, float beatTime)
    {
        if (enemyReadyToAttack)
        {
            if (meleeComboAttackMap[attackMapIndex] == 1)
            {
                //start animation
                DealMeleeDamage(); // temp until animations ready then called by animation events
                attackMapIndex++;
                attackNumber++;
            }
            else
            {
                attackMapIndex++;
            }
        } else if (attackMapIndex != 0)
        {
            attackMapIndex = 0;
            attackNumber = 0;
        }

        if (attackMapIndex >= meleeComboAttackMap.Length)
        {
            attackMapIndex = 0;
        }

        if (attackNumber >= 3)
        {
            attackNumber = 0;
        }
    }

    private void DealMeleeDamage()
    {
        RaycastHit[] hits = Physics.SphereCastAll(attackTransform.position, meleeAttackRadius, transform.right, 0f, playerLayer);
        if (hits.Length > 0)
        {
            PlayerStats playerStats = hits[0].collider.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.RemoveHealthOnAttack(stats.UnitAttackDamage, this.gameObject);
            }
        }
        EnemyFinishedAttack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attackTransform.position, meleeAttackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, meleeAttackDamageRange);
    }
}
