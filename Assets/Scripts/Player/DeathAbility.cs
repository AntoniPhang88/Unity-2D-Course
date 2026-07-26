using UnityEngine;

public class DeathAbility : BaseAbility
{
    public override void EnterAbility()
    {
        player.gatherInput.DisablePlayerMap();
        linkedPhysics.ResetVelocity();
        if(linkedPhysics.grounded)
            linkedAnimator.SetBool("Death", true);
        else
        {
            //if have other death animation
            linkedAnimator.SetBool("Death", true);
        }
    }
    //public override void UpdateAnimator()
    //{
    //    linkedAnimator.SetBool("Death", linkedStateMachine.currentState == PlayerStates.State.Death);
    //}

    public void ResetGame()
    {
        LevelManager.instance.RestartLevel();
    }
}
