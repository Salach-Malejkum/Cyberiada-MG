using UnityEngine;

public class PlatformTriggerZone : MonoBehaviour
{
    public MeshCollider platformCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            {
                print("inside");
                Physics.IgnoreCollision(other, platformCollider, true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            print("outside");
            Physics.IgnoreCollision(other, platformCollider, false);
        }
    }

    private bool IsPlayer(GameObject gameObject)
    {
        return gameObject.CompareTag("Player");
    }
}
