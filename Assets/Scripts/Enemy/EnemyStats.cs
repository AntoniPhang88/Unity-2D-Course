using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] protected EnemySimpleStateMachine enemyStateMachine;
    [SerializeField] protected float health;

    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range(0, 1)] private float flashStrength;
    [SerializeField] private Color flashCol;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    [SerializeField] private SpriteRenderer spriter;

    protected Coroutine damageCoroutine;
    private void Start()
    {
        defaultMaterial = spriter.material;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        DamageProcess();
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(Flash());
        if(health <= 0)
        {
            DeathProcess();
        }
    }

    protected virtual void DamageProcess()
    {
        //custimize in a child class
    }
    protected virtual void DeathProcess()
    {
        //customize in a child class
    }
    private IEnumerator Flash()
    {
        spriter.material = flashMaterial;
        flashMaterial.SetColor("_FlashColor", flashCol);
        flashMaterial.SetFloat("_FalshAmount", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
        damageCoroutine = null;
    }
}
