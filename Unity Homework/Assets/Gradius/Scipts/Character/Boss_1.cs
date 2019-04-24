using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : EnemyBase
{
    protected GameObject bossSpawn;
    private Animator bossAnim;

    protected float lastSpawnTime;
    protected bool isSpawn;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isSpawn = true;
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
        if (!(Time.time - lastSpawnTime > 4) )
        {
            Move(Vector3.zero);
        }
        else
        {
            if (isSpawn)
            {
                transform.position += Vector3.left * baseSpeed * Time.deltaTime;

                float distanceToCamera = Camera.main.transform.position.x + transform.position.x;
                float distanceToExitSpawnState = Camera.main.orthographicSize * Camera.main.aspect * 0.5f;

                if (distanceToCamera < distanceToExitSpawnState)
                {
                    isSpawn = false;
                }
            }
        }
    }

    void Spawn()
    {
        invincible = true;
    }
}
