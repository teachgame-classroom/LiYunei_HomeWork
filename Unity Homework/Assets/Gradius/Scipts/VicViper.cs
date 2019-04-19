using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimaryWeaponType { Normal,Missile,Double,Laser}


public class VicViper : MonoBehaviour
{
    /// <summary>
    /// from 0 to 1
    /// </summary>
    public float spawnPos = 0.8f;
    public int hp = 5;
    public int life = 10;
    public float speed = 8;
    public int optionMax = 3;
    private PrimaryWeaponType primaryWeapon;

    private bool isJustSpawned = true;
    private bool isInvincible = false;
    private float lastSpawnTime = 0;
    private float lastBlinkTime = 0;

    //private float lastFireTime = 0;
    //private float lastMissileTime = 0;

    private bool isUpDouble = false;
    private bool isUpLaser = false;
    private bool isUpBarrier = false;
    private int optionLevel = 0;

    private float[] intervals;

    private const int NORMAL = 0;
    private const int DOUBLE = 1;
    private const int LASER = 2;
    private const int MISSILE = 2;
    private const int OPTION = 5;

    public const int TOTAL_WEAPON = 4;
    private Weapon[] weapons = new Weapon[TOTAL_WEAPON];
    private Weapon currentWeapon;
    private int currentWeaponIdx;

    private Missile missile;


    private int powerUp = 0;

    private CameraMove cameraMove;

    private Transform[] shotPosTrans;
    //private Transform shotPosTrans;
    private Transform spawnTans;

    private GameObject[] bullets;

    private GameObject explosionPrefab;

    private GameObject optionPrefab;
    private GameObject[] options;

    private List<Vector3> trackList = new List<Vector3>();
    private float trackNodeDistance = 0.025f * 0.025f;

    private GameObject targetIconPrefab;
    private GameObject targetIcon;

    private Animator anim;
    private SpriteRenderer spriteRender;

    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Gradius/Prefabs/Bullets");
        targetIconPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/TargetIcon");
        explosionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/Explosion_Player");
        optionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Option");


        cameraMove = Camera.main.GetComponent<CameraMove>();
        spawnTans = Camera.main.transform.Find("PlayerSpawn");

        trackList.Add(transform.position);

        Vector3 v = MouseTarget();
        targetIconPrefab.transform.position = v;
        targetIcon = Instantiate(targetIconPrefab, targetIconPrefab.transform.position, Quaternion.identity);

        options = new GameObject[optionMax];
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();

        shotPosTrans = new Transform[optionMax + 1];
        shotPosTrans[0] = transform.Find("ShotPos");
        // shotPosTrans = transform.Find("ShotPos");

        InitWeapon();

        Spawn();
    }

    private void InitWeapon()
    {
        weapons[NORMAL] = new NormalWeapon(shotPosTrans);
        weapons[DOUBLE] = new DoubleCannon(shotPosTrans);
        weapons[LASER] = new Laser(shotPosTrans);

        missile = new Missile(shotPosTrans);

        currentWeaponIdx = 1;
        currentWeapon = weapons[currentWeaponIdx];
    }

    void Update()
    {
        if (isInvincible)
        {
            if (Time.time - lastBlinkTime > 0.1f)
            {
                spriteRender.enabled = !spriteRender.enabled;
                lastBlinkTime = Time.time;
            }
        }

        if (Time.time - lastSpawnTime > 3)
        {
            isInvincible = false;
            spriteRender.enabled = true;
        }

        if (isJustSpawned)
        {
            if (Time.time - lastSpawnTime > 1)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;

                float distanceToCamera = Camera.main.transform.position.x - transform.position.x;
                float distanceToExitSpawnState = Camera.main.orthographicSize * Camera.main.aspect * spawnPos;

                if (distanceToCamera < distanceToExitSpawnState)
                {
                    isJustSpawned = false;
                }
            }
        }
        else
        {
            MoveAnim();
            
            if (optionLevel > 0)
            {
                UpdataTrackList();

                options[0].transform.position = Vector3.MoveTowards(options[0].transform.position, trackList[0], speed * Time.deltaTime);
                if (optionLevel > 1)
                {
                    options[1].transform.position = Vector3.MoveTowards(options[1].transform.position, trackList[trackList.Count / 2], speed * Time.deltaTime);
                }

                
                for (int i = 0; i < optionMax; i++)
                {
                    shotPosTrans[i + 1] = options[i].transform;
                }
                
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                TryPowerUp();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (!isUpDouble)
                {
                    ChangePrimaryWeapon(NORMAL);
                    //ChangePrimaryWeapon(PrimaryWeaponType.Normal);
                }
                else
                {
                    ChangePrimaryWeapon(DOUBLE);
                    //ChangePrimaryWeapon(PrimaryWeaponType.Double);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && isUpLaser == true)
            {
                ChangePrimaryWeapon(LASER);
                //ChangePrimaryWeapon(PrimaryWeaponType.Laser);
            }

            targetIcon.transform.position = MouseTarget();

            Shoot();
        }
    }

    private void MoveAnim()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v > 0)
        {
            anim.SetInteger("Move", 1);
        }
        else if (v < 0)
        {
            anim.SetInteger("Move", 2);
        }
        else
        {
            anim.SetInteger("Move", 0);
        }

        transform.position += (Vector3.right * h + Vector3.up * v) * speed * Time.deltaTime + cameraMove.moveDirection * cameraMove.speed * Time.deltaTime;

        ClampPlayerPosition();
    }

    private void ClampPlayerPosition()
    {
        Vector3 camPos = Camera.main.transform.position;
        float left = camPos.x - Camera.main.orthographicSize * Camera.main.aspect + 0.5f;
        float right = camPos.x + Camera.main.orthographicSize * Camera.main.aspect - 0.5f;
        float top = camPos.y + Camera.main.orthographicSize - 0.2f;
        float bottom = camPos.y - Camera.main.orthographicSize + 0.2f;

        float clamp_x = Mathf.Clamp(transform.position.x, left, right);
        float clamp_y = Mathf.Clamp(transform.position.y, bottom, top);

        Vector3 clampPos = Vector3.right * clamp_x + Vector3.up * clamp_y;

        transform.position = clampPos;
    }

    private static Vector3 MouseTarget()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v = Vector3.Scale(v, Vector3.right + Vector3.up);
        return v;
    }

    float GetAngle(Vector3 direction)
    {
        float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
        return angle;
    }

    void Shoot()
    {
        currentWeapon.TryShoot();
        missile.TryShoot();
    }

    void TryPowerUp()
    {
        switch (powerUp)
        {
            case 1:
                PowerUpSpeed();
                break;
            case 2:
                PowerUpMissle();
                break;
            case 3:
                PowerUpDouble();
                break;
            case 4:
                PowerUpLaser();
                break;
            case 5:
                PowerUpOption();
                break;
            case 6:
                PowerUpBarrier();
                break;
        }
    }

    void ChangePrimaryWeapon(int newWeaponIdx)
    {
        newWeaponIdx = Mathf.Clamp(newWeaponIdx, 0, weapons.Length - 1);
        currentWeaponIdx = newWeaponIdx;

        currentWeapon = weapons[currentWeaponIdx];
    }

    void ChangePrimaryWeapon(PrimaryWeaponType newWeaponType)
    {
        if(primaryWeapon!= newWeaponType)
        {
            primaryWeapon = newWeaponType;
        }
    }

    void ShootNormal()
    {
        /*
        Vector3 direction = (MouseTarget() - shotPosTrans.position).normalized;

        if (GetAngle(direction) > shootAngle)
        {
            direction = Quaternion.Euler(0, 0, shootAngle) * Vector3.right;
        }
        if (GetAngle(direction) < -shootAngle)
        {
            direction = Quaternion.Euler(0, 0, -shootAngle) * Vector3.right;
        }

        GameObject bullet = Instantiate(bullets[NORMAL], shotPosTrans.position, Quaternion.identity);
        bullet.transform.right = direction;
        bullet.GetComponent<BulletMove>().moveDirection = direction;

        if (optionLevel >0)
        {
            for(int i =0; i<optionLevel; i++)
            {
                Vector3 optionsDirenction = (MouseTarget() - options[i].transform.position).normalized;

                GameObject optoinsBullet = Instantiate(bullets[NORMAL], options[i].transform.position, Quaternion.identity);
                optoinsBullet.transform.right = optionsDirenction;
                optoinsBullet.GetComponent<BulletMove>().moveDirection = optionsDirenction;
            }
        }
        */
    }

    void ShootDouble()
    {
        /*
        Vector3 direntionMagnitude = MouseTarget() - shotPosTrans.position;
        float distance = direntionMagnitude.magnitude;

        Vector3 direction =direntionMagnitude.normalized;

        if (distance < 1)
        {
            distance = 1;
        }

        if (GetAngle(direction) > shootAngle)
        {
            direction = Quaternion.Euler(0, 0, shootAngle) * Vector3.right;
        }

        if (GetAngle(direction) < -shootAngle)
        {
            direction = Quaternion.Euler(0, 0, -shootAngle) * Vector3.right;
        }

        GameObject bullet = Instantiate(bullets[NORMAL], shotPosTrans.position, Quaternion.identity);
        GameObject bulletUpper = Instantiate(bullets[NORMAL], shotPosTrans.position, Quaternion.Euler(0, 0, dubleAngle/distance));
        GameObject bulletLower = Instantiate(bullets[NORMAL], shotPosTrans.position, Quaternion.Euler(0, 0, -dubleAngle/distance));

        bullet.transform.right = direction;
        bulletUpper.transform.right = (Quaternion.Euler(0,0, dubleAngle / distance) * direction).normalized;
        bulletLower.transform.right = (Quaternion.Euler(0,0, -dubleAngle / distance) * direction).normalized;

        bullet.GetComponent<BulletMove>().moveDirection = direction;
        bulletUpper.GetComponent<BulletMove>().moveDirection = (Quaternion.Euler(0, 0,dubleAngle / distance) * direction).normalized;
        bulletLower.GetComponent<BulletMove>().moveDirection = (Quaternion.Euler(0, 0, -dubleAngle / distance) * direction).normalized;


        if(optionLevel>0)
        {
            for(int i =0; i < optionLevel; i++)
            {
                Vector3 optionsDirenction = (MouseTarget() - options[i].transform.position).normalized;

                GameObject optionsBullet = Instantiate(bullets[NORMAL], options[i].transform.position, Quaternion.identity);
                optionsBullet.transform.right = optionsDirenction;
                optionsBullet.GetComponent<BulletMove>().moveDirection = optionsDirenction;
            }
        }
        */
    }

    void ShootLaser()
    {
        /*
        GameObject bullet = Instantiate(bullets[LASER], shotPosTrans.position, Quaternion.identity);
        if(optionLevel>0)
        {
            for(int i =0; i < optionLevel; i++)
            {
                Instantiate(bullets[LASER], options[i].transform.position, Quaternion.identity);
            }
        }
        bullet.transform.right = Vector3.right;
        bullet.GetComponent<BulletMove>().moveDirection = Vector3.right;
        */
    }

    void ShootMissile()
    {
        GameObject bullet = Instantiate(bullets[MISSILE], transform.position, Quaternion.Euler(0,0,-45));

        if (optionLevel>0)
        {
            for(int i =0; i < optionLevel; i++)
            {
                Instantiate(bullets[MISSILE], options[i].transform.position, Quaternion.Euler(0, 0, -45));
            }
        }

        //if (missileLevel == 2)
        //{
        //    Instantiate(bullets[MISSILE], transform.position + transform.right*0.5f, Quaternion.Euler(0, 0, -45));
        //}
    }

    void PowerUpSpeed()
    {
        speed += 2;
        powerUp = 0;
    }

    void PowerUpDouble()
    {
        if (isUpDouble == false)
        {
            powerUp = 0;
            isUpDouble = true;
        }
        ChangePrimaryWeapon(0);
    }

    void PowerUpLaser()
    {
        powerUp = 0;
        if (isUpLaser == false)
        {
            isUpLaser = true;
        }
        else
        {
            bullets[1].GetComponent<BulletDamage>().laserCount++;
        }
        ChangePrimaryWeapon(PrimaryWeaponType.Laser);
    }

    void PowerUpMissle()
    {
        missile.LevelUp();
        powerUp -= MISSILE;
    }

    void CreatOption(int num)
    {
        options[num] = Instantiate(optionPrefab, transform.position, Quaternion.identity);
    }

    void PowerUpOption()
    {
        if(optionLevel < optionMax)
        {
            CreatOption(optionLevel);
            
            for(int i = 0; i < weapons.Length; i++)
            {
                weapons[i].PowerOpint();
            }
            optionLevel++;
            powerUp=0;
        }
    }

    void PowerUpBarrier()
    {
        if (isUpBarrier == false)
        {
            SetBarrierActive(true);
            powerUp = 0;
            isUpBarrier = true;
        }
    }

    private void SetBarrierActive(bool isActive)
    {
        transform.Find("Bullet_4_1").gameObject.SetActive(isActive);
        transform.Find("Bullet_4_2").gameObject.SetActive(isActive);
    }

    void UpdataTrackList()
    {
        if(optionLevel > 0)
        {
            if (Vector3.SqrMagnitude(transform.position - trackList[trackList.Count - 1]) > trackNodeDistance)
            {
                trackList.Add(transform.position);

                if (trackList.Count > optionLevel * 8) 
                {
                    trackList.RemoveAt(0);
                }
            }
        }
    }

    void Die()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        life--; 

        if(life > 0)
        {
            Spawn();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Spawn()
    {
        hp = 20;
        speed = 8;
        powerUp = 0;
        missile.Reset();
        optionLevel = 0;
        SetBarrierActive(false);
        primaryWeapon = NORMAL;
        transform.position = spawnTans.position;
        isJustSpawned = true;
        isInvincible = true;
        lastSpawnTime = Time.time;
    }

    void Hurt(int damage)
    {
        hp-= damage;
        if(hp <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        BulletDamage bullet = collision.GetComponent<BulletDamage>();

        if (collision.tag == "PowerUp")
        {
            powerUp++;
            if(powerUp > 6)
            {
                powerUp = 0;
                life++;
            }
            Destroy(collision.gameObject);
        }

        if (!isInvincible)
        {
            if (collision.tag == "Stage")
            {
                Hurt(1);
            }
            else
            {
                if (collision.tag == "EnemyBullet")
                {
                    Hurt(bullet.damage);
                    Destroy(collision.gameObject);
                }

            }
            if (enemy != null)
            {
                Die();
                if (enemy.canColliderDesity)
                {
                    enemy.Hurt(enemy.hp);
                }
                else
                {
                    enemy.Hurt(3);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;

        //for(int i = 0; i<trackList.Count; i++)
        //{
        //    Gizmos.DrawSphere(trackList[i], 0.1f);
        //}

        //Vector3 direction =(MouseTarget() - shotPosTrans.position);

        //float distance = direction.magnitude;
        //if(distance < 1)
        //{
        //    distance = 1;
        //}

        //Vector3 upperLimit = Quaternion.Euler(0, 0, 50/ distance) * direction.normalized * 10;
        //Vector3 lowerLimit = Quaternion.Euler(0, 0, -50 / distance) * direction.normalized * 10;

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(shotPosTrans.position, (shotPosTrans.position + upperLimit));
        //Gizmos.DrawLine(shotPosTrans.position, shotPosTrans.position + lowerLimit);

    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("PowerUp:{0}", powerUp));
        //GUILayout.Label(string.Format("Missle level:{0}", missileLevel));
        GUILayout.Label(string.Format("Hp:{0} , Life:{1}",hp,life));
        GUILayout.EndVertical();
    }
}
