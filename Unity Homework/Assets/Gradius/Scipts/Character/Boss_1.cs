using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : EnemyBase
{
    protected GameObject bossSpawn;
    private Animator bossAnim;

    protected float lastSpawnTime;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            bossAnim.SetBool("IsSpawn", true);
            lastSpawnTime = Time.time;
        }


        base.Update();
    }

    protected override void InitCharacter()
    {
        bossSpawn = Camera.main.transform.Find("bossSpawn/Anim").gameObject;
        bossAnim =bossSpawn.GetComponent<Animator>();

        base.InitCharacter();
    }

    protected override void Move()
    {
        if (Time.time - lastSpawnTime > 5)
        {
            Debug.Log("true");
            Move(Vector3.left);
        }
        else
        {
            Move(Vector3.zero);
        }
    }

    void Spawn()
    {
        invincible = true;
    }
}
