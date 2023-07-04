using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //allows script to manage scene
//Thanks Brackeys
public class StartMenuScript : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene(1); //starts the game (Shane)
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
