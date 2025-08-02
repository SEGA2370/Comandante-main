using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string LastLevelKey = "LastLevelIndex";

    public static void SaveLevelProgress(int levelIndex)
    {
        PlayerPrefs.SetInt(LastLevelKey, levelIndex);
        PlayerPrefs.Save();
    }

    public static int LoadLevelProgress()
    {
        return PlayerPrefs.GetInt(LastLevelKey, 1); // по умолчанию уровень 1
    }

    public static bool HasSavedProgress()
    {
        return PlayerPrefs.HasKey(LastLevelKey);
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LastLevelKey);
        PlayerPrefs.Save();
    }
}
