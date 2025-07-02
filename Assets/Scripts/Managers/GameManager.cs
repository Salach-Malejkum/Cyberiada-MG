using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public string nextSpawn;
    private GameObject player;

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
            playerHP = pStats.UnitCurrentHealth;
            baseMoveSpeed = pSkills.baseMoveSpeed;

            jumpForce = pSkills.jumpForce;
            wallJumpForce = pSkills.wallJumpForce;
            jumpCancelMulti = pSkills.jumpCancelMulti;
            airDragMovementModifier = pSkills.airDragMovementModifier;

            dashPower = pSkills.dashPower;
            dashTime = pSkills.dashTime;
            dashCooldown = pSkills.dashCooldown;
            
            timeToSprint = pSkills.timeToSprint;
            maxSprintSpeed = pSkills.maxSprintSpeed;
            sprintSpeedIncrement = pSkills.sprintSpeedIncrement;
            
            canDoubleJump = pSkills.canDoubleJump;
            canDash = pSkills.canDash;
            canWallJump = pSkills.canWallJump;
            canBlock = pSkills.canBlock;
            canAttack = pSkills.canAttack;

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

            pSkills.jumpForce = jumpForce;
            pSkills.wallJumpForce = wallJumpForce;
            pSkills.jumpCancelMulti = jumpCancelMulti;
            pSkills.airDragMovementModifier = airDragMovementModifier;

            pSkills.dashPower = dashPower;
            pSkills.dashTime = dashTime;
            pSkills.dashCooldown = dashCooldown;

            pSkills.timeToSprint = timeToSprint;
            pSkills.maxSprintSpeed = maxSprintSpeed;
            pSkills.sprintSpeedIncrement = sprintSpeedIncrement;

            pSkills.canDoubleJump = canDoubleJump;
            pSkills.canDash = canDash;
            pSkills.canWallJump = canWallJump;
            pSkills.canBlock = canBlock;
            pSkills.canAttack = canAttack;
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

}
