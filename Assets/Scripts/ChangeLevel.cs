using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;


    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private bool IsPlayer(GameObject gameObject)
    {
        return gameObject.CompareTag("Player");
    }
}
