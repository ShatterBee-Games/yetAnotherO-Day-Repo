using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBulletBehaviour : MonoBehaviour
{   
    
    float damage = 1;
    bool destroy = false;

    // Update is called once per frame
    void Update()
    {   
        
        //optimized -zoe
        destroy = transform.position.x > 30 || transform.position.x < -30;
        
        /*
        if (transform.position.x > 30 || transform.position.x < -30) {
            destroy = true;
        }
        */


        //MUST BE LAST THING YOU DO IN CODE
        if (destroy){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D C){
        GameObject other = C.gameObject;
        if (other.layer == 6){
            enemyController2D enemy = other.GetComponent<enemyController2D>();
            enemy.TakeDamage(damage);
        }
        destroy = true;
    }
}
