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
    [SerializeField] private InputActionAsset playerInput;

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
        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;
        dialogueActivated = false;
        dialogueCanvas.SetActive(false);
        optionsPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //playerInput.SwitchCurrentActionMap("Player");
        playerInput.FindActionMap("Player").Enable();
        playerInput.FindActionMap("UI").Enable();
        playerInput.FindActionMap("Camera").Enable();
        playerInput.FindActionMap("CheckPoint").Enable();
    }

    public void OnTalk(InputAction.CallbackContext inputAction)
    {
        if (inputAction.started)
        {
            if (dialogueActivated)
            {
                ManageDialogue();
            }
        }
    }

    private void ManageDialogue()
    {
        if (stepNum >= currentConversation.actors.Length)
        {
            currentNpcDialog.RemoveConversationsHeld(false);
            CheckForEvents();
            TurnOffDialogue();
        }
        else
        {
            PlayDialogue();
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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
        //playerInput.SwitchCurrentActionMap("Dialogue");
        playerInput.FindActionMap("Player").Disable();
        playerInput.FindActionMap("UI").Disable();
        playerInput.FindActionMap("Camera").Disable();
        playerInput.FindActionMap("CheckPoint").Disable();
        stepNum += 1;
    }

    public void Option(int optionNum)
    {
        foreach (GameObject button in optionButton)
        {
            button.SetActive(false);
        }

        CheckForEvents();

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

        if (currentConversation != null)
        {
            currentNpcDialog.RemoveConversationsHeld(true);
            stepNum = 0;
        }
        else
        {
            currentNpcDialog.RemoveConversationsHeld(false);
            stepNum += 1;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ManageDialogue();
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

    public void OnClickButton0(InputAction.CallbackContext inputAction)
    {
        if (dialogueActivated)
        {
            if (optionButton[0].activeSelf)
            {
                Option(0);
            }
        }
    }

    public void OnClickButton1(InputAction.CallbackContext inputAction)
    {
        if (dialogueActivated)
        {
            if (optionButton[1].activeSelf)
            {
                Option(1);
            }
        }
    }

    public void OnClickButton2(InputAction.CallbackContext inputAction)
    {
        if (dialogueActivated)
        {
            if (optionButton[2].activeSelf)
            {
                Option(2);
            }
        }
    }

    public void OnClickButton3(InputAction.CallbackContext inputAction)
    {
        if (dialogueActivated)
        {
            if (optionButton[3].activeSelf)
            {
                Option(3);
            }
        }
    }
    public void OnClickButton4(InputAction.CallbackContext inputAction)
    {
        if (dialogueActivated)
        {
            if (optionButton[4].activeSelf)
            {
                Option(4);
            }
        }
    }
    
    // Handles up/down navigation on gamepad for dialogue options
    private int selectedOptionIndex = 0;

    public void OnUpDown(InputAction.CallbackContext inputAction)
    {
        if (!dialogueActivated || !optionsPanel.activeSelf)
            return;

        float move = inputAction.ReadValue<float>();
        if (move > 0.5f)
            MoveSelection(-1);
        else if (move < -0.5f)
            MoveSelection(1);
    }

    private void MoveSelection(int direction)
    {
        int optionsCount = 0;
        for (int i = 0; i < optionButton.Length; i++)
            if (optionButton[i].activeSelf) optionsCount++;

        if (optionsCount == 0) return;

        int newIndex = selectedOptionIndex;
        do
        {
            newIndex = (newIndex + direction + optionButton.Length) % optionButton.Length;
        }
        while (!optionButton[newIndex].activeSelf);

        selectedOptionIndex = newIndex;
        optionButton[selectedOptionIndex].GetComponent<Button>().Select();
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
