using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [Header("Bounds")]
    [SerializeField] private Transform leftBounds;
    [SerializeField] private Transform rightBounds;
    [SerializeField] private Transform centerPoint;

    [Header("Boss Params")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private float slideTimer = 3f;

    [Header("Renderer")]
    [SerializeField] private Color onHitColor = Color.red;
    [SerializeField] private Renderer onHitRenderer;

    [Header("Attack")]
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float bossDamage = 25f;
    [SerializeField] private float runRange = 4f;

    private Rigidbody rb;
    private Animator animator;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private float moveRight;
    private float speed = 2f;

    private bool returning = false;

    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        moveRight = 1f;
        speed = walkSpeed;
    }

    private bool IsPlayerInBounds()
    {
        if (player == null) return false;
        else if (player.transform.position.x > leftBounds.position.x && player.transform.position.x < rightBounds.transform.position.x)
        {
            return true;
        }
        return false;
    }

    private bool BossInBounds()
    {
        if (player == null) return false;
        else if (this.transform.position.x > leftBounds.position.x && this.transform.position.x < rightBounds.transform.position.x)
        {
            return true;
        }
        returning = true;
        return false;
    }

    private void HandleFlip()
    {
        if (rb.linearVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private bool isAttacking;
    private bool canSlide = true;
    void FixedUpdate()
    {
        rb.linearVelocity = Vector3.zero;
        if (isAttacking) return;

        if (Vector3.Distance(new Vector3(this.transform.position.x, 0f, 0f), new Vector3(player.transform.position.x, 0f, 0f)) > runRange && canSlide)
        {
            canSlide = false;
            StartCoroutine(SlideOffCooldown());
        }

        if (!BossInBounds() || returning)
        {
            if (Vector3.Distance(this.transform.position, centerPoint.transform.position) < 2f)
            {
                returning = false;
            }
            else if (centerPoint.transform.position.x - this.transform.position.x < 0)
            {
                rb.linearVelocity = new Vector3(-moveRight * speed, 0f, 0f);
            }
            else
            {
                rb.linearVelocity = new Vector3(moveRight * speed, 0f, 0f);
            }
        }
        else if (IsPlayerInBounds())
        {
            returning = false;
            
            if (Vector3.Distance(new Vector3(this.transform.position.x, 0f, 0f), new Vector3(player.transform.position.x, 0f, 0f)) < attackRange)
            {
                StartCoroutine(attackAnim());
            }
            if (player.transform.position.x - this.transform.position.x < 0)
            {
                rb.linearVelocity = new Vector3(-moveRight * speed, 0f, 0f);
            }
            else
            {
                rb.linearVelocity = new Vector3(moveRight * speed, 0f, 0f);
            }
        }
        else
        {
            returning = true;
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        HandleFlip();
    }

    IEnumerator attackAnim()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("ExitAttack");
        isAttacking = false;
    }

    IEnumerator SlideOffCooldown()
    {
        speed = slideSpeed;
        yield return new WaitForSeconds(slideTimer);
        speed = walkSpeed;
        StartCoroutine(SlideResetCooldown());
    }

    IEnumerator SlideResetCooldown()
    {
        yield return new WaitForSeconds(slideTimer);
        canSlide = true;
    }

    private RaycastHit[] hits;
    private void DealMeleeDamage()
    {
        hits = Physics.SphereCastAll(attackTransform.position, attackRange, transform.right, 0f, playerLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            PlayerStats playerStats = hits[i].collider.gameObject.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.RemoveHealthOnAttack(bossDamage, this.gameObject);
            }
        }
    }

    public void onHitChangeColor()
    {
        StartCoroutine(changeColor());
    }

    IEnumerator changeColor()
    {
        onHitRenderer.material.SetColor("_SolidColor", onHitColor);
        yield return new WaitForSeconds(0.2f);
        onHitRenderer.material.SetColor("_SolidColor", new Color(0.0f, 0.0f, 0.0f, 0.0f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
}
