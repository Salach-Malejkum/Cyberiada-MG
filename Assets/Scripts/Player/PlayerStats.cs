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

    [Header("Respawns")]
    [SerializeField] protected float pitFallTimeToRespawn = 0.5f;
    public float PitFallTimeToRespawn
    {
        get { return this.pitFallTimeToRespawn; }
    }
    [SerializeField] protected float spikesTimeToRespawn = 0.1f;
    public float SpikesTimeToRespawn
    {
        get { return this.spikesTimeToRespawn; }
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
        this.unitRespawnCoordinates = transform.position;
    }

    private void OnDestroy()
    {
        this.onUnitDeath -= HandlePlayerDeath;
    }

    public override void RemoveHealthOnAttack(float damageAmount, GameObject aggressor)
    {
        base.RemoveHealthOnAttack(damageAmount, aggressor);
    }

    public void UpdateRespawnCoordinates(Vector3 newCoordinates)
    {
        unitRespawnCoordinates = newCoordinates;
    }

    public void UpdateFallCheckPointCoordinates(Vector3 newCoordinates)
    {
        fallCheckPoint = newCoordinates;
    }

    public void HandlePlayerDeath()
    {
        StartCoroutine(Respawn(true, spikesTimeToRespawn));
    }

    private IEnumerator Respawn(bool isPlayerDead, float timeToRespawn)
    {
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        //fade in
        yield return new WaitForSeconds(timeToRespawn);
        //fade out
        if (isPlayerDead)
        {
            transform.position = unitRespawnCoordinates;
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
            StartCoroutine(Respawn(false, pitFallTimeToRespawn));
        }
        RemoveHealthOnAttack(pitfallDamage, obj);
    }

    public void handleSpikes(GameObject obj)
    {
        if (UnitCurrentHealth > pitfallDamage)
        {
            StartCoroutine(Respawn(false, spikesTimeToRespawn));
        }
        RemoveHealthOnAttack(pitfallDamage, obj);
    }
}
