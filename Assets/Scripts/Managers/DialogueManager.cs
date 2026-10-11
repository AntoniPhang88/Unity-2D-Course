using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dialogueManagerInstance;
    private GatherInput gatherInput;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Choices UI")]
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject choicesButtonPrefab;

    private List<ChoiceButton> activeChoiceButtons = new List<ChoiceButton>();
    private DialogueChoice[] activeChoices;
    private bool choicesVisible;
    private int selectedChoiceIndex;

    private DialogueObject currentDialogue;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;
    public void RegisterGatherInput(GatherInput gatherInputInstance)
    {
        gatherInput = gatherInputInstance;
    }
    private void Awake()
    {
        if (dialogueManagerInstance == null)
            dialogueManagerInstance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void ShowChoices()
    {
        activeChoices = currentDialogue.choices;
        if(activeChoices.Length == 0)
        {
            EndDialogue();
            return;
        }
        gatherInput.EnableUIMap();
        //destroy old button, create new choice buttons, select the first one, update UI
        ClearActiveChoiceButtons();
        choicesPanel.SetActive(true);
        choicesVisible = true;

        for(int i = 0; i < activeChoices.Length; i++)
        {
            var go = Instantiate(choicesButtonPrefab, choicesPanel.transform);
            var cb = go.GetComponent<ChoiceButton>();

            cb.Initialize(this, i, activeChoices[i].choiceText);
            activeChoiceButtons.Add(cb);
        }

        selectedChoiceIndex = 0;
        StartCoroutine(SelectChoiceButtonNextFrame(activeChoiceButtons[selectedChoiceIndex]));
    }
    public void SelectChoice(int index)
    {
        if (!choicesVisible)
            return;
        var choice = activeChoices[index];
        switch (choice.dialogueAction)
        { 
            case DialogueChoiceAction.None:
                break;
            case DialogueChoiceAction.StartQuest:
                Debug.Log("StartQuest");
                break;
            case DialogueChoiceAction.CompleteQuest:
                Debug.Log("CompleteQuest");
                break;
            case DialogueChoiceAction.OpenShop:
                Debug.Log("OpenShop");
                break;
            case DialogueChoiceAction.GiveItem:
                Debug.Log("GiveItem");
                break;
            case DialogueChoiceAction.Custom:
                Debug.Log("Custom");
                break;
            default:
                break;
        }
        gatherInput.DisableUIMap();
        HideChoices();

        if(choice.nextDialogueObject != null)
        {
            StartDialogue(choice.nextDialogueObject);
        }
        else
        {
            EndDialogue();
        }
    }
    public void SetChoiceIndex(int i)
    {
        selectedChoiceIndex = i;
    }
    private void HideChoices()
    {
        ClearActiveChoiceButtons();
        if (choicesPanel != null)
            choicesPanel.SetActive(false);
        choicesVisible = false;
    }
    private IEnumerator SelectChoiceButtonNextFrame(ChoiceButton firstButton)
    {
        yield return null;

        if (firstButton != null)
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        //if (firstButton == null || !firstButton.gameObject.activeInHierarchy)
        //    yield break;
        //
        //var button = firstButton.GetComponent<UnityEngine.UI.Button>();
        //if (button != null && button.IsInteractable())
        //    button.Select();
    }
    private void ClearActiveChoiceButtons()
    {
        foreach(var cb in activeChoiceButtons)
        {
            if(cb != null)
                Destroy(cb.gameObject);
        }
        activeChoiceButtons.Clear();
    }
    public void StartDialogue(DialogueObject dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialogueUI.SetActive(true);
        gatherInput.DialogueActive();
        Interact.isInteracting = true;
        ShowLine();
    }
    private void ShowLine()
    {
        DialogueLine line = currentDialogue.lines[currentLineIndex];
        speakerNameText.text = line.speakerName;
        speakerImage.sprite = line.speakerIcon;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(line.dialogueText));
    }
    private void  NextLine()
    {
        currentLineIndex++;
        if(currentLineIndex >= currentDialogue.lines.Length)
        {
            if(currentDialogue.choices != null && currentDialogue.choices.Length > 0)
            {
                ShowChoices();
            }
            else
            {
                EndDialogue();
            }
        }
        else
        {
            ShowLine();
        }
    }
    public void ContinueDialogue()
    {
        if(choicesVisible)
        {
            SelectChoice(selectedChoiceIndex);
            return;
        }

        if(isTyping)
        {
            FinishTyping();
        }
        else
        {
            NextLine();
        }
    }
    private  void FinishTyping()
    {
        StopCoroutine(typingCoroutine);
        dialogueText.text = currentDialogue.lines[currentLineIndex].dialogueText;
        isTyping = false;

        if (currentLineIndex == currentDialogue.lines.Length-1 && currentDialogue.choices != null && currentDialogue.choices.Length > 0)
        {
            ShowChoices();
        }
    }
    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        currentDialogue = null;
        gatherInput.DialogueNotActive();
        Interact.isInteracting = false;
    }
    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach(char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

        if (currentLineIndex == currentDialogue.lines.Length - 1 && currentDialogue.choices != null && currentDialogue.choices.Length > 0)
        {
            ShowChoices();
        }
    }
}
