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
    [SerializeField] protected float timeToRespowen = 0.5f;
    public float TimeToRespowen
    {
        get { return this.timeToRespowen; }
    }

    private void Awake()
    {
        this.onUnitDeath += HandlePlayerDeath;
    }

    private void Start()
    {
        this.unitCurrentHealth = this.unitMaxHealth;
        this.unitRespownCcoordinates = transform.position;
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
        unitRespownCcoordinates = newCoordinates;
    }

    public void HandlePlayerDeath()
    {
        StartCoroutine(Respown());
    }

    private IEnumerator Respown()
    {
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        //fade in
        yield return new WaitForSeconds(timeToRespowen);
        //fade out
        transform.position = unitRespownCcoordinates;
        HealthRestored(this.unitMaxHealth);
        renderer.enabled = true;
    }
}
