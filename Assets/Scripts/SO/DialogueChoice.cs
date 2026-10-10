using UnityEngine;
[CreateAssetMenu(menuName ="Dialogue/Choice")]
public class DialogueChoice : ScriptableObject
{
    [TextArea(1, 2)]
    public string choiceText;

    public DialogueObject nextDialogueObject;

    public DialogueChoiceAction dialogueAction = DialogueChoiceAction.None;
}

public enum DialogueChoiceAction
{
    None,
    StartQuest,
    CompleteQuest,
    OpenShop,
    GiveItem,
    Custom
}
