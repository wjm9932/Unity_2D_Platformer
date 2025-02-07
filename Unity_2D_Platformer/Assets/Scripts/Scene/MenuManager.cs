using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject respawnMenu;
    [SerializeField] private GameObject firstSelectedButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneLoadManager.Instance.GoToMainMenu();
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void ActiveDieScreen(bool flag)
    {
        respawnMenu.SetActive(flag);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        pauseMenu.SetActive(true);
    }

    public void PauseOrResume()
    {
        if(pauseMenu.activeSelf == true)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}
