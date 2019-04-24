using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy_5 : EnemyBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void InitWeapon()
    {
        base.InitWeapon();
        currentWeapon = new StraightWeapon(shotPosTrans, false);
    }

    protected override void Shoot()
    {
        base.Shoot();
    }
}
