using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Character
{
    public int score = 100;

    public bool waitForPlayer = true;

    protected override string deathClipName { get { return "Sound Effect (7)"; } }

    

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

    protected override void Die()
    {
        GameController.instance.AddScore(score);
        base.Die();
    }

    protected virtual void LoadDamgeEffect()
    {
        dieEffect = Resources.Load<GameObject>("Gradius/Prefabs/Effects/explosion_Red");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            base.OnTriggerEnter2D(collision);
        }
    }
}
