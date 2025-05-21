using UnityEngine;

public class PlatformTriggerZone : MonoBehaviour
{
    public MeshCollider platformCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            {
                Physics.IgnoreCollision(other, platformCollider, true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            Physics.IgnoreCollision(other, platformCollider, false);
        }
    }

    private bool IsPlayer(GameObject gameObject)
    {
        return gameObject.CompareTag("Player");
    }
}
