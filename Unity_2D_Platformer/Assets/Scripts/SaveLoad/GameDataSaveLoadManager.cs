using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameDataSaveLoadManager : MonoBehaviour
{
    public static GameDataSaveLoadManager Instance;
    private string dataPath;
    private const string saveFileName = "saveData.json";
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        dataPath = Path.Combine(Application.dataPath, "Save Data");
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveGameData()
    {
        SaveData data = new SaveData();

        data.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        string path = Path.Combine(dataPath, saveFileName);
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, jsonData);
    }

    public int GetContinuedSceneIndex()
    {
        string path = Path.Combine(dataPath, saveFileName);
        string jsonData = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

        return saveData.currentSceneIndex;
    }

    public bool FileExist()
    {
        string path = Path.Combine(dataPath, saveFileName);
        return File.Exists(path);
    }
}
