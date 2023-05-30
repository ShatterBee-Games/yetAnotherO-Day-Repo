using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Health system -zoe
public class hearts : MonoBehaviour
{
    public int health;

[SerializeField, Tooltip("Max 8")]
    public int numOfHearts;

    public Image[] heartArray;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update()
    {

        if(health > numOfHearts){
            health = numOfHearts;
        }




        for (int i = 0; i < heartArray.Length; i++)
        {
            if(i < health){
                heartArray[i].sprite = fullHeart;
            } else {
                heartArray[i].sprite = emptyHeart;
            }

            if(i < numOfHearts){
                heartArray[i].enabled = true;
            } else {
                heartArray[i].enabled = false;
            }
        }
    }



}
