using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public string nextSpawn;
    private GameObject player;

    [Header("Unlocked Skills")]
    [SerializeField] private bool canDoubleJump = false;
    [SerializeField] private bool canDash = false;
    [SerializeField] private bool canWallJump = false;
    [SerializeField] private bool canBlock = false;
    [SerializeField] private bool canAttack = false;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SaveAbilityState();
    }

    public void SaveAbilityState()
    {
        if (player == null) return;

        PlayerMove pSkills = player.GetComponent<PlayerMove>();

        if (pSkills != null)
        {
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

        if (pSkills != null)
        {
            pSkills.canDoubleJump = canDoubleJump;
            pSkills.canDash = canDash;
            pSkills.canWallJump = canWallJump;
            pSkills.canBlock = canBlock;
            pSkills.canAttack = canAttack;
        }
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        GameObject leftSpawn = GameObject.FindGameObjectWithTag("SpawnLeft");
        GameObject rightSpawn = GameObject.FindGameObjectWithTag("SpawnRight");
        player = GameObject.FindGameObjectWithTag("Player");
        LoadAbilityState();

        if (player != null)
        {
            if (leftSpawn != null && nextSpawn == "Left")
            {
                player.transform.position = leftSpawn.transform.position;
            }
            if (rightSpawn != null && nextSpawn == "Right")
            {
                player.transform.position = rightSpawn.transform.position;
                player.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

}
