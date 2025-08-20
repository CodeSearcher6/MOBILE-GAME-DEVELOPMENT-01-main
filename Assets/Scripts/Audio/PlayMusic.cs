using System.Threading.Tasks;
using JSAM;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    private async void Start()
    {
        await Task.Delay(1000); // Wait for 1 second before playing musicCollapse commentComment on line R9Vonk47 commented on Aug 18, 2025 Vonk47on Aug 18, 2025ContributorДодати очікування, писав в Slack як це зробитиWrite a replyCode has comments. Press enter to view.
        var element = AudioManager.PlayMusic(AudioLibraryMusic.MusicFX, true);
        AudioManager.OnAudioManagerInitialized();
        AudioManager.MasterVolume = 0.5f; // Set the music volume to 50%
    }

}