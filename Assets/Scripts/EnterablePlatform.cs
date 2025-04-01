using UnityEngine;
using UnityEngine.ProBuilder;

public class EnterablePlatform : MonoBehaviour
{
    private MeshCollider meshCollider;
    private ProBuilderMesh proBuilderMesh;
    private float yMaxPos;

    void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        proBuilderMesh = GetComponent<ProBuilderMesh>();
        yMaxPos = proBuilderMesh.GetComponent<Renderer>().bounds.max.y;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            CapsuleCollider playerCollider = other.GetComponent<CapsuleCollider>();
            if (playerCollider.bounds.min.y < yMaxPos)
            {
                Debug.Log("XDD");
                Physics.IgnoreCollision(other, meshCollider, true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            Debug.Log("huh");
            Physics.IgnoreCollision(other, meshCollider, false);
        }
    }

    private bool IsPlayer(GameObject gameObject)
    {
        return gameObject.CompareTag("Player");
    }
}
