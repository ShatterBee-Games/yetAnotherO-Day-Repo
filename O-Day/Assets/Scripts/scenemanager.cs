using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scenemanager : MonoBehaviour
{


   public bool isMenu = false;
   public bool isIntro = false;
   public bool isEnd = false;
   public bool isCredits = false;


    Animator anim;

    void Start(){
        anim = GetComponent<Animator>();

    }

     void Update()
    {
        //updated so that pressing anykey works on menu -zoe

        // checks if active scene is menu if so sets is menu to true otherwise its set back to false
        isMenu = SceneManager.GetActiveScene().name == "MenuScene";
        
         // if any key is pressed && where on menu scene && mouse is not over a UI emelement lets start the game! 
        if (Input.anyKey && isMenu && !IsPointerOverUIElement())
        {
            clickedIntro();
        }

        if (Input.anyKey && isIntro)
        {
            StartGame();
        }

        if (Input.anyKey && isEnd)
        {
            startCredits();
        }

        if (Input.anyKey && isCredits)
        {
            MainMenu();
        }
    }

    public void MainMenu()
    {
        //make sure that Time is back to normal when goign to main menu -zoe
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1); //starts the game (Shane)
    }
    public void StartIntro(){
        SceneManager.LoadScene(4);
    }
    public void startCredits(){
        SceneManager.LoadScene(3);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void clickedIntro(){
        anim.SetTrigger("startIntro");
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    
}
