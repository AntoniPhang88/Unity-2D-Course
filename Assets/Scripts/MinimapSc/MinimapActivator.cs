using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapActivator : MonoBehaviour
{
    public InputActionReference minimapActivatorRef;
    private Player player;
    private bool mapActivated = false;
    [SerializeField] private CanvasGroup minimapCanvasGroup;
    void Start()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        minimapActivatorRef.action.performed += TryToOpenMinimap;
    }
    private void OnDisable()
    {
        minimapActivatorRef.action.performed -= TryToOpenMinimap;
    } 
    private void TryToOpenMinimap(InputAction.CallbackContext value)
    {
        if(mapActivated)
        {
            //disable map
            mapActivated = !mapActivated;
            minimapCanvasGroup.alpha = 0;
            player.gatherInput.DisableMinimap();
            if(player.playerStats.GetCurrentHealth() > 0)
                player.gatherInput.EnablePlayerMap();
        }
        else
        {
            //enable map
            if (player.playerStats.GetCurrentHealth() <= 0)
                return;
            mapActivated = !mapActivated;
            minimapCanvasGroup.alpha = 1;
            player.gatherInput.DisablePlayerMap();
            player.gatherInput.EnableMinimap();
        }
    }
}
