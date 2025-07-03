using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private EventReference switchButton;
    [SerializeField] private EventReference clickButton;
    [SerializeField] public Button firstMainMenuButton;
    [SerializeField] private Button firstOptionsButton;

    /*private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        firstButton.Select();
    }*/

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        firstMainMenuButton.Select();
    }

    public void StartGmae()
    {
        SceneManager.LoadScene(sceneName);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
        firstOptionsButton.Select();
        this.gameObject.SetActive(false);
    }

    public void PlayOnClick()
    {
        SFXManager.instance.PlayOneShot(clickButton, this.transform.position);
    }

    public void PlayOnFocus()
    {
        SFXManager.instance.PlayOneShot(switchButton, this.transform.position);
    }
}
