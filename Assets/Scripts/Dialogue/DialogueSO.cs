using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public DialogueActors[] actors;

    [Header("Dialogue")]
    [TextArea]
    public string[] dialogue;

    [Tooltip("The words that will appear on option buttons")]
    public string[] optionText;

    public DialogueSO option0;
    public DialogueSO option1;
    public DialogueSO option2;
    public DialogueSO option3;
    public DialogueSO option4;
}
