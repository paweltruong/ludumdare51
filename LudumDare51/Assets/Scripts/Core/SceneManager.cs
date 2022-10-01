using UnityEngine;

public class SceneManager : MonoBehaviour
{
    const string SCENE_MAINMENU = "MainMenu";
    const string SCENE_GAME = "Game";

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE_MAINMENU);
    }

    public void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE_GAME);
    }

    public void GoToWindows()
    {
        Application.Quit();
    }
}
