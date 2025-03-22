using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPoint : MonoBehaviour
{
    private bool playerInRangeOfCheckPoint = false;
    private PlayerStats playerStats;
    private float playerYPosition;
    [SerializeField] private SpriteRenderer interactionMarker;
    private Animator anim;

    private void Start()
    {
        interactionMarker.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionMarker.enabled = true;
            playerInRangeOfCheckPoint = true;
            playerStats = other.GetComponent<PlayerStats>();
            playerYPosition = other.gameObject.transform.position.y;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionMarker.enabled = false;
            playerInRangeOfCheckPoint = false;
            playerStats = null;
        }
    }

    void OnTalk(InputValue inputValue)
    {
        if (playerInRangeOfCheckPoint)
        {
            playerStats.UpdateRespownCoordinates(new Vector3(transform.position.x, playerYPosition, 0f));
            //start animation of checkpoint if not started
        }
    }
}
