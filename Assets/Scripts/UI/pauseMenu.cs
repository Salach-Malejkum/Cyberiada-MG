using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    private bool isGamePaused;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject pauseOptionsPanel;
    [SerializeField] private GameObject menuBackGround;

    void Start()
    {
        Debug.Log("pauseStart");
        isGamePaused = false;
        pauseMenuPanel.gameObject.SetActive(false);
        menuBackGround.gameObject.SetActive(false);
    }

    void OnPause()
    {
        if (isGamePaused)
        {
            Debug.Log("unpause");
            DisablePanels();
            Time.timeScale = 1;
        }
        else
        {
            Debug.Log("pause");
            EnablePanels();
            Time.timeScale = 0;
        }
    }

    private void DisablePanels()
    {
        Debug.Log("disable");
        isGamePaused = false;
        pauseMenuPanel.gameObject.SetActive(false);
        menuBackGround.gameObject.SetActive(false);
        pauseOptionsPanel.gameObject.SetActive(false);
    }

    private void EnablePanels()
    {
        Debug.Log("enable");
        isGamePaused = true;
        pauseMenuPanel.gameObject.SetActive(true);
        menuBackGround.gameObject.SetActive(true);
        pauseOptionsPanel.gameObject.SetActive(false);
    }
}
