using UnityEngine;

public class PlayerStats : UnitStats
{
    [SerializeField] protected float unitAttackBuff = 0f;
    public float UnitAttackBuff
    {
        get { return this.unitAttackBuff; }
    }
    [SerializeField] protected float timeBtwAttacks = 0.15f;
    public float TimeBtwAttacks
    {
        get { return this.timeBtwAttacks; }
    }
    [SerializeField] protected float timeBtwCombos = 0.5f;
    public float TimeBtwCombos
    {
        get { return this.timeBtwCombos; }
    }

    private void Awake()
    {
        this.onUnitDeath += HandlePlayerDeath;
    }

    private void Start()
    {
        this.unitCurrentHealth = this.unitMaxHealth;
    }

    private void OnDestroy()
    {
        this.onUnitDeath -= HandlePlayerDeath;
    }

    public override void RemoveHealthOnAttack(float damageAmount, GameObject aggressor)
    {
        base.RemoveHealthOnAttack(damageAmount, aggressor);
    }

    public void HandlePlayerDeath()
    {
        //metoda do respownu
    }
}
