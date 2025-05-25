using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPoint : MonoBehaviour
{
    private bool playerInRangeOfCheckPoint = false;
    private PlayerStats playerStats;
    private float playerYPosition;
    [SerializeField] private SpriteRenderer interactionMarker;
    private Animator anim;
    private bool interacted;

    private void Start()
    {
        interactionMarker.enabled = false;
        interacted = false;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionMarker.enabled = true;
            playerInRangeOfCheckPoint = true;
            playerStats = other.GetComponent<PlayerStats>();
            playerYPosition = other.gameObject.transform.position.y;
            OnTalkDebug();
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
            playerStats.UpdateRespawnCoordinates(new Vector3(transform.position.x, playerYPosition, 0f));
            if (!interacted)
            {
                interacted = true;
                anim.SetBool("interacted", interacted);
                transform.position = transform.position + new Vector3(0, 0.5f, 0);
            }
        }
    }

    void OnTalkDebug() // TODO DO WYJEBANIA
    {
        if (playerInRangeOfCheckPoint)
        {
            playerStats.UpdateRespawnCoordinates(new Vector3(transform.position.x, playerYPosition, 0f));
            if (!interacted)
            {
                interacted = true;
                anim.SetBool("interacted", interacted);
                transform.position = transform.position + new Vector3(0, 0.5f, 0);
            }
        }
    }
}
