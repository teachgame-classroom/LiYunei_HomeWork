using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy_3 : EnemyBase
{
    public Sprite[] turretSprites;
    public float turretMinAngle;
    public float turretMaxAngle;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        Shoot();
    }

    protected override void InitWeapon()
    {
        base.InitWeapon();
        currentWeapon = new GuidedWeapon(shotPosTrans,"Player" , turretMinAngle, turretMaxAngle, spriteRenderer, turretSprites,false);
    }

}
