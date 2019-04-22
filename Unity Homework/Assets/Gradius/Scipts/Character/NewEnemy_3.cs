using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy_3 : EnemyShoot
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
        SetSpriteByAimDirection(shotPosTrans[0].right);
    }

    protected override void InitWeapon()
    {
        base.InitWeapon();
        currentWeapon = new GuidedWeapon(shotPosTrans,"Player" );
    }

    void SetSpriteByAimDirection(Vector3 aimDirection)
    {
        if (turretSprites.Length == 0) return;

        float angleStep = (turretMaxAngle - turretMinAngle) / (turretSprites.Length - 1);

        float angle = Vector3.SignedAngle(Vector3.left, aimDirection, Vector3.back) + angleStep / 2;

        angle = Mathf.Clamp(angle, turretMinAngle, turretMaxAngle);

        int spriteIdx = Mathf.Abs(Mathf.FloorToInt(angle / angleStep)) - 1;

        spriteIdx = Mathf.Clamp(spriteIdx, 0, turretSprites.Length - 1);

        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = turretSprites[spriteIdx];
        }

    }

}
