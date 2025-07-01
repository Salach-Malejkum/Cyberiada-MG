using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStats stats;
    private RaycastHit[] hits;
    [Header("Melee Attack Range")]
    [SerializeField] private float meleeAttackRadius;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackErrorMargin = 0.1f;
    [SerializeField] SpriteRenderer rythmDebug;
    private float attackTimeCounter;
    private float comboEndCounter;
    private int meleeComboAttackNumber;
    private bool isOnBeat = false;
    private float beatTime;
    private float attackTime;
    private PlayerMove playerMove;
    private Animator anim;

    void Start()
    {
        this.stats = GetComponent<PlayerStats>();
        attackTimeCounter = stats.TimeBtwAttacks;
        MusicManager.Instance.Subscribe(CheckBeatChange);
        playerMove = GetComponent<PlayerMove>();
        anim = this.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        attackTimeCounter += Time.deltaTime;
        comboEndCounter += Time.deltaTime;

        if (Mathf.Abs(Time.time - beatTime) <= attackErrorMargin)
        {
            rythmDebug.color = Color.blue;
        }
        else
        {
            rythmDebug.color = Color.red;
        }
    }

    public void OnMeleeAttack(InputAction.CallbackContext inputAction)
    {
        if (playerMove.GetCanAttack() && inputAction.started && attackTimeCounter >= stats.TimeBtwAttacks && playerMove.isGrounded)
        {
            playerMove.isAttacking = true;
            anim.SetTrigger("AttackTrigger");
            attackTime = Time.time;
            if (comboEndCounter > stats.TimeBtwCombos)
            {
                meleeComboAttackNumber = 1;
            }

            switch (meleeComboAttackNumber)
            {
                case 1:
                    anim.SetInteger("AttackNum", meleeComboAttackNumber);
                    meleeComboAttackNumber++;
                    break;
                case 2:
                    anim.SetInteger("AttackNum", meleeComboAttackNumber);
                    meleeComboAttackNumber = 1;
                    break;
            }
            attackTimeCounter = 0;
            comboEndCounter = 0;
        }
    }

    private void CheckBeatChange(int beatNum, float beatTime)
    {
        this.beatTime = beatTime;
        OnBeat();
    }

    private void OnBeat()
    {
        if (Mathf.Abs(beatTime - attackTime) <= attackErrorMargin)
        {
            isOnBeat = true;
        }
        else
        {
            isOnBeat = false;
        }
    }

    private void DealMeleeDamage()
    {
        hits = Physics.SphereCastAll(attackTransform.position, meleeAttackRadius, transform.right, 0f, enemyLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            EnemyStats enemyStats = hits[i].collider.gameObject.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                if (isOnBeat)
                {
                    enemyStats.RemoveHealthOnAttack(stats.UnitAttackDamage + stats.UnitAttackBuff, this.gameObject);
                }
                else
                {
                    enemyStats.RemoveHealthOnAttack(stats.UnitAttackDamage, this.gameObject);
                }
            }

            DestroyHandler destroyHandler = hits[i].collider.gameObject.GetComponent<DestroyHandler>();
            if (destroyHandler != null)
                destroyHandler.OnHit();
        }
        playerMove.isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attackTransform.position, meleeAttackRadius);
    }
}
