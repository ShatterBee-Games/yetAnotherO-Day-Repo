using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    
    bool destroy = false;
    public Transform playerPosition;
    float waitTime = 2.0f;
    bool fired = false;

    void Update()
    {   
        waitTime -= Time.deltaTime;
        if (waitTime < 0f && !fired){
            fired = true;
            shootTowardsPlayer();
        }

        //optimized -zoe
        destroy = transform.position.x > 30 || transform.position.x < -30 || transform.position.y > 20 || transform.position.y < -20;
        
        /*
        if (transform.position.x > 30 || transform.position.x < -30 || transform.position.y > 20 || transform.position.y < -20) {
            destroy = true;
        }
        */


        //MUST BE LAST THING YOU DO IN CODE
        if (destroy){
            Destroy(gameObject);
        }
    }

    void shootTowardsPlayer(){
        Vector2 bulletPosition2d = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition2d = new Vector2(playerPosition.position.x, playerPosition.position.y);
        Vector2 direction = (playerPosition2d-bulletPosition2d).normalized;
        float xspeed = 10.0f;
        GetComponent<Rigidbody2D>().velocity = direction*xspeed ;

    }

     void OnCollisionEnter2D(Collision2D C){
        destroy = true;
     }

}
