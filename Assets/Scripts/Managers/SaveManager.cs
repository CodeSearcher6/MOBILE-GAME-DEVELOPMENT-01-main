using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveHighScore(int highScore)
    {
        SaveData data = new SaveData { highScore = highScore };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Save path: " + savePath);
    }

    public static int LoadHighScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.highScore;
        }
        return 0; 
    }
}
