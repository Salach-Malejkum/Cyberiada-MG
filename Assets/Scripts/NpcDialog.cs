using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class NpcDialog : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text NpcName;
    [SerializeField] private string[] dialogue;
    [SerializeField] private float writingSpeed = 0.01f;
    private int dialogueIndex;

    private bool playerInRange;
    private bool isTalking = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ResetDialogueBox();
        }
    }

    void OnTalk(InputValue inputValue)
    {
        if (playerInRange)
        {
           if (isTalking)
            {
                NextLine();
            }
            else
            {
                NpcName.text = this.name.ToString();
                dialogueBox.SetActive(true);
                isTalking = true;
                StartCoroutine(Typing());
            }
        }
    }

    void ResetDialogueBox()
    {
        dialogueText.text = "";
        NpcName.text = "";
        dialogueIndex = 0;
        isTalking = false;
        dialogueBox.SetActive(false);
    }

    void NextLine()
    {
        if (dialogueIndex < dialogue.Length - 1)
        {
            dialogueIndex++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ResetDialogueBox();
        }
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[dialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(writingSpeed);
        }
    }
}
