using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLaser : MonoBehaviour
{
    [SerializeField, Tooltip("warning sprite")]
    GameObject warnSprite;

    [SerializeField, Tooltip("laser sprite")]
    GameObject laserSprite;

   
    bool destroy = false;
    bool laserOn = false;
    float switchTime;

    [SerializeField, Tooltip("time needed to fire laser")]
    float switchTimeMax = 2.0f;

    [SerializeField, Tooltip("time laser is fired")]
    float fireTime = 0.35f;


    // Start is called before the first frame update
    void Start()
    {
        switchTime = switchTimeMax;
        SpriteRenderer warnRenderer = warnSprite.GetComponent<SpriteRenderer>();
        warnRenderer.color = new Color(1f,1f,1f,0.1f);
    }

    // Update is called once per frame
    void Update()
    {   
    
        switchTime -= Time.deltaTime;

        if(switchTime > 0f){
            float opacity = (switchTimeMax - switchTime)/switchTimeMax;
            SpriteRenderer warnRenderer = warnSprite.GetComponent<SpriteRenderer>();
            warnRenderer.color = new Color(1f,1f,1f,opacity);
        }

        if (switchTime < 0f){
            laserSprite.SetActive(true);
            warnSprite.SetActive(false);
            laserOn = true;
            GetComponent<Collider2D>().enabled = true;
            transform.Translate(0.0001f,0f,0f);
        }

        if (laserOn){
            fireTime -= Time.deltaTime;
            if (fireTime < 0f){
                destroy = true;
            }
        }


        //MUST BE LAST THING YOU DO IN CODE
        if (destroy){
            Destroy(gameObject);
        }
    }

    //Deduct health code I'll do it later -Kayli
    // void OnCollisionEnter2D(Collision2D C){
    //     GameObject other = C.gameObject;
    //     if (other.layer == 6){
    //         enemyController2D enemy = other.GetComponent<enemyController2D>();
    //         enemy.TakeDamage(damage);
    //     }
    //     destroy = true;
    // }
}
