using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Statstics")]
    [SerializeField] private float speed;

    [Header("Patrol")]
    [SerializeField] private GameObject leftEdge;
    [SerializeField] private GameObject rightEdge;
    [SerializeField] private float patrolEdgeSize = 0.5f;
    [SerializeField] private float patrolPauseTime = 2f;
    private int moveDirection;

    [Header("Field of vision")]
    [SerializeField] private float fieldOfVisionVerticalRange;
    [SerializeField] private float fieldOfVisionHorisontalRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform transformer;

    [Header("Attack zone")]
    [SerializeField] private Transform attackPosition;

    private Transform currentDestination;
    private Rigidbody enemyRb;
    RaycastHit playerHit;
    private Transform player;

    private bool chasingPlayer = false;
    private IPlayerInAttackRange playerInAttackRange;
    private bool patrolWaitCancel = false;
    private bool isWaiting = false;
    private SpriteRenderer renderer;
    public Animator anim;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        currentDestination = rightEdge.transform;
        moveDirection = 1;
        playerInAttackRange = GetComponent<IPlayerInAttackRange>();
        renderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
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
            EndingChase();
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

        enemyRb.linearVelocity = new Vector3(moveDirection * speed, 0f, 0f);
        if (anim != null)
            anim.SetFloat("Speed", enemyRb.linearVelocity.magnitude);


        if (Vector3.Distance(transform.position, currentDestination.position) < patrolEdgeSize)
        {
            StartCoroutine(PatrolPause());
        }
    }

    private IEnumerator PatrolPause()
    {
        isWaiting = true;
        enemyRb.linearVelocity = Vector3.zero;
        if (anim != null)
            anim.SetFloat("Speed", enemyRb.linearVelocity.magnitude);
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
            if (playerInAttackRange.PlayerInAttackRange() || playerInAttackRange.EnemyAttacking())
            {
                enemyRb.linearVelocity = new Vector3(0f, 0f, 0f);
                if (anim != null)
                    anim.SetFloat("Speed", enemyRb.linearVelocity.magnitude);
                playerInAttackRange.EnemyReadyToAttack();
            }
            else
            {
                enemyRb.linearVelocity = new Vector3(direction.x * speed, 0f, 0f);
                if (anim != null)
                    anim.SetFloat("Speed", enemyRb.linearVelocity.magnitude);
            }

            if ((direction.x > 0 && !renderer.flipX) || (direction.x < 0 && renderer.flipX))
            {
                Flip();
            }
        }
        else
        {
            EndingChase();
        }
    }

    private bool CheckGroundAhead(float directionX)
    {
        Vector3 rayOrigin = transform.position + new Vector3(directionX * patrolEdgeSize, 0f, 0f);
        float rayLength = 2.5f;
        return Physics.Raycast(rayOrigin, Vector3.down, rayLength, LayerMask.GetMask("Ground"));
    }

    private void Flip()
    {
        renderer.flipX = !renderer.flipX;

        Vector3 visionPosition = this.transformer.localPosition;
        visionPosition.x *= -1;
        this.transformer.localPosition = visionPosition;
        Vector3 attackPosition = this.attackPosition.localPosition;
        attackPosition.x *= -1;
        this.attackPosition.localPosition = attackPosition;
        moveDirection *= -1;
    }

    public bool PlayerInSight()
    {
        Vector3 fieldOfVisionSize = new Vector3(fieldOfVisionHorisontalRange, fieldOfVisionVerticalRange, 5f);
        RaycastHit[] hits = Physics.BoxCastAll(transformer.position, fieldOfVisionSize/2, transform.right, Quaternion.identity, 0f, playerLayer);

        if (hits.Length > 0)
        {
            playerHit = hits[0];
            return true;
        }
        return false;
    }

    private void EndingChase()
    {
        chasingPlayer = false;
        currentDestination = (Vector3.Distance(transform.position, leftEdge.transform.position) < Vector3.Distance(transform.position, rightEdge.transform.position))
            ? rightEdge.transform : leftEdge.transform;
        if ((currentDestination.position.x > transform.position.x) ^ (renderer.flipX))
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        if (leftEdge == null || rightEdge == null || transformer == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(leftEdge.transform.position, patrolEdgeSize);
        Gizmos.DrawWireSphere(rightEdge.transform.position, patrolEdgeSize);
        Gizmos.DrawLine(leftEdge.transform.position, rightEdge.transform.position);

        Gizmos.color = Color.red;
        Vector3 fieldOfVisionSize = new Vector3(fieldOfVisionHorisontalRange, fieldOfVisionVerticalRange, 5f);
        Gizmos.DrawWireCube(transformer.position, fieldOfVisionSize);
    }
}
