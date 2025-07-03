using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.gameObject.GetComponent<PlayerStats>();
            stats.handleSpikes(this.gameObject, damage);
        }
    }
}
