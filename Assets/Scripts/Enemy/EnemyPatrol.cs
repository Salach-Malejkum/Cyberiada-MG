using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("statstics")]
    [SerializeField] private float speed;
    [Header("Patrol")]
    [SerializeField] private GameObject leftEdge;
    [SerializeField] private GameObject rightEdge;
    [SerializeField] private float patrolEdgeSize = 0.5f;
    [SerializeField] private float patrolPauseTime = 2f;
    [Header("field of vision")]
    [SerializeField] private float fieldOfVisionVerticalRange;
    [SerializeField] private float fieldOfVisionHorisontalRange;
    [SerializeField] private float fieldOfVisionHorizontalOffset;
    [SerializeField] private float fieldOfVisionVerticalOffset;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private CapsuleCollider collider;

    private Transform currentDestination;
    private Rigidbody EnemyRb;
    RaycastHit playerHit;
    private Transform player;

    private bool chasingPlayer = false;
    private bool patrolWaitCancel = false;
    private bool isWaiting = false;

    void Start()
    {
        EnemyRb = GetComponent<Rigidbody>();
        currentDestination = rightEdge.transform;
    }

    void Update()
    {
        if (PlayerInSight())
        {
            chasingPlayer = true;
            patrolWaitCancel = true;
            player = playerHit.transform;
        }
        else if (chasingPlayer)
        {
            chasingPlayer = false;
            currentDestination = (Vector3.Distance(transform.position, leftEdge.transform.position) < Vector3.Distance(transform.position, rightEdge.transform.position))
                ? rightEdge.transform : leftEdge.transform;
            if (currentDestination.position.x > transform.position.x && transform.localScale.x < 0)
            {
                Flip();
            }
            if (currentDestination.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }
        }

        if (chasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (isWaiting) return;

        float moveDirection = (currentDestination == rightEdge.transform) ? 1 : -1;
        EnemyRb.linearVelocity = new Vector3(moveDirection * speed, 0f, 0f);

        if (Vector3.Distance(transform.position, currentDestination.position) < patrolEdgeSize)
        {
            StartCoroutine(PatrolPause());
        }
    }

    private IEnumerator PatrolPause()
    {
        isWaiting = true;
        EnemyRb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(patrolPauseTime);
        if (patrolWaitCancel)
        {
            PatrolPauseCanceld();
        }
        else
        {
            Flip();
            currentDestination = (currentDestination == rightEdge.transform) ? leftEdge.transform : rightEdge.transform;
            isWaiting = false;
            Debug.Log("canceld?");
        }
    }

    private void PatrolPauseCanceld()
    {
        isWaiting = false;
        patrolWaitCancel = false;
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        bool hasGroundAhead = CheckGroundAhead(direction.x);

        if (isWaiting)
        {
            PatrolPauseCanceld();
        }

        if (hasGroundAhead)
        {
            EnemyRb.linearVelocity = new Vector3(direction.x * speed, 0f, 0f);

            if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
            {
                Flip();
            }
        }
        else
        {
            chasingPlayer = false;
            currentDestination = (Vector3.Distance(transform.position, leftEdge.transform.position) < Vector3.Distance(transform.position, rightEdge.transform.position))
                ? rightEdge.transform : leftEdge.transform;

            if (currentDestination.position.x > transform.position.x && transform.localScale.x < 0)
            {
                Flip();
            }
            if (currentDestination.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

    // Checks if there's ground in front of the enemy
    private bool CheckGroundAhead(float directionX)
    {
        Vector3 rayOrigin = transform.position + new Vector3(directionX * patrolEdgeSize, 0f, 0f);
        float rayLength = 1.5f; // Adjust based on how far below the enemy should detect ground
        return Physics.Raycast(rayOrigin, Vector3.down, rayLength, LayerMask.GetMask("Ground"));
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private bool PlayerInSight()
    {
        Vector3 origin = collider.bounds.center + new Vector3(fieldOfVisionHorizontalOffset * transform.localScale.x, fieldOfVisionVerticalOffset, 0f);
        Vector3 direction = transform.right * transform.localScale.x;
        Vector3 fieldOfVisionSize = new Vector3(0f, fieldOfVisionVerticalRange, 0f);
        origin += direction * (fieldOfVisionSize.x / 2);
        return Physics.BoxCast(origin, fieldOfVisionSize / 2, direction, out playerHit, Quaternion.identity, fieldOfVisionHorisontalRange, playerLayer);
    }

    private void OnDrawGizmos()
    {
        if (leftEdge == null || rightEdge == null || collider == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(leftEdge.transform.position, patrolEdgeSize);
        Gizmos.DrawWireSphere(rightEdge.transform.position, patrolEdgeSize);
        Gizmos.DrawLine(leftEdge.transform.position, rightEdge.transform.position);

        Gizmos.color = Color.red;
        Vector3 origin = collider.bounds.center + new Vector3(fieldOfVisionHorizontalOffset * transform.localScale.x, fieldOfVisionVerticalOffset, 0f);
        Vector3 fieldOfVisionSize = new Vector3(0f, fieldOfVisionVerticalRange, 0f);
        Gizmos.DrawWireCube(origin + transform.right * transform.localScale.x * fieldOfVisionHorisontalRange / 2, fieldOfVisionSize);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + transform.right * transform.localScale.x * fieldOfVisionHorisontalRange);
    }
}
