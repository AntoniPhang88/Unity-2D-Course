using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShootEffect : MonoBehaviour
{
    public void Play()
    {
        gameObject.SetActive(true);
        Animator effectAnimator = GetComponent<Animator>();
        effectAnimator.Play(0, 0, 0f);
        effectAnimator.Update(0f);
    }

    public void DisableEffect()
    {
        gameObject.SetActive(false);
    }
}
