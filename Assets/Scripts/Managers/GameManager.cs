using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public string nextSpawn;
    
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

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        GameObject leftSpawn = GameObject.FindGameObjectWithTag("SpawnLeft");
        GameObject rightSpawn = GameObject.FindGameObjectWithTag("SpawnRight");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

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
