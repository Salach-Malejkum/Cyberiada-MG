using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CinematicText : MonoBehaviour
{
    [SerializeField] private Animator[] lines;
    [SerializeField] private GameObject button;
    [SerializeField] private float waitTime = 5.0f;
    [SerializeField] private string sceneName;

    private int curr_line = 0;

    void Start()
    {
        button.SetActive(false);
        StartCoroutine(showLine());
    }

    IEnumerator showLine()
    {
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
