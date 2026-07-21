using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    public InputActionReference moveActionRef;
    public InputActionReference verticalActionRef;

    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        playerMap.Disable();
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

        playerMap.Enable();
        //playerInput.actions.Enable();
        //jumpActionRef.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = moveActionRef.action.ReadValue<float>();
        verticalInput = verticalActionRef.action.ReadValue<float>();
        Debug.Log("Horizontal Input Value : " + horizontalInput);
    }
}
