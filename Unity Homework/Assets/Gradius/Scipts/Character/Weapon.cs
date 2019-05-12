using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public bool isPlayerWeapon;

    protected GameObject bulletPrefab;
    protected Transform[] shotPosTrans;

    protected ObjectPool bulletPool;

    protected abstract float FireInterval { get; }
    protected float lastFireTime;

    protected int optionLevel;

    public Weapon(int bulletPrefabIndedx, Transform[] shotPosTrans,bool isPlayerWeapon)
    {
        string bulletPrefabPath = "Gradius/Prefabs/Bullets/Bullet_" + bulletPrefabIndedx;
        Init(shotPosTrans, isPlayerWeapon, bulletPrefabPath);
    }


    public Weapon(string bulletPrefabName, Transform[] shotPosTrans, bool isPlayerWeapon)
    {
        string bulletPrefabPath = "Gradius/Prefabs/Bullets/" + bulletPrefabName;
        Init(shotPosTrans, isPlayerWeapon, bulletPrefabName);
    }

    private void Init(Transform[] shotPosTrans, bool isPlayerWeapon, string bulletPrefabName)
    {
        bulletPrefab = Resources.Load<GameObject>(bulletPrefabName);
        this.shotPosTrans = shotPosTrans;
        this.isPlayerWeapon = isPlayerWeapon;

        bulletPool = new ObjectPool(bulletPrefab, 50);
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
        GameObject instance = bulletPool.Get(shotPos.position, shotPos.rotation);

        instance.GetComponent<BulletMove>().moveDirection = instance.transform.right;


        if (isPlayerWeapon)
        {
            instance.tag = "PlayerBullet";
        }
        else
        {
            instance.tag = "EnemyBullet";
        }
    }


    public void PowerOpint()
    {
        optionLevel++;
    }
}
