using UnityEngine;

public class BossStats : EnemyStats
{
    [SerializeField] private BossStateMachine BossStateMachine;
    [SerializeField] private HealthBarControl bossHealthBar;
    protected override void DamageProcess()
    {
        bossHealthBar.SetSliderValue(health, maxHealth);
    }
    protected override void DeathProcess()
    {
        BossStateMachine.ChangeState(BossStateMachine.BossState.Death);
    }
}
