using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCon : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private string sceneName;

    private void OnCollisionEnter(Collision collision)
    {
        endScreen.SetActive(true);
        StartCoroutine(EndGame());
    }


    private IEnumerator EndGame()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}