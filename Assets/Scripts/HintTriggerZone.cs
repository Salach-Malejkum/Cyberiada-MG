using UnityEngine;
using System;

public class HintTriggerZone : MonoBehaviour
{
    public static event Action<GameObject, string> OnMessageTriggered;
    [SerializeField] private string messageIdentifier = "HintTriggerZone";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnMessageTriggered?.Invoke(other.gameObject, messageIdentifier);
        }
    }
}
