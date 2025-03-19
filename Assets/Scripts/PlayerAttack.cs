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
    private bool isOnBeat = false;
    private float beatTime;
    private float attackTime;
    [SerializeField] private float attackErrorMargin = 0.1f;

    void Start()
    {
        this.stats = GetComponent<PlayerStats>();
        attackTimeCounter = stats.TimeBtwAttacks;
        MusicManager.Instance.Subscribe(CheckBeatChange);
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

            attackTime = Time.time;
            if (ComboEndCounter > stats.TimeBtwCombos)
            {
                meleeComboAttackNumber = 1;
            }

            switch (meleeComboAttackNumber)
            {
                case 1:
                    //run animation for attack 1
                    meleeComboAttackNumber++;
                    break;
                case 2:
                    //run animation for attack 2
                    meleeComboAttackNumber = 1;
                    break;
            }
            attackTimeCounter = 0;
            ComboEndCounter = 0;
            DealMeleeDamage(); // tymczasowo zanim nie bêdzie animacji, wywo³ywane jako animation event
        }
    }

    private void CheckBeatChange(int beatNum, float beatTime)
    {
        //Debug.Log(beatTime + ", " + beatNum);
        this.beatTime = beatTime;
        OnBeat();
    }

    private void OnBeat()
    {
        if (Mathf.Abs(beatTime - attackTime) < attackErrorMargin)
        {
            isOnBeat = true;
        }
        else
        {
            isOnBeat = false;
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
                if (isOnBeat)//warunek czy atak w têpie
                {
                    enemyStats.RemoveHealthOnAttack(stats.UnitAttackDamage + stats.UnitAttackBuff, this.gameObject);
                    Debug.Log("onBeat");
                }
                else
                {
                    enemyStats.RemoveHealthOnAttack(stats.UnitAttackDamage, this.gameObject);
                    Debug.Log("not on beat");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackTransform.position, meleeAttackRadius);
    }
}
