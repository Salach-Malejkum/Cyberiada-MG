using UnityEngine;

public class OneWayDoor : MonoBehaviour
{
    [SerializeField] private MeshCollider oneWayCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oneWayCollider.enabled = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oneWayCollider.enabled = true;
        }
    }
}
