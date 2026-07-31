using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateCheckpoint : MonoBehaviour
{
    public InputActionReference ActiveCheck;
    [HideInInspector]
    public Checkpoint checkPoint;

    private void OnEnable()
    {
        ActiveCheck.action.performed += TryToActive;
    }
    private void OnDisable()
    {
        ActiveCheck.action.performed -= TryToActive;
    }
    private void TryToActive(InputAction.CallbackContext value)
    {
        if (checkPoint == null)
            return;
        //active
        checkPoint.Activate();
    }
}
