using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalWeapon : MouseGuideWeapon
{
    protected override float fireInterval
    {
        get { return 0.2f; }
    }
    public NormalWeapon(Transform[] shotPosTrans) : base(0, shotPosTrans)
    {

    }

}
