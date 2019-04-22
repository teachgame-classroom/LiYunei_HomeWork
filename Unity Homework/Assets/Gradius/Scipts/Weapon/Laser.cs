using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    protected override float fireInterval
    {
        get { return 0.3f; }
    }

    public Laser(Transform[] shotPosTrans) : base(1, shotPosTrans)
    {

    }

    protected override void Shoot()
    {
        for(int i =0; i<shotPosTrans.Length; i++)
        {
            if( i <= optionLevel)
            {
                shotPosTrans[i].rotation = Quaternion.identity;
                base.Shoot();
            }
        }
    }
}
