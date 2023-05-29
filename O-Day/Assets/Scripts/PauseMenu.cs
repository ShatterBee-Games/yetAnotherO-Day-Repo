using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //creates a pauseUI object and declares it's display and false (Shane)
    public static bool pauseDisplay = false;
    public GameObject pauseUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //checks if the pause menu is up and either brings it up or gets rid of it (Shane)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseDisplay)
            {
                pauseOff();
            } else
            {
                pauseOn();
            }

        }
    }
    //Feel free to edit this code and leave behind a comment of what I should've done instead (Shane)
    public void pauseOn()
    {
        pauseUI.SetActive(true);
        pauseDisplay = true;
        Time.timeScale = 0f;
    }
    public void pauseOff()
    {
        pauseUI.SetActive(false);
        pauseDisplay = false;
        Time.timeScale = 1f;
    }
}
