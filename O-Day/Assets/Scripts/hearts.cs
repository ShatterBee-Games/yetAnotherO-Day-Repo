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

       //optimized -zoe

        health = Mathf.Min(health, numOfHearts);

        for (int i = 0; i < heartArray.Length; i++)
        {
            heartArray[i].sprite = i < health ? fullHeart : emptyHeart;
            heartArray[i].enabled = i < numOfHearts;
        }


    }



}
