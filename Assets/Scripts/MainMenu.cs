using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private string rollaball;

    public void PlayGame()
    {
        SceneManager.LoadScene(rollaball);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
