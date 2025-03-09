using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class DialogueManager : MonoBehaviour
{
    private DialogueSO currentConversation;
    private NpcDialog currentNpcDialog;
    private int stepNum = 0;
    private bool dialogueActivated;

    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private Image portrait;
    private TMP_Text dialogueText;

    private string currentSpeaker;
    private Sprite currentPortrait;

    [SerializeField] private ActorSO[] actorSO;

    [SerializeField] private GameObject[] optionButton;
    private TMP_Text[] optionButtonText;
    private GameObject optionsPanel;

    private EventManager eventManager;

    private void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        optionsPanel = GameObject.Find("OptionPanel");
        optionsPanel.SetActive(false);

        optionButtonText = new TMP_Text[optionButton.Length];
        for (int i = 0; i < optionButton.Length; i++)
        {
            optionButtonText[i] = optionButton[i].GetComponentInChildren<TMP_Text>();
        }

        for (int i = 0; i < optionButton.Length; i++)
        {
            optionButton[i].SetActive(false);
        }

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);
    }

    public void InitiateDialogue(NpcDialog npcDialogue)
    {
        currentNpcDialog = npcDialogue;
        currentConversation = npcDialogue.conversation;
        CheckForEvents();
        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;
        dialogueActivated = false;
        dialogueCanvas.SetActive(false);
        optionsPanel.SetActive(false);
    }

    void OnTalk(InputValue inputValue)
    {
        if (dialogueActivated)
        {
            if (stepNum >= currentConversation.actors.Length)
            {
                currentNpcDialog.RemoveConversationsHeld();
                //currentConversation.wasHeld = true;
                TurnOffDialogue();
            } 
            else
            {

                PlayDialogue();
            }
        }
    }

    private void PlayDialogue()
    {
        for (int i = 0; i < actorSO.Length; i++)
        {
            if (actorSO[i].name == currentConversation.actors[stepNum].ToString())
            {
                currentSpeaker = actorSO[i].actorName;
                currentPortrait = actorSO[i].actorPortrait;
            }
        }


        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;
        
        if (currentConversation.actors[stepNum] == DialogueActors.Branch)
        {
            for (int i = 0; i < currentConversation.optionText.Length; i++)
            {
                if (currentConversation.optionText[i] == null)
                {
                    optionButton[i].SetActive(false);
                }
                else
                {
                    optionButtonText[i].text = currentConversation.optionText[i];
                    optionButton[i].SetActive(true);
                }

                optionButton[0].GetComponent<Button>().Select();
            }
        }
        
        if (stepNum < currentConversation.dialogue.Length)
        {
            dialogueText.text = currentConversation.dialogue[stepNum];
        }
        else
        {
            optionsPanel.SetActive(true);
        }
        dialogueCanvas.SetActive(true);
        stepNum += 1;
    }

    public void Option(int optionNum)
    {
        //Debug.Log("OptionSelected");
        foreach (GameObject button in optionButton)
        {
            button.SetActive(false);
        }

        //currentConversation.wasHeld = true;
        currentNpcDialog.RemoveConversationsHeld();

        if (optionNum == 0)
        {
            currentConversation = currentConversation.option0;
        }
        if (optionNum == 1)
        {
            currentConversation = currentConversation.option1;
        }
        if (optionNum == 2)
        {
            currentConversation = currentConversation.option2;
        }
        if (optionNum == 3)
        {
            currentConversation = currentConversation.option3;
        }
        if (optionNum == 4)
        {
            currentConversation = currentConversation.option4;
        }
        CheckForEvents();
        stepNum = 0;
    }

    private void CheckForEvents()
    {
        foreach (DialogueEvents e in (DialogueEvents[])Enum.GetValues(typeof(DialogueEvents)))
        {
            for (int i = 0; i < currentConversation.eventsStarted.Length; i++)
            {
                if (currentConversation.eventsStarted[i] == e)
                {
                    eventManager.EventOccured(e);
                }
            }
        }
    }
}

public enum DialogueActors
{
    Branch,
    Player,
    testNPC,
    testNPC2,
    NPC4,
    NPC5,
    NPC6,
    NPC7,
    NPC8,
    NPC9,
    NPC10,
    NPC11,
    NPC12,
    NPC13,
    NPC14,
    NPC15,
    NPC16,
    NPC17,
    NPC18,
    NPC19,
    NPC20,
    NPC21,
    NPC22,
    NPC23,
};