using UnityEngine;

public class EnemyStats : UnitStats
{
    private void Awake()
    {
        this.onUnitDeath += HandleEnemyDeath;
    }

    private void Start()
    {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.unitRespownCcoordinates = transform.position;
    }

    private void OnDestroy()
    {
        this.onUnitDeath -= HandleEnemyDeath;
    }

    public override void RemoveHealthOnAttack(float damageAmount, GameObject aggressor)
    {
        base.RemoveHealthOnAttack(damageAmount, aggressor);
    }

    protected virtual void HandleEnemyDeath()
    {
        if (this.lastAggressor != null && this.lastAggressor.CompareTag("Player"))
        {
            PlayerStats stats = this.lastAggressor.GetComponent<PlayerStats>();
        
        }
        Destroy(this.gameObject);
    }
}
