using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/Dialogue Object")]
public class DialogueObject : ScriptableObject
{
    public DialogueLine[] lines;

    [Header("Optional choices shown after last line")]
    public DialogueChoice[] choices;
}
