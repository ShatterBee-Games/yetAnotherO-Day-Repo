using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //allows script to manage scene
using UnityEngine.UI;

[System.Serializable]
public class spawnLocation
{
    public Transform position;
    public Vector3 step;
}

[System.Serializable]
public class modeAttacks
{
    public List<Transform> listStompPosition;
    public List<Transform> listVomitPosition;
    public List<spawnLocation> listProjectileLocation;
}

public class enemyController2D : MonoBehaviour
{
    Animator anim;

    [SerializeField, Tooltip("Prefab for stomp")]
    GameObject stompPrefab;

    [SerializeField, Tooltip("Prefab for projectile")]
    GameObject projectilePrefab;

    [SerializeField, Tooltip("Prefab for laser")]
    GameObject laserPrefab;

    [SerializeField, Tooltip("Prefab for vomit")]
    GameObject vomitPrefab;

    CameraShaker cameraShaker;

    [SerializeField] GameObject Shake;

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
    float rightSwap = 140f;

    [SerializeField, Tooltip("health transition value center")]
    float centerSwap = 80f;

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
    float bulletTimerMax = 0.5f;

    float vomitAttackTimer;

    float vomitBulletTimer;

    [SerializeField, Tooltip("vomit bullet timer max")]
    float vomitBulletTimerMax = 0.1f;

    [SerializeField, Tooltip("vomit timer max")]
    float vomitAttackTimerMax = 2f;

    float attackTimer;

    [SerializeField, Tooltip("attack timer maximum delay")]
    float attackTimerMax = 3.0f;

    [SerializeField, Tooltip("attack timer minimum delay")]
    float attackTimerMin = 1.0f;

    [SerializeField, Tooltip("healthbar image")]
    Image healthjuice;

    [SerializeField, Tooltip("health max")]
    float healthMax = 200f;

    [SerializeField, Tooltip("health current")]
    float health;

    [SerializeField, Tooltip("music controller")]
    Animator musicController;

    [SerializeField, Tooltip("left safe time")]
    float leftSafe = 0f;

    [SerializeField, Tooltip("right safe time")]
    float rightSafe = 1.0f;

    [SerializeField, Tooltip("center safe time")]
    float centerSafe = 2.0f;

    float currentSafeTime = 3.0f;

    public AudioClip impact;
    public AudioSource audio;


    void Awake()
    {
        cameraShaker = Shake.GetComponent<CameraShaker>();
        audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = Random.Range(attackTimerMin, attackTimerMax);
        mode = MODE_LEFT;
        health = healthMax;
        anim = GetComponent<Animator>();
        currentSafeTime = leftSafe;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == MODE_LEFT && health <= rightSwap)
        {
            anim.SetTrigger("switchRight");
            attackTimerMax = 2f;
            attackTimerMin = 0.5f;
            currentSafeTime = rightSafe;
            // mode = MODE_RIGHT;
            // bossSprites[MODE_LEFT].SetActive(false);
            // bossSprites[MODE_RIGHT].SetActive(true);
        }

        if (mode == MODE_RIGHT && health <= centerSwap)
        {
            anim.SetTrigger("switchCenter");
            musicController.SetTrigger("switchCenter");
            attackTimerMax = 1f;
            attackTimerMin = 0f;
            currentSafeTime = centerSafe;
            // if(musicController.clip != centerMusic){
            //     musicController.clip=centerMusic;
            //     musicController.Play();
            // }
            // mode = MODE_CENTER;
            // bossSprites[MODE_RIGHT].SetActive(false);
            // bossSprites[MODE_CENTER].SetActive(true);
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer < 0 && health >0)
        {
            attackTimer = Random.Range(attackTimerMin, attackTimerMax);
            float chance = Random.Range(0.0f, 3.0f);
            if (chance > 2f)
            {
                anim.SetTrigger("startStomp");
                attackTimer += 1.2f;
                // SpawnStomp();
            }
            else if (chance > 1f)
            {
                anim.SetTrigger("startLaser");
                attackTimer += 1.2f;
                //spawnLaser();
            }
            else
            {
                if (mode == MODE_CENTER)
                {
                    anim.SetTrigger("startVomit");
                    attackTimer += 2.4f;
                    //StartVomit();
                }
                else
                {
                    anim.SetTrigger("startBullet");
                    attackTimer += 1.25f;
                    //startSpawn();
                }
            }
        }

        if (bulletsToSpawn > 0)
        {
            bulletTimer -= Time.deltaTime;
            if (bulletTimer <= 0)
            {
                SpawnBullets(bulletsToSpawnMax - bulletsToSpawn);
                bulletsToSpawn--;
                bulletTimer = bulletTimerMax;
            }
        }

        if (vomitAttackTimer > 0)
        {
            vomitAttackTimer -= Time.deltaTime;
            vomitBulletTimer -= Time.deltaTime;
            if (vomitBulletTimer <= 0)
            {
                SpawnVomit();
                vomitBulletTimer = vomitBulletTimerMax;
            }
        }

        if (health<=0){
            anim.SetTrigger("healthZero");
            Debug.Log("dead");
        }
    }

    public void SwitchModesRight()
    {
        mode = MODE_RIGHT;
        bossSprites[MODE_LEFT].SetActive(false);
        bossSprites[MODE_RIGHT].SetActive(true);
    }

    public void SwitchModesCenter()
    {
        mode = MODE_CENTER;
        bossSprites[MODE_RIGHT].SetActive(false);
        bossSprites[MODE_CENTER].SetActive(true);
    }

    public void TakeDamage(float damage)
    {   
        audio.PlayOneShot(impact, 1F);
        health -= damage;
        healthjuice.fillAmount = health / healthMax;
        Debug.Log(health);
        
    }

    public void SpawnStomp()
    {
        List<Transform> listStompPosition = attackModes[mode].listStompPosition;

        for (
            int stompPositionIndex = 0;
            stompPositionIndex < listStompPosition.Count;
            stompPositionIndex++
        )
        {
            Transform stompPosition = listStompPosition[stompPositionIndex];
            GameObject stompGameObject = Instantiate(
                stompPrefab,
                stompPosition.position,
                Quaternion.identity
            );
            Rigidbody2D stomp = stompGameObject.GetComponent<Rigidbody2D>();
            float xspeed;
            xspeed = 10.0f;
            stomp.velocity = new Vector2(xspeed, 0.0f);
            enemyStomp stompcode = stompGameObject.GetComponent<enemyStomp>();
            stompcode.initPosition = stomp.position;
            //spawning second one
            stompGameObject = Instantiate(stompPrefab, stompPosition.position, Quaternion.Euler(0, 180, 0));
            stomp = stompGameObject.GetComponent<Rigidbody2D>();
            stomp.velocity = new Vector2(-xspeed, 0.0f);
            stompcode = stompGameObject.GetComponent<enemyStomp>();
            stompcode.initPosition = stomp.position;
            cameraShaker.BasicShake(0.8f, 0.2f);
        }

        attackTimer += (2.0f - currentSafeTime) ;
    }

    public void startSpawn()
    {
        List<spawnLocation> listProjectileLocation = attackModes[mode].listProjectileLocation;

        projectileLocationIndex = Random.Range(0, listProjectileLocation.Count);
        spawnLocation projectileLocation = listProjectileLocation[projectileLocationIndex];
        projectilePosition = projectileLocation.position;
        step = projectileLocation.step;
        bulletsToSpawn = bulletsToSpawnMax;
        bulletTimer = bulletTimerMax;
        attackTimer += (3.0f - currentSafeTime);
    }

    public void SpawnBullets(int i)
    {
        GameObject projectileGameObject = Instantiate(
            projectilePrefab,
            projectilePosition.position + step * i,
            Quaternion.identity
        );
        enemyBullet projectilecode = projectileGameObject.GetComponent<enemyBullet>();
        projectilecode.playerPosition = playerPosition;
    }

    public void spawnLaser()
    {
        GameObject laserGameObject = Instantiate(
            laserPrefab,
            laserTopLocation.position,
            Quaternion.identity
        );
        Rigidbody2D laser = laserGameObject.GetComponent<Rigidbody2D>();
        //spawn second one
        laserGameObject = Instantiate(
            laserPrefab,
            laserBottomLocation.position,
            Quaternion.identity
        );
        laser = laserGameObject.GetComponent<Rigidbody2D>();
        attackTimer += (3.0f - currentSafeTime);
    }

    public void StartVomit()
    {
        vomitAttackTimer = vomitAttackTimerMax;
        vomitBulletTimer = vomitBulletTimerMax;
        SpawnVomit();
        cameraShaker.SmoothShake(0.3f, vomitAttackTimerMax, 0.1f,0.1f);
        attackTimer += (vomitAttackTimerMax - currentSafeTime);
    }

    public void SpawnVomit()
    {   
        List<Transform> listVomitPosition = attackModes[mode].listVomitPosition;
        int vomitPositionIndex = Random.Range(0, listVomitPosition.Count);
        Transform vomitPosition = listVomitPosition[vomitPositionIndex];
        GameObject vomitGameObject = Instantiate(
            vomitPrefab,
            vomitPosition.position,
            Quaternion.identity
        );
        Rigidbody2D vomit = vomitGameObject.GetComponent<Rigidbody2D>();
        float yspeed;
        yspeed = -10.0f;
        vomit.velocity = new Vector2(0.0f, yspeed);
    }

    public void EntryShake(){
        cameraShaker.BasicShake(1f, 0.25f);
    }

    public void VictoryScreen(){
        SceneManager.LoadScene(5); //YOURE A WINNER DING DING DING
    }
}
