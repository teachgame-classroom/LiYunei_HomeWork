using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    protected GameObject bulletPrefab;
    protected Transform[] shotPosTrans;

    protected abstract float fireInterval { get; }
    protected float lastFireTime;

    protected int optionLevel;

    public Weapon(int bulletPrefabIndedx, Transform[] shotPosTrans)
    {
        Debug.Log("Weapon ShotPos Length:" + shotPosTrans.Length);

        string bulletPrefabName = "Gradius/Prefabs/Bullets/Bullet_" + bulletPrefabIndedx;
        bulletPrefab = Resources.Load<GameObject>(bulletPrefabName);
        this.shotPosTrans = shotPosTrans;
    }

    public Weapon(string bulletPrefabName, Transform[] shotPosTrans)
    {
        bulletPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Bullets/" + bulletPrefabName);
        this.shotPosTrans = shotPosTrans;

        Debug.Log("Length:" + shotPosTrans.Length);
    }

    public void TryShoot()
    {
        if (CanFire())
        {
            Shoot();
        }
    }

    public bool CanFire()
    {
        if (Time.time - lastFireTime > fireInterval)
        {
            lastFireTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void Shoot()
    {
        for (int i = 0; i < shotPosTrans.Length; i++)
        {
            if(i <= optionLevel)
            {
                Shoot(shotPosTrans[i]);
            }
        }
        
    }

    protected virtual void Shoot(Transform shotPos)
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, shotPos.position, shotPos.rotation);
    }


    public void PowerOpint()
    {
        if (optionLevel <= 2)
        {
            optionLevel++;
        }
    }
}
