using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class scenemanager : MonoBehaviour
{
   public bool isMenu = false;
   public bool isIntro = false;
   public bool isEnd = false;

     void Update()
    {
        if (Input.anyKey && isMenu)
        {
            StartIntro();
        }

        if (Input.anyKey && isIntro)
        {
            StartGame();
        }

        if (Input.anyKey && isEnd)
        {
            MainMenu();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0); 
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1); //starts the game (Shane)
    }
    public void StartIntro(){
        SceneManager.LoadScene(4);
    }
    public void EndGame()
    {
        Application.Quit();
    }


}
