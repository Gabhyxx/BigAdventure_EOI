using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    void DoNotDestroyOnLoad()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RestartScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
