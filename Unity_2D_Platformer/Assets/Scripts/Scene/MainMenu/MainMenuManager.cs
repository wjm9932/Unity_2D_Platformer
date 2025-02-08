using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        if(GameDataSaveLoadManager.Instance.FileExist() == true)
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        SceneLoadManager.Instance.LoadNextScene();
    }

    public void ContinueGame()
    {
        SceneLoadManager.Instance.LoadSavedScene(GameDataSaveLoadManager.Instance.GetContinuedSceneIndex(), null);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
