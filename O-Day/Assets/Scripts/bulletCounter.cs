using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletCounter : MonoBehaviour
{
    public int bullets;
    public Image[] bulletArray;
    public Image reloadBar;
    public float progress;


    void Update()
    {

        for (int i = 0; i < bulletArray.Length; i++)
        {
            bulletArray[i].enabled = i < bullets;
        }

        reloadBar.fillAmount = 1 - progress;
    }



}
