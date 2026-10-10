using UnityEngine;

public class FrameRate : MonoBehaviour
{
    [SerializeField] private int FPS;

    void Start()
    {
        Application.targetFrameRate = FPS;
    }
}
