using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //creates a pauseUI object and declares it's display and false (Shane)
    public static bool pauseDisplay = false;
    public GameObject pauseUI;

    void Update()
    {
        //checks if the pause menu is up and either brings it up or gets rid of it (Shane)
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        // removed if Stament and replaced it with one funtion :> -zoe
            pause();

            /*
            ////// REMOVED //
            if (pauseDisplay)
            {
                pauseOff();
            } else
            {
                pauseOn();
            }
            ///////////////
            */

        }
    }


// Replaced 2 functions with 1 -zoe

 public void pause()
 {
    pauseDisplay = !pauseDisplay;
    pauseUI.SetActive(pauseDisplay);
    float time = pauseDisplay ? 0f : 1f;
    Time.timeScale = time;
 }

/*

Hey Shane! heres a little explination! 

pauseDisplay = !pauseDisplay; - sets the bool to oppostie of itself

float time = pauseDisplay ? 0f : 1f; - creates a float named time and makes it work with bool pauseDisplay 
if pauseDisplay is true then 1f if false then 0f 

Time.timeScale = time; - then we just connect time to Time.timeScale

hope this helps zoe <3 


///////////// REMOVED ///////////////////////////

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
///////////////////////////////////////////////////
*/


}
