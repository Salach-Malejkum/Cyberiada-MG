using UnityEngine;

public interface IPlayerInAttackRange
{
    public bool PlayerInAttackRange();
    public void EnemyReadyToAttack();
    public void EnemyFinishedAttack();
    public bool EnemyAttacking();
}
