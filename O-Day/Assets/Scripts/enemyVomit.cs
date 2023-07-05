using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyVomit : MonoBehaviour
{
    public bool destroy = false;

    void Update()
    {   
       

        
        
        //dont optimize before checking if it breaks on collision destroy -kayli
        // noted wont touch then -zoe
        if (transform.position.x > 30 || transform.position.x < -30 || transform.position.y > 20 || transform.position.y < -20) {
            destroy = true;
        }
        


        //MUST BE LAST THING YOU DO IN CODE
        if (destroy){
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D C){
        GameObject other = C.gameObject;
        if (other.layer == 7){
            destroy = true;
        }
    }
}
