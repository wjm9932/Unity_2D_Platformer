using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.IO;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;

    [SerializeField] private Image background;
    [SerializeField] private Image loadingBar;
    [SerializeField] private GameObject loadingScene;

    private Action? loadAtcion;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("MainMenu");
        gameObject.SetActive(false);
    }

    public void GoToMainMenu()
    {
        gameObject.SetActive(true);
        loadAtcion = null;
        StartCoroutine(LoadSceneCoroutine("MainMenu"));
    }

    public void LoadScene(string scene, Action action)
    {
        gameObject.SetActive(true);
        loadAtcion = action;
        SceneManager.sceneLoaded += OnSceneLoaded;

        StartCoroutine(LoadSceneCoroutine(scene));
    }


    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        loadingBar.fillAmount = 0f;
        yield return StartCoroutine(FadeInAndOut(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;

        float process = 0.0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                loadingBar.fillAmount = op.progress;
            }
            else
            {
                process += Time.deltaTime * 2.0f;
                loadingBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, process);

                if (process > 1.0f)
                {
                    op.allowSceneActivation = true;
                    StartCoroutine(FadeInAndOut(false));
                    yield break;
                }
            }
        }

    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "GameScene")
        {
            StartCoroutine(LateStart());
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }


    private IEnumerator FadeInAndOut(bool isStartLoading)
    {
        yield return StartCoroutine(Fade(true));

        loadingScene.SetActive(isStartLoading);

        yield return StartCoroutine(Fade(false));

        if(isStartLoading == false)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float process = 0f;
        float duration = 0.5f;
        Color color = background.color;

        while (process < 1.0f)
        {
            process += Time.deltaTime / duration;
            color.a = isFadeIn ? Mathf.Lerp(0.0f, 1.0f, process) : Mathf.Lerp(1.0f, 0.0f, process);
            background.color = color;
            yield return null;
        }

        color.a = isFadeIn ? 1.0f : 0.0f;
        background.color = color;
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        loadAtcion?.Invoke();
    }

}
