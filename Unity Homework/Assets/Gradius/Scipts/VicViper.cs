﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimaryWeaponType { Normal,Double=3,Laser=4}

public class VicViper : MonoBehaviour
{
    public float speed = 10;
    public float shootAngle = 30f;
    public float dubleAngle = 50f;
    private PrimaryWeaponType primaryWeapon;

    private bool isUpDouble = false;
    private bool isUpLaser = false;
    private int missileLevel = 0; 

    private const int NORMAL = 0;
    private const int LASER = 1;
    private const int MISSILE = 2;

    private int powerUp = 0;

    private Transform shotPosTrans;

    private GameObject[] bullets;

    private GameObject optionPrefab;
    //private GameObject[] options;

    private List<Vector3> trackList = new List<Vector3>();
    private float trackNodeDistance = 0.1f;

    private GameObject targetIconPrefab;
    private GameObject targetIcon;

    private Animator anim;

    private float lastFireTime = 0;
    private float fireInterval = 0.5f*0.5f;

    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Gradius/Prefabs/Bullets");
        targetIconPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/TargetIcon");
        optionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Option");
        shotPosTrans = transform.Find("ShotPos");

        //UpdataTrackList();

        //optionPrefab.transform.position = Vector3.MoveTowards(optionPrefab.transform.position, trackList[0], speed * Time.deltaTime);

        trackList.Add(transform.position);

        Vector3 v = MouseTarget();
        targetIconPrefab.transform.position = v;
        targetIcon = Instantiate(targetIconPrefab, targetIconPrefab.transform.position, Quaternion.identity);

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        MoveAnim();

        UpdataTrackList();

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

        if (Time.time - lastFireTime > fireInterval)
        {
            Shoot();
            lastFireTime = Time.time;
        }
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

        if (missileLevel > 0)
        {
            ShootMissile();
        }
    }

    void TryPowerUp()
    {
        
        switch (powerUp)
        {
            case 1:
                break;
            case 2:
                PowerUPMissle();
                break;
            case 3:
                if (isUpDouble == false)
                {
                    powerUp -= (int)(PrimaryWeaponType.Double);
                    isUpDouble = true;
                }
                ChangePrimaryWeapon(PrimaryWeaponType.Double);
                break;
            case 4:
                powerUp -= (int)(PrimaryWeaponType.Laser);
                if (isUpLaser == false)
                {
                    isUpLaser = true;
                }
                else
                {
                    GetComponent<BulletDamage>().laserCount++;
                }
                ChangePrimaryWeapon(PrimaryWeaponType.Laser);
                break;
            case 5:
                break;
            case 6:
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
    }

    void ShootDouble()
    {
        Vector3 direction = (MouseTarget() - shotPosTrans.position).normalized;

        Vector2 direntionMagnitude = MouseTarget() - shotPosTrans.position;
        float distance = direntionMagnitude.magnitude;

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
        GameObject Lower = Instantiate(bullets[NORMAL], shotPosTrans.position, Quaternion.Euler(0, 0, -dubleAngle/distance));

        bullet.transform.right = direction;
        bulletUpper.transform.right = (Quaternion.Euler(0,0, dubleAngle / distance) * direction).normalized;
        Lower.transform.right = (Quaternion.Euler(0,0, -dubleAngle / distance) * direction).normalized;

        bullet.GetComponent<BulletMove>().moveDirection = direction;
        bulletUpper.GetComponent<BulletMove>().moveDirection = (Quaternion.Euler(0, 0,dubleAngle / distance) * direction).normalized;
        Lower.GetComponent<BulletMove>().moveDirection = (Quaternion.Euler(0, 0, -dubleAngle / distance) * direction).normalized;

    }

    void ShootLaser()
    {
        GameObject bullet = Instantiate(bullets[LASER], shotPosTrans.position, Quaternion.identity);
        bullet.transform.right = Vector3.right;
        bullet.GetComponent<BulletMove>().moveDirection = Vector3.right;
    }

    void ShootMissile()
    {
        GameObject bullet = Instantiate(bullets[MISSILE], transform.position, Quaternion.Euler(0,0,-45));

        if(missileLevel == 2)
        {
            Instantiate(bullets[MISSILE], transform.position + transform.right*0.5f, Quaternion.Euler(0, 0, -45));
        }
    }

    void PowerUPMissle()
    {
        if (missileLevel < 2)
        {
            missileLevel++;
            powerUp -= MISSILE;
        }
    }

    void UpdataTrackList()
    {
        if (Vector3.SqrMagnitude(transform.position - trackList[trackList.Count - 1])>trackNodeDistance)
        {
            trackList.Add(transform.position);

            if(trackList.Count > 16)
            {
                trackList.RemoveAt(0);
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
        for(int i = 0; i<trackList.Count; i++)
        {
            Gizmos.DrawSphere(trackList[i], 0.1f);
        }

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
