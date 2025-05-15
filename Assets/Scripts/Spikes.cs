using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.gameObject.GetComponent<PlayerStats>();
            stats.handleSpikes(this.gameObject);
        }
    }
}
