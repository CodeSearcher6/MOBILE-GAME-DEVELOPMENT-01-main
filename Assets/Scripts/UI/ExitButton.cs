using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("RunnerScene"); 
    }
}
