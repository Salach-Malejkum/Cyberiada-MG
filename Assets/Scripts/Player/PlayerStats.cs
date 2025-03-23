using System;
using System.Collections;
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
    [SerializeField] protected float timeToRespawn = 0.5f;
    public float TimeToRespawn
    {
        get { return this.timeToRespawn; }
    }

    [SerializeField] protected Vector3 fallCheckPoint;
    public Vector3 FallCheckPoint
    {
        get { return this.fallCheckPoint; }
    }

    [SerializeField] protected float pitfallDamage;
    public float PitfallDamage
    {
        get { return this.pitfallDamage; }
    }

    private void Awake()
    {
        this.onUnitDeath += HandlePlayerDeath;
    }

    private void Start()
    {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.unitRespownCoordinates = transform.position;
    }

    private void OnDestroy()
    {
        this.onUnitDeath -= HandlePlayerDeath;
    }

    public override void RemoveHealthOnAttack(float damageAmount, GameObject aggressor)
    {
        base.RemoveHealthOnAttack(damageAmount, aggressor);
    }

    public void UpdateRespownCoordinates(Vector3 newCoordinates)
    {
        unitRespownCoordinates = newCoordinates;
    }

    public void UpdateFallCheckPointCoordinates(Vector3 newCoordinates)
    {
        fallCheckPoint = newCoordinates;
    }

    public void HandlePlayerDeath()
    {
        StartCoroutine(Respawn(true));
    }

    private IEnumerator Respawn(bool isPlayerDead)
    {
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        //fade in
        yield return new WaitForSeconds(timeToRespawn);
        //fade out
        if (isPlayerDead)
        {
            transform.position = unitRespownCoordinates;
            HealthRestored(this.unitMaxHealth);
        }
        else
        {
            transform.position = fallCheckPoint;
        }
        renderer.enabled = true;
    }

    public void handlePlayerFall(GameObject obj)
    {
        if (UnitCurrentHealth > pitfallDamage)
        {
            StartCoroutine(Respawn(false));
        }
        RemoveHealthOnAttack(pitfallDamage, obj);
    }
}
