using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalWeapon : MouseGuideWeapon
{
    protected override float FireInterval
    {
        get { return 0.2f; }
    }
    public NormalWeapon(Transform[] shotPosTrans, bool isPlayerWeapon) : base(0, shotPosTrans, isPlayerWeapon)
    {

    }

}
