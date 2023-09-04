using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //creates a pauseUI object and declares it's display and false (Shane)
    public static bool pauseDisplay = false;
    public GameObject pauseUI;

    void Update()
    {
        //checks if the pause menu is up and either brings it up or gets rid of it (Shane)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            // removed if Stament and replaced it with one funtion :> -zoe
            pauseDisplay = !pauseDisplay;
            pauseUI.SetActive(pauseDisplay);
            float time = pauseDisplay ? 0f : 1f;
            Time.timeScale = time;

            //refrence AudioManager to turn down all audios in array in the AudioManager script when paused -zoe

            if (pauseDisplay)
            {
                AudioManager.instance.TurnDownAllAudio();
            }
            else
            {
                AudioManager.instance.RestoreAudioVolumes();
            }
        }
    }

    public void pauseToggleActive()
    {
        pauseDisplay = !pauseDisplay;
        pauseUI.SetActive(pauseDisplay);
        float time = pauseDisplay ? 0f : 1f;
        Time.timeScale = time;

        if (pauseDisplay)
        {
            AudioManager.instance.TurnDownAllAudio();
        }
        else
        {
            AudioManager.instance.RestoreAudioVolumes();
        }
    }

    /*

    Hey Shane! heres a little explination!

    pauseDisplay = !pauseDisplay; - sets the bool to oppostie of itself

    float time = pauseDisplay ? 0f : 1f; - creates a float named time and makes it work with bool pauseDisplay
    if pauseDisplay is true then 1f if false then 0f

    Time.timeScale = time; - then we just connect time to Time.timeScale

    hope this helps zoe <3
*/
}
