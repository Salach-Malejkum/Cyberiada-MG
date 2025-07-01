using UnityEngine;

public class ObjectStats : UnitStats
{
    private void Awake()
    {
        this.onUnitDeath += HandleObjectDestruction;
    }

    private void Start()
    {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.unitRespawnCoordinates = transform.position;
    }

    private void OnDestroy()
    {
        this.onUnitDeath -= HandleObjectDestruction;
    }

    public override void RemoveHealthOnAttack(float damageAmount, GameObject aggressor)
    {
        base.RemoveHealthOnAttack(damageAmount, aggressor);
    }

    protected virtual void HandleObjectDestruction()
    {
        if (this.lastAggressor != null && this.lastAggressor.CompareTag("Player"))
        {
            PlayerStats stats = this.lastAggressor.GetComponent<PlayerStats>();

        }
        Destroy(this.gameObject);
    }
}
