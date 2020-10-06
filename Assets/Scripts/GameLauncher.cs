using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{    
    private void Start()
    {
        DataGame.currentLevel = DataGame.GetCurrentLevel();           
        SceneManager.LoadScene("Level" + DataGame.currentLevel);
    }
}
