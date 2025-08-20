using UnityEngine;
using UnityEngine.UI;
using JSAM;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetVolume(float value)
    {
        AudioManager.MasterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
}
