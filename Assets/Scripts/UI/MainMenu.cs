using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject optionsPanel;

    public void StartGmae()
    {
        Debug.Log("start");
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
        this.gameObject.SetActive(false);
        Debug.Log("options");
    }
}
