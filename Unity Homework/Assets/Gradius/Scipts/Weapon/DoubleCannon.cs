using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCannon : MouseGuideWeapon
{
    protected override float fireInterval
    {
        get { return 0.2f; }
    }

    public DoubleCannon(Transform[] shotPosTrans) : base(0,shotPosTrans)
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

                GameObject bulletUpper = GameObject.Instantiate(bulletPrefab, shotPosTrans[i].position, Quaternion.Euler(0, 0, 50 / distance) * shotPosTrans[i].rotation);
                GameObject bulletLower = GameObject.Instantiate(bulletPrefab, shotPosTrans[i].position, Quaternion.Euler(0, 0, -50 / distance) * shotPosTrans[i].rotation);
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
