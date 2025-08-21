using UnityEngine;
using JSAM;

public class PlayMusic : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.OnAudioManagerInitialized += HandleAudioInitialized;
        if (AudioManager.Instance.Initialized)
        {
            HandleAudioInitialized();
        }
    }
    private void OnDestroy()
    {
        AudioManager.OnAudioManagerInitialized -= HandleAudioInitialized;
    }

    private void HandleAudioInitialized()
    {
        Debug.Log("Music initialized");
        AudioManager.PlayMusic(AudioLibraryMusic.MusicFX, true);
        Debug.Log("Now playing: " + AudioLibraryMusic.MusicFX);

    }
}
