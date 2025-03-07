using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class NpcDialog : MonoBehaviour
{
    public DialogueSO[] conversation;

    private Transform player;
    private SpriteRenderer speechBubbleRenderer;

    private DialogueManager dialogueManager;

    private bool dialogueInitiated;

    private void Start()
    {
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
        }
    }

    private void Flip()
    {
        Vector3 currentScale = transform.parent.localScale;
        currentScale.x *= -1;
        transform.parent.localScale = currentScale;
    }
}
