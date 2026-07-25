using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private HealthBarControl healthBarControl;
    [SerializeField] private float maxHealth;
    private float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
    }

    public void DamagePlayer(float damage)
    {
        currentHealth -= damage;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
        if(currentHealth <= 0)
        {
            if(player.stateMachine.currentState != PlayerStates.State.KnockBack)
                Debug.Log("Player is Dead");
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
