using UnityEngine;
using UnityEngine.SceneManagement;

public static class DataGame
{
    public static int currentLevel;
    public static bool isMainMenu = true;

    public static void LevelUp()
    {
        currentLevel += 1;
        if (currentLevel >= SceneManager.sceneCountInBuildSettings)
        {
            currentLevel = 1;
        }
        SaveGame(currentLevel);
    }

    public static void SaveGame(int currentLevel)
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.Save();//HKEY_CURRENT_USER/Software/Unity/UnityEditor/DefaultCompany        
    }

    public static int GetCurrentLevel()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            return PlayerPrefs.GetInt("CurrentLevel");            
        }
        else
        {
            return 1;
        }        
    }
}
