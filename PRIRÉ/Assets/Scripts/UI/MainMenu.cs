using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //load game scene
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //quit game 
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
