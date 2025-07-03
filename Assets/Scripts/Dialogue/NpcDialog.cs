using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class NpcDialog : MonoBehaviour
{
    [HideInInspector] public DialogueSO conversation;

    [SerializeField] private DialogueSO[] conversations;

    private Transform player;
    private SpriteRenderer speechBubbleRenderer;

    private DialogueManager dialogueManager;
    private EventManager eventManager;

    private bool dialogueInitiated;

    [SerializeField] private SpriteRenderer npcRenderer;

    private void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        speechBubbleRenderer = GetComponent<SpriteRenderer>();
        speechBubbleRenderer.enabled = false;

        for (int i = 0; i < conversations.Length; i++)
        {
            if (GameManager.instance.dialogues.Contains(conversations[i]))
            {
                conversations[i] = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Transform>();
            Vector3 direction = (player.position - transform.position).normalized;
            if ((direction.x > 0 && !npcRenderer.flipX) || (direction.x < 0 && npcRenderer.flipX))
            {
                Flip();
            }

            if (!dialogueInitiated)
            {
                speechBubbleRenderer.enabled = true;

                SelectConversation();
                dialogueManager.InitiateDialogue(this);
                dialogueInitiated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speechBubbleRenderer.enabled = false;
            dialogueManager.TurnOffDialogue();
            dialogueInitiated = false;
            //RemoveConversationsHeld();
        }
    }

    private void Flip()
    {
        npcRenderer.flipX = !npcRenderer.flipX;
    }

    private void SelectConversation()
    {
        for (int i = 0; i < conversations.Length; i++)
        {
            if (conversations[i] != null)
            {
                if (eventManager.RequiredEventsOccured(conversations[i].eventsRequired))
                {
                    conversation = conversations[i];
                }
                else if (conversations[i].eventsRequired[0] == DialogueEvents.NoEvent && !conversations[i].isRepeatable)
                {
                    conversation = conversations[i];
                }
            }
        }
        if (conversation == null)
        {
            for (int i = 0; i < conversations.Length; i++)
            {
                if (conversations[i] != null)
                {
                    if (conversations[i].isRepeatable)
                    {
                        conversation = conversations[i];
                    }
                }
            }
        }
    }

    public void RemoveConversationsHeld()
    {
        for(int i = 0; i < conversations.Length; i++)
        {
            if (conversations[i] != null /*&& conversations[i].wasHeld*/)
                if (!conversations[i].isRepeatable && conversations[i] == conversation)
                {
                    conversations[i] = null;
                    GameManager.instance.AddFinishedDialogue(conversation);
                }
        }
        conversation = null;
        dialogueInitiated = false;
    }
}
