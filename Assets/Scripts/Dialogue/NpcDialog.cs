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

    private void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        speechBubbleRenderer = GetComponent<SpriteRenderer>();
        speechBubbleRenderer.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueInitiated)
        {
            speechBubbleRenderer.enabled = true;

            player = other.gameObject.GetComponent<Transform>();

            if(player.position.x > transform.position.x && transform.parent.localScale.x < 0)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && transform.parent.localScale.x > 0)
            {
                Flip();
            }
            SelectConversation();
            dialogueManager.InitiateDialogue(this);
            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speechBubbleRenderer.enabled = false;
            dialogueManager.TurnOffDialogue();
            dialogueInitiated = false;
            RemoveConversationsHeld();
        }
    }

    private void Flip()
    {
        Vector3 currentScale = transform.parent.localScale;
        currentScale.x *= -1;
        transform.parent.localScale = currentScale;
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
            if (conversations[i] != null)
                if (!conversations[i].isRepeatable && conversations[i] == conversation)
                {
                    conversations[i] = null;
                }
        }
        conversation = null;
    }
}
