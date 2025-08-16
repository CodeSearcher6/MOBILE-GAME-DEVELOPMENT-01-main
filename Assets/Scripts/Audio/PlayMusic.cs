using JSAM;
using UnityEngine;
using System.Threading.Tasks;


public class PlayMusic : MonoBehaviour

{
    private async void Start()
    {
        await Task.Delay(1000); // Wait for 1 second before playing music

        AudioManager.MasterVolume = 0.5f; // Set the master volume to 50%
        var element = AudioManager.PlayMusic(AudioLibraryMusic.MusicFX, true); // Play the main menu music
    }
}
