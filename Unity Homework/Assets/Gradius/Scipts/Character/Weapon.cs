using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public bool isPlayerWeapon;

    protected GameObject bulletPrefab;
    protected Transform[] shotPosTrans;
    protected List<Transform> shotPosTransList;

    protected abstract float FireInterval { get; }
    protected float lastFireTime;

    protected int optionLevel;

    public Weapon(int bulletPrefabIndedx, Transform[] shotPosTrans,bool isPlayerWeapon)
    {
        string bulletPrefabName = "Gradius/Prefabs/Bullets/Bullet_" + bulletPrefabIndedx;
        bulletPrefab = Resources.Load<GameObject>(bulletPrefabName);
        this.shotPosTrans = shotPosTrans;
        this.isPlayerWeapon = isPlayerWeapon;
    }

    public Weapon(string bulletPrefabName, Transform[] shotPosTrans, bool isPlayerWeapon)
    {
        bulletPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Bullets/" + bulletPrefabName);
        this.shotPosTrans = shotPosTrans;
        this.isPlayerWeapon = isPlayerWeapon;
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
        if (Time.time - lastFireTime > FireInterval)
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
        for (int i = 0; i <  shotPosTrans.Length; i++)
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

        if (isPlayerWeapon)
        {
            bullet.tag = "PlayerBullet";
        }
        else
        {
            bullet.tag = "EnemyBullet";
        }
    }


    public void PowerOpint()
    {
        optionLevel++;
    }
}
