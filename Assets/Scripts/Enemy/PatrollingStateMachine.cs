using UnityEngine;

public class PatrollingStateMachine : EnemySimpleStateMachine
{
    [SerializeField] private PatrollPhysics patrollPhysics;

    [Header("IDLE STATE")]
    [SerializeField] private string idleAnimationName;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float idleStateTimer;

    [Header("MOVE STATE")]
    [SerializeField] private string moveAnimationName;
    [SerializeField] private float speed;
    [SerializeField] private float minMoveTime;
    [SerializeField] private float maxMoveTime;
    [SerializeField] private float minimumTurnDelay;
    private float moveStateTimer;
    private float turnCooldown;

    #region IDLE
    public override void EnterIdle()
    {
        anim.Play(idleAnimationName);
        idleStateTimer = Random.Range(minIdleTime, maxIdleTime);
        patrollPhysics.NegateForces();
    }
    public override void UpdateIdle()
    {
        idleStateTimer -= Time.deltaTime;
        if(idleStateTimer <= 0)
        {
            ChangeState(EnemyState.Move);
        }
    }
    public override void ExitIdle()
    {
        //do something
    }
    #endregion

    #region Move
    public override void EnterMove()
    {
        anim.Play(moveAnimationName);
        moveStateTimer = Random.Range(minIdleTime, maxIdleTime);
    }
    public override void UpdateMove()
    {
        moveStateTimer -= Time.deltaTime;
        if(moveStateTimer <= 0)
            ChangeState(EnemyState.Idle);

        if(turnCooldown > 0)
            turnCooldown -= Time.deltaTime;

        if(patrollPhysics.wallDetected || patrollPhysics.groundDetected == false)
        {
            if (turnCooldown > 0)
                return;
            ForceFlip();
            speed *= -1;
            turnCooldown = minimumTurnDelay;
        }
    }
    public override void FixUpdateMove()
    {
        patrollPhysics.rb.linearVelocity = new Vector2(speed, patrollPhysics.rb.linearVelocityY);
    }
    #endregion
}
