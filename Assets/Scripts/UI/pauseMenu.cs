using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool isGamePaused;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject pauseOptionsPanel;
    [SerializeField] private GameObject menuBackGround;

    void Start()
    {
        isGamePaused = false;
        pauseMenuPanel.gameObject.SetActive(false);
        menuBackGround.gameObject.SetActive(false);
        pauseOptionsPanel.gameObject.SetActive(false);
    }

    public void OnPause(InputAction.CallbackContext inputAction)
    {
        if (isGamePaused)
        {
            Resume();
        }
        else
        {
            EnablePanels();
            Time.timeScale = 0;
        }
    }

    private void DisablePanels()
    {
        isGamePaused = false;
        pauseMenuPanel.gameObject.SetActive(false);
        menuBackGround.gameObject.SetActive(false);
        pauseOptionsPanel.gameObject.SetActive(false);
    }

    private void EnablePanels()
    {
        isGamePaused = true;
        pauseMenuPanel.gameObject.SetActive(true);
        menuBackGround.gameObject.SetActive(true);
        pauseOptionsPanel.gameObject.SetActive(false);
        pauseMenuPanel.GetComponent<MainMenu>().firstMainMenuButton.Select();
    }

    public void Resume()
    {
        DisablePanels();
        Time.timeScale = 1;
    }
}
