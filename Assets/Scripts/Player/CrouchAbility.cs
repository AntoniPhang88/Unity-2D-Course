using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchAbility : BaseAbility
{
    public InputActionReference crouchActionRef;
    [SerializeField] private float crouchSpeed;
    private string crouchParameterName = "Crouch";
    private int crouchParameterID;

    private string xSpeedParameterName = "xSpeed";
    private int xSpeedParameterID;
    private bool wantToStop;

    protected override void Initialization()
    {
        base.Initialization();
        crouchParameterID = Animator.StringToHash(crouchParameterName);
        xSpeedParameterID = Animator.StringToHash(xSpeedParameterName);
    }

    private void OnEnable()
    {
        crouchActionRef.action.performed += TryToCrouch;
        crouchActionRef.action.canceled += StopCrouch;
    }
    private void OnDisable()
    {
        crouchActionRef.action.performed -= TryToCrouch;
        crouchActionRef.action.canceled -= StopCrouch;
    }

    public override void EnterAbility()
    {
        linkedPhysics.CrounchColliders();
    }

    public override void ExitAbility()
    {
        wantToStop = false;
        linkedPhysics.StandColliders();
    }

    private void TryToCrouch(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if (linkedPhysics.grounded == false ||
            linkedStateMachine.currentState == PlayerStates.State.Dash ||
            linkedStateMachine.currentState == PlayerStates.State.Ladders)
            return;
        wantToStop = false;
        linkedStateMachine.ChangeState(PlayerStates.State.Crouch);
    }
    private void StopCrouch(InputAction.CallbackContext value)
    {
        if (!isPermitted)
            return;
        if (linkedStateMachine.currentState != PlayerStates.State.Crouch)
            return;

        if(linkedPhysics.ceilingDetected)
        {
            wantToStop=true;
            return;
        }

        if (linkedInput.horizontalInput == 0)
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        else if (linkedInput.horizontalInput != 0)
            linkedStateMachine.ChangeState(PlayerStates.State.Run);
    }

    public override void ProcessAbility()
    {
        player.Flip();
        if(wantToStop && linkedPhysics.ceilingDetected == false)
        {
            if (linkedInput.horizontalInput == 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            else if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
        }
        if (linkedPhysics.grounded == false)
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
    }

    public override void ProcessFixedAbility()
    {
        if (linkedPhysics.grounded)
            linkedPhysics.rb.linearVelocity = new Vector2(linkedInput.horizontalInput * crouchSpeed, linkedPhysics.rb.linearVelocityY);
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(crouchParameterID, linkedStateMachine.currentState == PlayerStates.State.Crouch);
        linkedAnimator.SetFloat(xSpeedParameterID, Mathf.Abs(linkedPhysics.rb.linearVelocityX));
    }

}
