using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject mainPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("optionsStart");
        volumeSlider.onValueChanged.AddListener(delegate { MusicVolumeCheck(); });
        this.gameObject.SetActive(false);
    }

    private void MusicVolumeCheck()
    {
        Debug.Log(volumeSlider.value);
    }

    public void Back()
    {
        Debug.Log("Back");
        this.gameObject.SetActive(false);
        mainPanel.SetActive(true);
    }
}
