using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimaryWeaponType { Normal,Missile,Double,Laser}

public class VicViper : MonoBehaviour
{
    public float speed = 10;
    public float fireInterval = 0.5f;
    public float fireLaser = 0.5f;
    public float fireMissIles = 1f;
    public float shootAngle = 30f;
    public float dubleAngle = 50f;
    public int optionMax = 3;
    private PrimaryWeaponType primaryWeapon;

    private float lastFireTime = 0;

    private bool isUpDouble = false;
    private bool isUpLaser = false;
    private int missileLevel = 0;
    private int optionLevel = 0;

    private float[] intervals;

    private const int NORMAL = 0;
    private const int LASER = 1;
    private const int MISSILE = 2;
    private const int OPTION = 5;

    private int powerUp = 0;

    private Transform shotPosTrans;

    private GameObject[] bullets;

    private GameObject optionPrefab;
    private GameObject[] options;

    private List<Vector3> trackList = new List<Vector3>();
    private float trackNodeDistance = 0.025f*0.025f;

    private GameObject targetIconPrefab;
    private GameObject targetIcon;

    private Animator anim;

    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Gradius/Prefabs/Bullets");
        targetIconPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/TargetIcon");
        optionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Option");
        shotPosTrans = transform.Find("ShotPos");

        trackList.Add(transform.position);

        Vector3 v = MouseTarget();
        targetIconPrefab.transform.position = v;
        targetIcon = Instantiate(targetIconPrefab, targetIconPrefab.transform.position, Quaternion.identity);

        intervals= new float[]{ fireInterval, fireMissIles, fireInterval, fireLaser };
        options = new GameObject[optionMax];
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        MoveAnim();

        if (optionLevel > 0)
        {
            UpdataTrackList();

            options[0].transform.position = Vector3.MoveTowards(options[0].transform.position, trackList[0], speed * Time.deltaTime);
            if(optionLevel > 1)
            {
                options[1].transform.position = Vector3.MoveTowards(options[1].transform.position, trackList[trackList.Count/2], speed * Time.deltaTime);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TryPowerUp();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && isUpDouble == true)
        {
            ChangePrimaryWeapon(PrimaryWeaponType.Double);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && isUpLaser == true)
        {
            ChangePrimaryWeapon(PrimaryWeaponType.Laser);
        }

        targetIcon.transform.position = MouseTarget();

        Shoot();
    }

    private void MoveAnim()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //Debug.Log(h + "," + v);

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

        transform.position += (Vector3.right * h + Vector3.up * v) * speed * Time.deltaTime;

        ClampPlayerPosition();
    }

    private void ClampPlayerPosition()
    {
        Vector3 camPos = Camera.main.transform.position;
        float left = camPos.x - Camera.main.orthographicSize * Camera.main.aspect+0.5f;
        float right = camPos.x + Camera.main.orthographicSize * Camera.main.aspect-0.5f;
        float top = camPos.y + Camera.main.orthographicSize-0.2f;
        float bottom = camPos.y - Camera.main.orthographicSize+0.2f;

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
        if(Time.time - lastFireTime > intervals[(int)primaryWeapon])
        {
            switch (primaryWeapon)
            {
                case PrimaryWeaponType.Normal:
                    ShootNormal();
                    break;
                case PrimaryWeaponType.Double:
                    ShootDouble();
                    break;
                case PrimaryWeaponType.Laser:
                    ShootLaser();
                    break;
            }
            if(Time.time -lastFireTime> fireMissIles)
            {
                if (missileLevel > 0)
                {
                    ShootMissile();
                }
            }
            lastFireTime = Time.time;
        }
    }

    void TryPowerUp()
    {
        
        switch (powerUp)
        {
            case 1:
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

    void ChangePrimaryWeapon(PrimaryWeaponType newWeaponType)
    {
        if(primaryWeapon!= newWeaponType)
        {
            primaryWeapon = newWeaponType;
        }
    }

    void ShootNormal()
    {
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
    }

    void ShootDouble()
    {
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
    }

    void ShootLaser()
    {
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

        if (missileLevel == 2)
        {
            Instantiate(bullets[MISSILE], transform.position + transform.right*0.5f, Quaternion.Euler(0, 0, -45));
        }
    }

    void PowerUpDouble()
    {
        if (isUpDouble == false)
        {
            powerUp -= (int)(PrimaryWeaponType.Double);
            isUpDouble = true;
        }
        ChangePrimaryWeapon(PrimaryWeaponType.Double);
    }

    void PowerUpLaser()
    {
        powerUp -= (int)(PrimaryWeaponType.Laser);
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
        if (missileLevel < 2)
        {
            missileLevel++;
            powerUp -= MISSILE;
        }
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
            optionLevel++;
            powerUp -= OPTION;
        }
    }

    void PowerUpBarrier()
    {
        transform.Find("Bullet_4_1").gameObject.SetActive(true);
        transform.Find("Bullet_4_2").gameObject.SetActive(true);
        powerUp = 0;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Is collider " + collision.gameObject.name);

        if(collision.tag == "PowerUp")
        {
            powerUp++;
            Destroy(collision.gameObject);
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
        GUILayout.Label(string.Format("Missle level:{0}", missileLevel));
        GUILayout.Label(string.Format("Track Node Count:" + trackList.Count));
        GUILayout.EndVertical();
    }
}
