using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateCheckpoint : MonoBehaviour
{
    public InputActionReference ActivateCheck;
    [HideInInspector]
    public Checkpoint checkPoint;

    private void OnEnable()
    {
        ActivateCheck.action.performed += TryToActive;
    }
    private void OnDisable()
    {
        ActivateCheck.action.performed -= TryToActive;
    }
    private void TryToActive(InputAction.CallbackContext value)
    {
        if (checkPoint == null)
            return;
        //active
        checkPoint.Activate();
    }

}
