using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyStomp : MonoBehaviour
{
    //float damage = 5;

    //vector2 is a 2d vector class that contains an x and y coordinate
    public Vector2 initPosition; 

    float maxDistance = 10f;
    bool destroy = false;


    // Update is called once per frame
    void Update()
    {   
        
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        float currentDist = Vector2.Distance(rigidBody.position, initPosition);
        if (currentDist > maxDistance) {
            destroy = true;
        }


        //MUST BE LAST THING YOU DO IN CODE
        if (destroy){
            Destroy(gameObject);
        }
    }

    //Deduct health code I'll do it later -Kayli   good luck  -zoe <333 
    // void OnCollisionEnter2D(Collision2D C){
    //     GameObject other = C.gameObject;
    //     if (other.layer == 6){
    //         enemyController2D enemy = other.GetComponent<enemyController2D>();
    //         enemy.TakeDamage(damage);
    //     }
    //     destroy = true;
    // }
}
