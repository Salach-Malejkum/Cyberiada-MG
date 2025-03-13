using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;

    public void StartGmae()
    {
        Debug.Log("start");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    public void ShowOptions()
    {
        this.gameObject.SetActive(false);
        optionsPanel.SetActive(true);
    }
}
