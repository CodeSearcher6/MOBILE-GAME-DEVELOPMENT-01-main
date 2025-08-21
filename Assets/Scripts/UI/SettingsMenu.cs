using UnityEngine;
using UnityEngine.UI;
using JSAM;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private const string MasterVolumeKey = "MasterVolume";
    private const float DefaultVolume = 1f;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);

        float savedVolume = PlayerPrefs.GetFloat(MasterVolumeKey, DefaultVolume);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetVolume(float value)
    {
        AudioManager.MasterVolume = value;
        PlayerPrefs.SetFloat(MasterVolumeKey, value);
    }
}
