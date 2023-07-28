using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textScroll : MonoBehaviour
{

    [Header("Text Settings")]

    [SerializeField] [TextArea] private string[] itemInfo;

    [SerializeField] private float textSpeed = 0.01f;

    [Header("UI Element")]
    
    [SerializeField] private TextMeshProUGUI itemInfoText;

    private int currentDisplayingText = 0;

    public void Awake(){
        ActivateText();
    }

    public void ActivateText(){
        StartCoroutine(AnimateText());

    }

    IEnumerator AnimateText(){
        for(int i = 0; i < itemInfo[currentDisplayingText].Length+1; i++ ){
            itemInfoText.text=itemInfo[currentDisplayingText].Substring(0,i);
            yield return new WaitForSeconds(textSpeed);

        }
    }

}
