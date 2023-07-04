using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class scenemanager : MonoBehaviour
{

    public void MainMenu()
    {
        SceneManager.LoadScene(0); 
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1); //starts the game (Shane)
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
