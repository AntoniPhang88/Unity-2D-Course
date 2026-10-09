using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    private InputActionMap playerMap;
    private InputActionMap uiMap;
    private InputActionMap miniMap;
    private InputActionMap activatorMap;
    private InputActionMap dialogueMap;
    private InputAction dialogueAction;
    private bool dialogueActive;
    private bool waitingForDialogueRelease;
    private int dialogueEndFrame;

    public bool CanJump => playerMap != null && playerMap.enabled &&
        !dialogueActive && !waitingForDialogueRelease;

    public InputActionReference moveActionRef;
    public InputActionReference verticalActionRef;
    public InputActionReference dialogueActionRef;

    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;

    private void OnEnable()
    {
        if (dialogueAction != null)
            dialogueAction.performed += TryToContinueDialogue;
    }

    private void OnDisable()
    {
        if (dialogueAction != null)
            dialogueAction.performed -= TryToContinueDialogue;
        playerMap?.Disable();
        dialogueMap?.Disable();
    }
    private void TryToContinueDialogue(InputAction.CallbackContext value)
    {
        if (dialogueActive && DialogueManager.dialogueManagerInstance != null)
            DialogueManager.dialogueManagerInstance.ContinueDialogue();
    }
    private void TryJump(InputAction.CallbackContext value)
    {
        Debug.Log("Jump"); 
    }

    private void StopJump(InputAction.CallbackContext value)
    {
        Debug.Log("Stop Jump");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Mengikuti action maps name pada control input
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");
        miniMap = playerInput.actions.FindActionMap("MinimapControls");
        activatorMap = playerInput.actions.FindActionMap("Activators");
        dialogueMap = playerInput.actions.FindActionMap("DialogueControl");
        // Resolve from the same input asset whose maps we enable and disable.
        if (dialogueActionRef != null && dialogueActionRef.action != null)
            dialogueAction = dialogueMap?.FindAction(dialogueActionRef.action.id.ToString());
        if (dialogueAction == null)
            dialogueAction = dialogueMap?.FindAction("Skip");
        if (dialogueAction != null)
            dialogueAction.performed += TryToContinueDialogue;

        dialogueMap?.Disable();
        playerMap.Enable();
        activatorMap.Enable();
        if (DialogueManager.dialogueManagerInstance != null)
            DialogueManager.dialogueManagerInstance.RegisterGatherInput(this);
        //playerInput.actions.Enable();
        //jumpActionRef.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // Consume the closing press, even if Jump runs later in the same input update.
        if (waitingForDialogueRelease && Time.frameCount > dialogueEndFrame &&
            !IsDialogueButtonPressed())
            waitingForDialogueRelease = false;

        horizontalInput = moveActionRef.action.ReadValue<float>();
        verticalInput = verticalActionRef.action.ReadValue<float>();
        //Debug.Log("Horizontal Input Value : " + horizontalInput);
    }
    public void EnableMinimap()
    {
        miniMap.Enable();
    }
    public void DisableMinimap()
    {
        miniMap.Disable();
    }
    public void EnablePlayerMap()
    {
        if (!dialogueActive)
            playerMap.Enable();
    }
    public void DisablePlayerMap()
    {
        playerMap.Disable();
    }
    public void DialogueActive()
    {
        dialogueActive = true;
        DisablePlayerMap();
        horizontalInput = 0;
        verticalInput = 0;
        dialogueMap?.Enable();
    }
    public void DialogueNotActive()
    {
        waitingForDialogueRelease = true;
        dialogueEndFrame = Time.frameCount;
        dialogueActive = false;
        dialogueMap?.Disable();
        EnablePlayerMap();
    }

    private bool IsDialogueButtonPressed()
    {
        if (dialogueAction == null)
            return false;

        // The action is disabled after dialogue, so inspect its physical controls.
        foreach (InputControl control in dialogueAction.controls)
        {
            if (control.IsPressed())
                return true;
        }
        return false;
    }
}
