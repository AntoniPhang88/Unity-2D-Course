using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : BaseAbility
{
    public InputActionReference dashActionRef;
    [SerializeField] private float dashForce;
    [SerializeField] private float maxDashDuration;
    private float dashTimer;

    private string dashAnimParameterName = "Dash";
    private int dashParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        dashParameterID = Animator.StringToHash(dashAnimParameterName);
    }

    private void OnEnable()
    {
        dashActionRef.action.performed += TryDash;
    }
    private void OnDisable()
    {
        dashActionRef.action.performed -= TryDash;
    }

    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        //optional
        linkedPhysics.ResetVelocity();
    }

    private void TryDash(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        //other conditions
        if (linkedStateMachine.currentState == PlayerStates.State.Dash || linkedPhysics.wallDetected || linkedStateMachine.currentState == PlayerStates.State.Crouch)
            return;

        linkedStateMachine.ChangeState(PlayerStates.State.Dash);
        linkedPhysics.DisableGravity();
        linkedPhysics.ResetVelocity();
        if (player.facingRight)
            linkedPhysics.rb.linearVelocityX = dashForce;
        else
            linkedPhysics.rb.linearVelocityX = -dashForce;

        dashTimer = maxDashDuration;
    }

    public override void ProcessAbility()
    {
        dashTimer -= Time.deltaTime;
        if (linkedPhysics.wallDetected)
            dashTimer = -1;
        if(dashTimer <= 0)
        {
            if (linkedPhysics.grounded)
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(dashParameterID, linkedStateMachine.currentState == PlayerStates.State.Dash);
    }
}
