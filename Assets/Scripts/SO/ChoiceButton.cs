using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ChoiceButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Button cButton;
    [SerializeField] private TextMeshProUGUI label;

    private int index;
    private DialogueManager dialogueManager;

    public void Initialize(DialogueManager manager, int newIndex, string buttonText)
    {
        dialogueManager = manager;
        index = newIndex;
        label.text = buttonText;
        cButton.onClick.RemoveAllListeners();
        cButton.onClick.AddListener(OnClicked);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Button Selected");
    }

    private void OnClicked()
    {
        Debug.Log("OnClicked");
    }
}
