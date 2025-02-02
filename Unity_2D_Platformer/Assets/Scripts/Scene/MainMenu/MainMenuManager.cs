using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneLoadManager.Instance.LoadNextScene(null);
    }

    public void ContinueGame()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
