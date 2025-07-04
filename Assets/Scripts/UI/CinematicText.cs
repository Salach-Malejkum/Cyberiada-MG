using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class CinematicText : MonoBehaviour
{
    [SerializeField] private Animator[] lines;
    [SerializeField] private GameObject button;
    [SerializeField] private float waitTime = 5.0f;
    [SerializeField] private string sceneName;
    [SerializeField] private VideoPlayer videoPlayer;

    private int curr_line = 0;

    void Start()
    {
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
        button.SetActive(false);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        StartCoroutine(showLine());
    }

    IEnumerator showLine()
    {
        yield return new WaitForSeconds(1);
        if (curr_line < lines.Length)
        {
            lines[curr_line].Play("line_fade");
            curr_line++;
        }
        else
        {
            button.SetActive(true);
        }
        
        

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(showLine());
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
