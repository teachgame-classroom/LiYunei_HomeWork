using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedWeapon : Weapon
{
    public Sprite[] turretSprites;
    protected SpriteRenderer spriteRenderer;
    public float searchRange = 5;
    protected string targetTag;
    protected float minAngle;
    protected float maxAngle;

    protected override float FireInterval
    {
        get { return 0.2f; }
    }

    protected override string fireClipName { get { return "Sound Effect (1)"; } }

    public GuidedWeapon(Transform[] shotPosTrans, string targetTag, float minAngle, float maxAngle,SpriteRenderer spriteRenderer, Sprite[] turretSprites,bool isPlayerWeapon) : base(0, shotPosTrans, isPlayerWeapon)
    {
        this.targetTag = targetTag;
        this.minAngle = minAngle;
        this.maxAngle = maxAngle;
        this.spriteRenderer = spriteRenderer;

        this.turretSprites = turretSprites;

    }

    protected override void Shoot(Transform shotPos)
    {
        Vector3 pos;

        if (FindTargetPosition(targetTag,out pos))
        {
            SetAimDirection(shotPos, pos);

            float angle = Vector3.SignedAngle(Vector3.right, shotPosTrans[0].right, Vector3.forward);
            bool plyaerNotInTurretAngle = (angle < minAngle || angle > maxAngle);

            if(!plyaerNotInTurretAngle)
            {
                base.Shoot(shotPos);
                SetSpriteByAimDirection(shotPosTrans[0].right);
            }
        }
        else
        {

        }
    }

    protected void SetAimDirection(Transform shotPos,Vector3 targetPosition)
    {
        Vector3 aimDirection ;
        aimDirection = (targetPosition - shotPos.position).normalized;
        shotPos.right = aimDirection;
    }

    protected virtual bool FindTargetPosition(string targetTag,out Vector3 targetPos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(shotPosTrans[0].position, searchRange);

        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].tag == targetTag)
            {
                targetPos = colliders[i].transform.position;
                return true;
            }
        }
        targetPos = Vector3.zero;
        return false;
    }

    void SetSpriteByAimDirection(Vector3 aimDirection)
    {
        if (turretSprites.Length == 0) return;

        float angleStep = (maxAngle - minAngle) / (turretSprites.Length - 1);

        float angle = Vector3.SignedAngle(Vector3.left, aimDirection, Vector3.back) + angleStep / 2;

        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        int spriteIdx = Mathf.Abs(Mathf.FloorToInt(angle / angleStep)) - 1;

        spriteIdx = Mathf.Clamp(spriteIdx, 0, turretSprites.Length - 1);

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = turretSprites[spriteIdx];
        }
    }
}
