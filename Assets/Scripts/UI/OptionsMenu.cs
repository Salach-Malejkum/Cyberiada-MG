using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private GameObject mainPanel;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        masterVolumeSlider.value = SFXManager.instance.masterVolume;
        musicVolumeSlider.value = SFXManager.instance.musicVolume;
        sfxVolumeSlider.value = SFXManager.instance.sfxVolume;

        masterVolumeSlider.onValueChanged.AddListener(delegate { MasterVolumeCheck(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { MusicVolumeCheck(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SFXVolumeCheck(); });

        masterVolumeSlider.Select();
        this.gameObject.SetActive(false);
    }

    private void MasterVolumeCheck()
    {
        SFXManager.instance.masterVolume = masterVolumeSlider.value;
    }
    private void MusicVolumeCheck()
    {
        SFXManager.instance.musicVolume = musicVolumeSlider.value;
    }
    private void SFXVolumeCheck()
    {
        SFXManager.instance.sfxVolume = sfxVolumeSlider.value;
    }

    public void Back()
    {
        this.gameObject.SetActive(false);
        mainPanel.SetActive(true);
    }
}
