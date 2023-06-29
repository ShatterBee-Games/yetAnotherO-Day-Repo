using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //allows script to manage scene


[System.Serializable]
public class spawnLocation{
    public Transform position;
    public Vector3 step;

}

[System.Serializable]
public class modeAttacks{
    public List<Transform> listStompPosition;
    public List<spawnLocation> listProjectileLocation;

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

    //this is where we do the left right center switching
    [SerializeField, Tooltip("list of attack modes")]
    List<modeAttacks> attackModes;
    
    //hey go away don't touch the constants (I have a knife) -kayli
    const int MODE_LEFT = 0;
    const int MODE_RIGHT = 1;
    const int MODE_CENTER = 2;
    //pspspsps go away from those constants pspsps

    int mode;

    [SerializeField, Tooltip("health transition value right")]
    float rightSwap = 170f;

    [SerializeField, Tooltip("health transition value center")]
    float centerSwap = 140f;

    [SerializeField, Tooltip("health transition value phase2")]
    float p2Swap = 100f;

    [SerializeField, Tooltip("list of boss location sprites")]
    List<GameObject> bossSprites;
    

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
        mode = MODE_LEFT;
    }

    // Update is called once per frame
    void Update()
    {   

        if (mode == MODE_LEFT && health <= rightSwap){
            mode = MODE_RIGHT;
            bossSprites[MODE_LEFT].SetActive(false);
            bossSprites[MODE_RIGHT].SetActive(true);
        } 

        if (mode == MODE_RIGHT && health <= centerSwap){
            mode = MODE_CENTER;
            bossSprites[MODE_RIGHT].SetActive(false);
            bossSprites[MODE_CENTER].SetActive(true);
        } 

        if (mode == MODE_CENTER && health <= p2Swap){
             SceneManager.LoadScene(4);
        } 


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



    public void SpawnStomp(){
        List<Transform> listStompPosition = attackModes[mode].listStompPosition;
        
        for (int stompPositionIndex = 0; stompPositionIndex < listStompPosition.Count; stompPositionIndex++){
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

        attackTimer += 2.0f;
    }

    public void startSpawn(){
        List<spawnLocation> listProjectileLocation = attackModes[mode].listProjectileLocation;

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
