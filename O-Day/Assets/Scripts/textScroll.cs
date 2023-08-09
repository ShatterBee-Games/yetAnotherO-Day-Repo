using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 
using UnityEngine.InputSystem;

public class textScroll : MonoBehaviour
{   
    Controls _controls;
    GameObject soundEffectsSource; 
    public AudioClip keyboardClick1;  
    public AudioClip keyboardClick2;
    public AudioClip keyboardClick3;

    [Header("Text Settings")]

    [SerializeField] [TextArea] private string[] itemInfo;

    [SerializeField] private float textSpeed = 0.01f;

    [Header("UI Element")]
    
    [SerializeField] private TextMeshProUGUI itemInfoText;

    private float skipTime = 45.0f;

    private int currentDisplayingText = 0;

    public void Awake(){
        ActivateText();
         _controls = new Controls();

         _controls.Player.Shoot.performed += ctx => GameStart();
         soundEffectsSource= GameObject.FindGameObjectWithTag("sound"); 
    }

    public void Update(){
        if (skipTime > 0f){
            skipTime -= Time.deltaTime;
        }

        if (skipTime<=0){
            GameStart();
        }
    }

    public void ActivateText(){
        StartCoroutine(AnimateText());

    }

    public void GameStart(){
        SceneManager.LoadScene(1);
    }

    IEnumerator AnimateText(){
        for(int i = 0; i < itemInfo[currentDisplayingText].Length+1; i++ ){
            itemInfoText.text=itemInfo[currentDisplayingText].Substring(0,i);
            float chance = Random.Range(0.0f, 3.0f);

            if(soundEffectsSource!=null && chance >2f){ 
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(keyboardClick1); 
            }  
            else if (soundEffectsSource!=null && chance >1f){
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(keyboardClick2); 
            }
            else if (soundEffectsSource!=null){
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(keyboardClick3); 
            }

            yield return new WaitForSeconds(textSpeed);

        }
    }

}
