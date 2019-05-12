using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Character
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();
        if(hurtTags.Length == 0)
        {
            hurtTags = new string[] { "PlayerBullet" };
        }

        LoadDamgeEffect();
    }

    protected virtual void LoadDamgeEffect()
    {
        dieEffect = Resources.Load<GameObject>("Gradius/Prefabs/Effects/explosion_Red");
    }
}
