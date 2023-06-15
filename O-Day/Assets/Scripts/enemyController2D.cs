using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController2D : MonoBehaviour
{
    public float health = 200f;

    [SerializeField, Tooltip("Prefab for stomp")]
    GameObject stompPrefab;

    [SerializeField, Tooltip("Position for stomp")]
    List<Transform> listStompPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")){
            SpawnStomp();
        }


        
    }

    public void TakeDamage(float damage){
        health -= damage;
        Debug.Log(health);
    }


    //Its going to put a ArgumentOutOfRangeException for now because the legs have the same script tied to them
    //doesn't intefere with anything for now, but could be fixed with making the legs children of the enemy body
    //ignore it for now, when the animations and hitboxes come in a fix should make itself apparent
    //-Kayli
    public void SpawnStomp(){
        int stompPositionIndex = Random.Range(0,listStompPosition.Count);
        Transform stompPosition = listStompPosition[stompPositionIndex];
        GameObject stompGameObject = Instantiate(stompPrefab, stompPosition.position, Quaternion.identity);
        Rigidbody2D stomp = stompGameObject.GetComponent<Rigidbody2D>();
        float xspeed;
        xspeed = 5.0f;
        stomp.velocity = new Vector2(xspeed, 0.0f);
        enemyStomp stompcode = stompGameObject.GetComponent<enemyStomp>();
        stompcode.initPosition = stomp.position; 
        //spawning second one
        stompGameObject = Instantiate(stompPrefab, stompPosition.position, Quaternion.identity);
        stomp = stompGameObject.GetComponent<Rigidbody2D>();
        stomp.velocity = new Vector2(-xspeed, 0.0f);
        stompcode = stompGameObject.GetComponent<enemyStomp>();
        stompcode.initPosition = stomp.position; 
    }

}
