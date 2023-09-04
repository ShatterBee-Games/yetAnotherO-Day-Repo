using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // a script to store all audios that we want turned down on command in one go! -zoe
    // for the pause menu 


    public static AudioManager instance;
    public AudioSource[] audioSources; // Array of all your audio sources

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Function to turn down all audio sources
    public void TurnDownAllAudio()
    {
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = 0.0f;
        }
    }

    // Function to restore original audio volumes
    public void RestoreAudioVolumes()
    {
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = 1.0f;
        }
    }
}
