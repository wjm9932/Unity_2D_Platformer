using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;

    [SerializeField] private Image background;
    [SerializeField] private Slider loadingBar;
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


        Application.targetFrameRate = 144;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GoToMainMenu()
    {
        gameObject.SetActive(true);
        loadAtcion = null;
        StartCoroutine(LoadSceneCoroutine("MainMenu"));
    }

    public void LoadScene(int sceneIndex, Action action)
    {
        gameObject.SetActive(true);
        loadAtcion = action;
        SceneManager.sceneLoaded += OnSceneLoaded;

        StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    public void ReloadCurrentScene()
    {
        gameObject.SetActive(true);
        StartCoroutine(LoadSceneCoroutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadSavedScene(Action action, int savedSceneIndex)
    {
        gameObject.SetActive(true);
        loadAtcion = action;
        SceneManager.sceneLoaded += OnSceneLoaded;

        StartCoroutine(LoadSceneCoroutine(savedSceneIndex));
    }

    public void LoadNextScene()
    {
        gameObject.SetActive(true);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(LoadSceneCoroutine(nextSceneIndex));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        loadingBar.value = 0f;
        yield return StartCoroutine(FadeInAndOut(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;

        float process = 0.0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                loadingBar.value = op.progress;
            }
            else
            {
                process += Time.deltaTime * 2.0f;
                loadingBar.value = Mathf.Lerp(0.9f, 1.0f, process);

                if (process > 1.0f)
                {
                    op.allowSceneActivation = true;
                    StartCoroutine(FadeInAndOut(false));
                    yield break;
                }
            }
        }

    }

    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        loadingBar.value = 0f;
        yield return StartCoroutine(FadeInAndOut(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);

        op.allowSceneActivation = false;

        float process = 0.0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                loadingBar.value = op.progress;
            }
            else
            {
                process += Time.deltaTime * 2.0f;
                loadingBar.value = Mathf.Lerp(0.9f, 1.0f, process);

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
        if (arg0.buildIndex > 1)
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
