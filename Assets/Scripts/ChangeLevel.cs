using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Animator transitionAnimator;
    

    public spawnPos nextSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            GameManager.instance.nextSpawn = nextSpawn.ToString();
            StartCoroutine(LoadLevel());
        }
    }

    private bool IsPlayer(GameObject gameObject)
    {
        return gameObject.CompareTag("Player");
    }

    IEnumerator LoadLevel()
    {
        transitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
        transitionAnimator.SetTrigger("Start");
    }
}

public enum spawnPos
{
    Left,
    Right
};
