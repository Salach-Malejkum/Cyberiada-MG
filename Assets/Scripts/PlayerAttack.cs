using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStats stats;
    [SerializeField] private LayerMask attackableLayer;
    private RaycastHit[] hits;
    [Header("Melee Attack Range")]
    [SerializeField] private float meleeAttackRadius;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private LayerMask enemyLayer;
    private float attackTimeCounter;
    private float ComboEndCounter;
    private int meleeComboAttackNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.stats = GetComponent<PlayerStats>();
        attackTimeCounter = stats.TimeBtwAttacks;
    }

    private void Update()
    {
        attackTimeCounter += Time.deltaTime;
        ComboEndCounter += Time.deltaTime;
    }

    public void OnMeleeAttack(InputAction.CallbackContext inputAction)
    {
        
        if (inputAction.started && attackTimeCounter >= stats.TimeBtwAttacks)
        {

            if (ComboEndCounter > stats.TimeBtwCombos)
            {
                meleeComboAttackNumber = 1;
            }

            switch (meleeComboAttackNumber)
            {
                case 1:
                    //run animation for attack 1
                    Debug.Log(meleeComboAttackNumber);
                    meleeComboAttackNumber ++;
                    break;
                case 2:
                    //run animation for attack 2
                    Debug.Log(meleeComboAttackNumber);
                    meleeComboAttackNumber = 1;
                    break;
            }
            attackTimeCounter = 0;
            ComboEndCounter = 0;
            DealMeleeDamage(); // tymczasowo zanim nie bêdzie animacji, wywo³ywane jako animation event
        }
    }

    private void DealMeleeDamage()// animation event function
    {
        hits = Physics.SphereCastAll(attackTransform.position, meleeAttackRadius, transform.right * transform.localScale.x, 0f, enemyLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            EnemyStats enemyStats = hits[i].collider.gameObject.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                if (true)//warunek czy atak w têpie
                {
                    enemyStats.RemoveHealthOnAttack(stats.UnitAttackDamage+ stats.UnitAttackBuff, this.gameObject);
                }
                else
                {
                    enemyStats.RemoveHealthOnAttack(stats.UnitAttackDamage, this.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackTransform.position, meleeAttackRadius);
    }
}
