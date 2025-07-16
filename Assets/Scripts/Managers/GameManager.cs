using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public string nextSpawn;
    private GameObject player;

    [Header("Finished Actions")]
    [SerializeField] public List<DialogueSO> dialogues {  get; private set; }
    [SerializeField] public List<string> finishedHints;

    [Header("Move")]
    [SerializeField] private float baseMoveSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float jumpCancelMulti;
    [SerializeField] private float airDragMovementModifier;

    [Header("Dash")]
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [Header("Sprint")]
    [SerializeField] private float timeToSprint;
    [SerializeField] private float maxSprintSpeed;
    [SerializeField] private float sprintSpeedIncrement;

    [Header("Unlocked Skills")]
    [SerializeField]  private bool canDoubleJump = false;
    [SerializeField] private bool canDash = false;
    [SerializeField] private bool canWallJump = false;
    [SerializeField] private bool canBlock = false;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private bool canRangeAttack = false;

    [Header("Stats")]
    [SerializeField] private float playerHP;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        dialogues = new List<DialogueSO>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SaveAbilityState();
    }

    public void SaveAbilityState()
    {
        if (player == null) return;

        PlayerMove pSkills = player.GetComponent<PlayerMove>();
        PlayerStats pStats = player.GetComponent<PlayerStats>();

        if (pSkills != null)
        {
            instance.playerHP = pStats.UnitCurrentHealth;
            instance.baseMoveSpeed = pSkills.baseMoveSpeed;

            instance.jumpForce = pSkills.jumpForce;
            instance.wallJumpForce = pSkills.wallJumpForce;
            instance.jumpCancelMulti = pSkills.jumpCancelMulti;
            instance.airDragMovementModifier = pSkills.airDragMovementModifier;

            instance.dashPower = pSkills.dashPower;
            instance.dashTime = pSkills.dashTime;
            instance.dashCooldown = pSkills.dashCooldown;
            
            instance.timeToSprint = pSkills.timeToSprint;
            instance.maxSprintSpeed = pSkills.maxSprintSpeed;
            instance.sprintSpeedIncrement = pSkills.sprintSpeedIncrement;
            
            instance.canDoubleJump = pSkills.canDoubleJump;
            instance.canDash = pSkills.canDash;
            instance.canWallJump = pSkills.canWallJump;
            instance.canBlock = pSkills.canBlock;
            instance.canAttack = pSkills.canAttack;
            instance.canRangeAttack = pSkills.canRangeAttack;
        }
    }

    private void LoadAbilityState()
    {
        if (player == null) return;

        PlayerMove pSkills = player.GetComponent<PlayerMove>();
        PlayerStats pStats = player.GetComponent<PlayerStats>();

        if (pSkills != null)
        {
            pStats.RemoveHealthOnAttack(pStats.UnitMaxHealth - playerHP, this.gameObject);

            pSkills.jumpForce = instance.jumpForce;
            pSkills.wallJumpForce = instance.wallJumpForce;
            pSkills.jumpCancelMulti = instance.jumpCancelMulti;
            pSkills.airDragMovementModifier = instance.airDragMovementModifier;

            pSkills.dashPower = instance.dashPower;
            pSkills.dashTime = instance.dashTime;
            pSkills.dashCooldown = instance.dashCooldown;

            pSkills.timeToSprint = instance.timeToSprint;
            pSkills.maxSprintSpeed = instance.maxSprintSpeed;
            pSkills.sprintSpeedIncrement = instance.sprintSpeedIncrement;

            pSkills.canDoubleJump = instance.canDoubleJump;
            pSkills.canDash = instance.canDash;
            pSkills.canWallJump = instance.canWallJump;
            pSkills.canBlock = instance.canBlock;
            pSkills.canAttack = instance.canAttack;
            pSkills.canRangeAttack = instance.canRangeAttack;
        }
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (instance != this) return;
        GameObject leftSpawn = GameObject.FindGameObjectWithTag("SpawnLeft");
        GameObject rightSpawn = GameObject.FindGameObjectWithTag("SpawnRight");
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (leftSpawn != null && nextSpawn == "Left")
            {
                player.transform.position = leftSpawn.transform.position;
            }
            if (rightSpawn != null && nextSpawn == "Right")
            {
                player.transform.position = rightSpawn.transform.position;
                player.GetComponent<PlayerMove>().Flip();
            }
            LoadAbilityState();
        }
    }

    public void AddFinishedDialogue(DialogueSO dialogue)
    {
        instance.dialogues.Add(dialogue);
    }

}
