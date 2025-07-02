using System.Collections;
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
    [SerializeField] private float rangedAttackDelayMultiplier = 30f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    private float attackTimeCounter;
    private float rangedAttackTimeCounter;
    private float comboEndCounter;
    private int meleeComboAttackNumber;
    private bool isOnBeat = false;
    private float beatTime;
    private float attackTime;
    private PlayerMove playerMove;
    private Animator anim;

    [Header("Material Renderer")]
    [SerializeField] private Renderer mat_renderer;
    [SerializeField] private Color onBeatColor;
    [SerializeField] private Color offBeatColor;

    void Start()
    {
        this.stats = GetComponent<PlayerStats>();
        attackTimeCounter = stats.TimeBtwAttacks;
        rangedAttackTimeCounter = stats.TimeBtwAttacks * rangedAttackDelayMultiplier;
        MusicManager.Instance.Subscribe(CheckBeatChange);
        playerMove = GetComponent<PlayerMove>();
        anim = this.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        attackTimeCounter += Mathf.Clamp(attackTimeCounter + Time.deltaTime, 0f, stats.TimeBtwAttacks + 1f);
        rangedAttackTimeCounter += Mathf.Clamp(attackTimeCounter + Time.deltaTime, 0f, stats.TimeBtwAttacks * rangedAttackDelayMultiplier + 1f);
        comboEndCounter += Time.deltaTime;

        if (Mathf.Abs(Time.time - beatTime) <= attackErrorMargin)
        {
            mat_renderer.material.SetColor("_OutlineColor", onBeatColor);
        }
        else
        {
            mat_renderer.material.SetColor("_OutlineColor", offBeatColor);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext inputAction)
    {
        if (playerMove.canRangeAttack && inputAction.started && rangedAttackTimeCounter >= stats.TimeBtwAttacks * rangedAttackDelayMultiplier)
        {
            playerMove.isAttacking = true;
            anim.SetTrigger("RangedAttack");
            attackTime = Time.time;

            rangedAttackTimeCounter = 0;
        }
    }

    private void ShootRangedProjectile()
    {
        playerMove.isAttacking = false;
        GameObject projectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        PlayerProjectile projectileScript = projectileInstance.GetComponent<PlayerProjectile>();
        if (projectileScript != null)
        {
            projectileScript.SetShooter(stats.UnitAttackDamage, firePoint.localPosition.x);
        }
    }

    public void OnMeleeAttack(InputAction.CallbackContext inputAction)
    {
        if (playerMove.GetCanAttack() && inputAction.started && attackTimeCounter >= stats.TimeBtwAttacks)
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
            EnemyPatrol enemyPatrol = hits[i].collider.gameObject.GetComponent<EnemyPatrol>();
            EnemyStats enemyStats = hits[i].collider.gameObject.GetComponent<EnemyStats>();
            BossBehaviour bossBehaviour = hits[i].collider.gameObject.GetComponent<BossBehaviour>();
            DestructibleObject destructibleObject = hits[i].collider.gameObject.GetComponent<DestructibleObject>();

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
                if (isOnBeat)
                {
                    enemyStats.RemoveHealthOnAttack(stats.UnitAttackDamage + stats.UnitAttackBuff, this.gameObject);
                }
                else
                {
                    enemyStats.RemoveHealthOnAttack(stats.UnitAttackDamage, this.gameObject);
                }
            }
        }
        playerMove.isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attackTransform.position, meleeAttackRadius);
    }
}
