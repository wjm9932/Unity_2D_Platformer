using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    public void StartNewGame()
    {
        SceneLoadManager.Instance.LoadNextScene();
    }

    public void ContinueGame()
    {
        //SceneLoadManager.Instance.LoadSavedScene();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
