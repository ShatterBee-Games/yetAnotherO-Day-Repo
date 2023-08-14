using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //allows script to manage scene

public class nextScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void nextScene()
    {
        SceneManager.LoadScene(1); //starts the game (Shane)
    }
}
