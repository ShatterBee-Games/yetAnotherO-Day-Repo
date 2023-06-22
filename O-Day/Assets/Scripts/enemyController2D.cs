using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class spawnLocation{
    public Transform position;
    public Vector3 step;

}

public class enemyController2D : MonoBehaviour
{
    public float health = 200f;

    [SerializeField, Tooltip("Prefab for stomp")]
    GameObject stompPrefab;

    [SerializeField, Tooltip("Prefab for projectile")]
    GameObject projectilePrefab;

    [SerializeField, Tooltip("Prefab for laser")]
    GameObject laserPrefab;

    [SerializeField, Tooltip("Position for stomp")]
    List<Transform> listStompPosition;

    [SerializeField, Tooltip("Position for projectile")]
    List<spawnLocation> listProjectileLocation;

    [SerializeField, Tooltip("Position for laser top")]
    Transform laserTopLocation;

    [SerializeField, Tooltip("Position for laser bottom")]
    Transform laserBottomLocation;

    [SerializeField, Tooltip("Player Position")]
    Transform playerPosition;

    Vector3 step;
    int projectileLocationIndex;
    Transform projectilePosition;  
    int bulletsToSpawn;

    [SerializeField, Tooltip("bullet spawn num")]
    int bulletsToSpawnMax = 3;

    float bulletTimer;

    [SerializeField, Tooltip("bullet spawn delay")]
    float bulletTimerMax= 0.5f;

    float attackTimer;

    [SerializeField, Tooltip("attack timer maximum delay")]
    float attackTimerMax= 6.0f;

    [SerializeField, Tooltip("attack timer minimum delay")]
    float attackTimerMin= 1.6f;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = Random.Range(attackTimerMin, attackTimerMax);
    }

    // Update is called once per frame
    void Update()
    {   
        attackTimer -= Time.deltaTime;

        if ( attackTimer <0 ){
            attackTimer = Random.Range(attackTimerMin, attackTimerMax);
            float chance = Random.Range(0.0f, 3.0f);
            if(chance > 2f){
                SpawnStomp();
            }
            else if (chance > 1f){
                spawnLaser();
            }
            else {
                startSpawn();
            }
        }

        if (bulletsToSpawn > 0){
            bulletTimer -= Time.deltaTime;
            if(bulletTimer <= 0){
                SpawnBullets(bulletsToSpawnMax - bulletsToSpawn);
                bulletsToSpawn--;
                bulletTimer = bulletTimerMax;
            }
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
        attackTimer += 2.0f;
    }

    public void startSpawn(){
        projectileLocationIndex = Random.Range(0,listProjectileLocation.Count);
        spawnLocation projectileLocation = listProjectileLocation[projectileLocationIndex];
        projectilePosition = projectileLocation.position;
        step = projectileLocation.step;
        bulletsToSpawn = bulletsToSpawnMax;
        bulletTimer = bulletTimerMax;
        attackTimer += 3.0f;
    }



    public void SpawnBullets(int i){

        GameObject projectileGameObject = Instantiate(projectilePrefab, projectilePosition.position+step*i, Quaternion.identity);
        enemyBullet projectilecode = projectileGameObject.GetComponent<enemyBullet>();
        projectilecode.playerPosition = playerPosition;

        
    }

    public void spawnLaser(){
        
        GameObject laserGameObject = Instantiate(laserPrefab, laserTopLocation.position, Quaternion.identity);
        Rigidbody2D laser = laserGameObject.GetComponent<Rigidbody2D>();
        //spawn second one
        laserGameObject = Instantiate(laserPrefab, laserBottomLocation.position, Quaternion.identity);
        laser = laserGameObject.GetComponent<Rigidbody2D>();
        attackTimer += 3.0f;

    }

}
