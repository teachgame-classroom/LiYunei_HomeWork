using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : EnemyBase
{
    protected Transform[] shotPosTrans;
    protected Weapon currentWeapon;

    protected override void Start()
    {
        base.Start();
        InitWeapon();
    }

    protected override void Update()
    {
        base.Update();
        Shoot();
    }

    protected virtual void InitWeapon()
    {
        ShotPosMarker[] markers = GetComponentsInChildren<ShotPosMarker>();

        if (markers.Length > 0)
        {
            shotPosTrans = new Transform[markers.Length];

            for(int i = 0; i <markers.Length; i++)
            {
                shotPosTrans[i] = markers[i].transform;
            }
        }
    }

    protected virtual void Shoot()
    {
        if(currentWeapon != null)
        {
            currentWeapon.TryShoot();
        }
    }
}
