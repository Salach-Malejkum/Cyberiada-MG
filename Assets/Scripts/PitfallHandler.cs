using UnityEngine;

public class PitfallHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.gameObject.GetComponent<PlayerStats>();
            stats.handlePlayerFall(this.gameObject);
        }
    }
}
