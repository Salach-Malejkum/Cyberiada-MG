using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class UnitStats : MonoBehaviour
{
    [SerializeField] protected float unitMaxHealth = 10f;
    public float UnitMaxHealth
    {
        get { return this.unitMaxHealth; }
    }
    [SerializeField] protected float unitCurrentHealth = 0f;
    public float UnitCurrentHealth
    {
        get { return this.unitCurrentHealth; }
    }
    [SerializeField] protected float unitAttackDamage = 0f;
    public float UnitAttackDamage
    {
        get { return this.unitAttackDamage; }
    }
    [SerializeField] protected Vector3 unitRespawnCoordinates;
    public Vector3 UnitRespownCoordinates
    {
        get { return this.unitRespawnCoordinates; }
    }

    public event Action onUnitDeath;

    public GameObject lastAggressor;

    public event Action<float, float> OnUnitHealthUpdate;
    public event Action<float, float> OnUnitMaxHealthUpdate;
    public event Action<float> OnAttackUpdate;
    public event Action<float> OnMovementSpeedUpdate;
    public event Action<float> OnAttackSpeedUpdate;

    public virtual void RemoveHealthOnAttack(float hpAmount, GameObject aggressor)
    {
        if (this.gameObject == null) { return; }
        if (aggressor.CompareTag("Player"))
        {
            this.lastAggressor = aggressor;
        }
        this.OnHealthChanged(UnitCurrentHealth, UnitCurrentHealth - hpAmount);
    }

    public virtual void HealthRestored(float hpAmount)
    {
        if (UnitCurrentHealth + hpAmount > unitMaxHealth)
        {
            OnHealthChanged(UnitCurrentHealth, unitMaxHealth);
        }
        else
        {
            OnHealthChanged(UnitCurrentHealth, UnitCurrentHealth + hpAmount);
        }
    }

    private void OnHealthChanged(float oldHP, float newHP)
    {
        this.unitCurrentHealth = newHP;
        this.OnUnitHealthUpdate?.Invoke(newHP, unitMaxHealth);
        this.OnDeathCheck();
    }

    protected void OnDeathCheck()
    {
        if (this.gameObject == null) { return; }

        if (this.unitCurrentHealth <= 0)
        {
            onUnitDeath?.Invoke();
        }
    }

    private void OnMaxHealthChanged(float oldMaxHP, float newMaxHP)
    {
        this.OnUnitMaxHealthUpdate?.Invoke(this.unitCurrentHealth, newMaxHP);
    }

    private void OnAttackChanged(float oldAttack, float newAttack)
    {
        this.OnAttackUpdate?.Invoke(newAttack);
    }

    private void OnMovementSpeedChanged(float oldMS, float newMS)
    {
        this.OnMovementSpeedUpdate?.Invoke(newMS);
    }
}
