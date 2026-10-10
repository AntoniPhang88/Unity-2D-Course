using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public InputActionReference interactActionReference;
    private IInteractable currentInteractable;
    public static bool isInteracting = false;

    private void OnEnable()
    {
        interactActionReference.action.performed += TryToInteract;
    }
    private void OnDisable()
    {
        interactActionReference.action.performed -= TryToInteract;
    }
    private void TryToInteract(InputAction.CallbackContext value)
    {
        if(currentInteractable != null && isInteracting == false)
        {
            currentInteractable.CustomInteract();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = null;
        }
    }
}
