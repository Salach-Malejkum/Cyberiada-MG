using System;
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
    private CheckPointManager checkPointManager;

    private GameObject[] enemies;

    public event Action player;

    private void Awake()
    {
        checkPointManager = GameObject.FindGameObjectWithTag("checkPointManager").GetComponent<CheckPointManager>();
    }

    private void Start()
    {
        interactionMarker.enabled = false;
        interacted = false;
        anim = GetComponent<Animator>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkPointManager.setCheckPointInRage(this.gameObject);

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
            checkPointManager.removeCheckPointInRage();
            interactionMarker.enabled = false;
            playerInRangeOfCheckPoint = false;
            playerStats = null;
        }
    }

    public void Interact()
    {
        if (playerInRangeOfCheckPoint)
        {
            playerStats.UpdateRespawnCoordinates(new Vector3(transform.position.x, playerYPosition, 0f));
            playerStats.HealthRestored(playerStats.UnitMaxHealth);
            RespawnEnemies();
            if (!interacted)
            {
                interacted = true;
                anim.SetBool("interacted", interacted);
            }
        }
    }

    private void RespawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyStats enemyStats = enemies[i].GetComponent<EnemyStats>();
            EnemyPatrol enemyPatrol = enemies[i].GetComponent<EnemyPatrol>();
            enemies[i].SetActive(true);
            enemyStats.HealthRestored(enemyStats.UnitMaxHealth);
            enemyPatrol.onRespawnChangeColor();
        }
    }
}
