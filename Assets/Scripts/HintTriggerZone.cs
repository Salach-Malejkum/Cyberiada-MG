using UnityEngine;
using System;

public class HintTriggerZone : MonoBehaviour
{
    public static event Action<string> OnMessageTriggered;
    [SerializeField] private string messageIdentifier = "HintTriggerZone";

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger Entered");
        if (other.CompareTag("Player"))
        {
            OnMessageTriggered?.Invoke(messageIdentifier);
        }
    }
}
