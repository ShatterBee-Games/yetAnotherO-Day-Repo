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

    bool st;


    // Start is called before the first frame update
    void Start()
    {
        switchTime = switchTimeMax;
        Transform warnTransform = warnSprite.GetComponent<Transform>();
        warnTransform.localScale = new Vector3(0f,1f,1f);

    }

   // optimized -zoe
    void Update()
    {
        switchTime -= Time.deltaTime;

        st = switchTime > 0f ? true : false;

        if (st)
        {
            stg();
        }
        else
        {
            stl();
        }

        if (laserOn)
        {
            fireTime -= Time.deltaTime;
            destroy = fireTime < 0f ? true : false;
        }

        //MUST BE LAST THING YOU DO IN CODE
        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    //switch time greater
    void stg()
    {
        float opacity = (switchTimeMax - switchTime) / switchTimeMax;
        Transform warnTransform = warnSprite.GetComponent<Transform>();
        warnTransform.localScale = new Vector3(opacity,1f,1f);
    }

    //switch time less
    void stl()
    {
        laserSprite.SetActive(true);
        warnSprite.SetActive(false);
        laserOn = true;
        GetComponent<Collider2D>().enabled = true;
        transform.Translate(0.0001f, 0f, 0f);
        
    }
}







