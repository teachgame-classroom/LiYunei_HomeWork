using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern { Straight, ZigSaw, Sine}

public class Enemy : MonoBehaviour
{

    public MovePattern movePattern;
    public float speed = 5;
    public float changeDiectionPeriod = 1f;
    public float sinAmp = 1; 

    private float lastChangDirectionTime = 0;
    private Vector3 velocity_h = Vector3.left ;
    private Vector3 velocity_v = Vector3.up;

    public int hp = 1;
    private GameObject explosionPrefab;

    /// <summary>
    /// 该敌人所属小队，敌人生成时由小队脚本指定
    /// </summary>
    public SquadonManager squadonManager;

    // Start is called before the first frame update
    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/Explosion_Red");


    }

    // Update is called once per frame
    void Update()
    {
        if(squadonManager == null)
        {
            switch (movePattern)
            {
                case MovePattern.Straight:
                    StraightMove();
                    break;
                case MovePattern.ZigSaw:
                    ZigSawMove();
                    break;
                case MovePattern.Sine:
                    SineMove();
                    break;
            }
        }
    }
    /// <summary>
    /// 直线飞行
    /// </summary>
    void StraightMove()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }
    /// <summary>
    /// 折线飞行
    /// </summary>
    void ZigSawMove()
    {
        if(Time.time - lastChangDirectionTime > changeDiectionPeriod)
        {
            velocity_v = -velocity_v;
            lastChangDirectionTime = Time.time;
        }

        Vector3 velocity = (velocity_h + velocity_v).normalized;

        transform.right = -velocity;

        transform.Translate(velocity *speed* Time.deltaTime, Space.World);
    }
    /// <summary>
    /// 正弦飞行
    /// </summary>
    void SineMove()
    {
        velocity_v = Vector3.up * Mathf.Sin(Mathf.PI * 2 * Time.time / changeDiectionPeriod) * sinAmp;

        Vector3 velocity =velocity_h *speed + velocity_v;

        transform.Translate(velocity * speed * Time.deltaTime, Space.World);
    }

    public void Hurt(int damage)
    {
        hp -= damage;

        if(hp<= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if(squadonManager != null)
        {
            squadonManager.OnMenberDestroy(transform.position);
        }

        Instantiate(explosionPrefab, transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
