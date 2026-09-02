using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReloadAbility : BaseAbility
{
    public InputActionReference reloadActionRef;
    [SerializeField] private ReloadBar reloadBar;
    private Weapon currentWeapon;
    private Coroutine reloadCoroutine;

    protected override void Initialization()
    {
        base.Initialization();
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
    }

    public override void EnterAbility()
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
        linkedPhysics.ResetVelocity();
    }

    private void OnEnable()
    {
        reloadActionRef.action.performed += TryToReload;
    }
    private void OnDisable()
    {
        reloadActionRef.action.performed -= TryToReload;
    }

    private void TryToReload(InputAction.CallbackContext value)
    {
        if(!isPermitted || currentWeapon == null)
            return;
        if (linkedPhysics.grounded == false || linkedStateMachine.currentState == PlayerStates.State.Ladders
            || linkedStateMachine.currentState == PlayerStates.State.Dash
            || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if(currentWeapon.ReloadCheck() == false || currentWeapon.isReloading)
            return;

        reloadCoroutine = StartCoroutine(ReloadProcess());
    }

    private IEnumerator ReloadProcess()
    {
        linkedStateMachine.ChangeState(PlayerStates.State.Reload);
        currentWeapon.isReloading = true;
        reloadBar.ActivateReloadBar();

        float elapsedTime = 0;
        while(elapsedTime < currentWeapon.reloadTime)
        {
            elapsedTime += Time.deltaTime;
            reloadBar.UpdateReloadBar(elapsedTime, currentWeapon.reloadTime);
            yield return null;
        }
        reloadBar.DeactivateReloadBar();

        currentWeapon.Reload();
        Shooting.OnUpdateAmmo?.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
        if (linkedStateMachine.currentState != PlayerStates.State.Death && linkedStateMachine.currentState != PlayerStates.State.KnockBack)
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
    }

    public override void ExitAbility()
    {
        reloadBar.DeactivateReloadBar();
        if (reloadCoroutine != null)
            StopCoroutine(reloadCoroutine);
        currentWeapon.isReloading = false;
    }
    public override void UpdateAnimator()
    {
        //reload animation
    }
}
