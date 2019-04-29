using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    protected override float FireInterval
    {
        get { return 0.3f; }
    }

    protected override string fireClipName { get { return "Sound Effect (10)"; } }

    public Laser(Transform[] shotPosTrans,bool isPlayerWeapon) : base(1, shotPosTrans, isPlayerWeapon)
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
