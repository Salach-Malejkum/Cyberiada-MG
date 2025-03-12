using UnityEngine;

[CreateAssetMenu(fileName = "ActorSO", menuName = "Scriptable Objects/ActorSO")]
public class ActorSO : ScriptableObject
{
    public string actorName;
    public Sprite actorPortrait;
}
