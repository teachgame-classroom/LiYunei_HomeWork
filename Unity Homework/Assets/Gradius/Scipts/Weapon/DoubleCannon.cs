using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCannon : MouseGuideWeapon
{
    protected override float FireInterval
    {
        get { return 0.2f; }
    }

    public DoubleCannon(Transform[] shotPosTrans,bool isPlayerWeapon) : base(0,shotPosTrans, isPlayerWeapon)
    {

    }

    protected override void Shoot(Transform shotPos)
    {
        base.Shoot(shotPos);

        for (int i = 0; i <shotPosTrans.Length; i++)
        {
            if(i <= optionLevel)
            {
                float distance = GetMouseDistance();
                SetDirection(shotPos);

                GameObject bulletUpper = bulletPool.Get(shotPosTrans[i].position, Quaternion.Euler(0, 0, 50 / distance) * shotPosTrans[i].rotation);
                bulletUpper.GetComponent<BulletMove>().moveDirection = bulletUpper.transform.right;

                //GameObject bulletUpper = GameObject.Instantiate(bulletPrefab, shotPosTrans[i].position, Quaternion.Euler(0, 0, 50 / distance) * shotPosTrans[i].rotation);
                GameObject bulletLower = bulletPool.Get(shotPosTrans[i].position, Quaternion.Euler(0, 0, -50 / distance) * shotPosTrans[i].rotation);
                bulletLower.GetComponent<BulletMove>().moveDirection = bulletLower.transform.right;

                //GameObject bulletLower = GameObject.Instantiate(bulletPrefab, shotPosTrans[i].position, Quaternion.Euler(0, 0, -50 / distance) * shotPosTrans[i].rotation);
            }
        }
    }

    protected float GetMouseDistance()
    {
        
        Vector3 direntionMagnitude = MouseTarget() - shotPosTrans[0].position;
        float distance = direntionMagnitude.magnitude;

        if (distance < 1) distance = 1;

        return distance;

    }
}
