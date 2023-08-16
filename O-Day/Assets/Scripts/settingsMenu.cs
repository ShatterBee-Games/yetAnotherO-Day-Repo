using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class settingsMenu : MonoBehaviour
{
    [SerializeField, Tooltip("vol slider")]
    Slider volSlider;

    public static bool settingsDisplay = false;
    public GameObject settingsUI;

    public static bool controlsDisplay = false;
    public GameObject controlsUI;
    
    public static bool pauseDisplay = false;
    public GameObject pauseUI;
    
    void Awake(){
        volSlider.value = PlayerPrefs.GetFloat("volume", 1f);
        float vol = volSlider.value;
        AudioListener.volume = vol;
    }

    public void setVolume(float volume){
        PlayerPrefs.SetFloat("volume", volume);
        AudioListener.volume = volume;
    }

    public void setFullScreen(bool val){
        Screen.fullScreen = val;
    }

    public void settingsToggleActive(){
        settingsDisplay = !settingsDisplay;
        settingsUI.SetActive(settingsDisplay);
    }

    public void controlsToggleActive(){
        controlsDisplay = !controlsDisplay;
        controlsUI.SetActive(controlsDisplay);

    }

    public void pauseToggleActive(){
        Debug.Log("toggle");
        pauseDisplay = !pauseDisplay;
        pauseUI.SetActive(pauseDisplay);
    }


}
