using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern { Static, Straight, ZigSaw, Sine}


public class Enemy : MonoBehaviour
{
    public MovePattern movePattern;

    private  float straightMoveDistance = 1;

    private float straightMoveTotalDistance;

    public float speed = 5;
    public float changeDiectionPeriod = 1f;
    public float sinAmp = 1;

    private bool targetInRange;

    private float lastChangDirectionTime = 0;
    private Vector3 velocity_h = Vector3.left ;
    private Vector3 velocity_v = Vector3.up;

    public int hp = 1;
    private GameObject explosionPrefab;

    /// <summary>
    /// 该敌人所属小队，敌人生成时由小队脚本指定
    /// </summary>
    public SquadonManager squadonManager;

    private GameObject player;

    public Sprite[] turrentSprites;
    private SpriteRenderer spriteRenderer;
    private Transform shotPos;

    private Animator anim;

    public GameObject bulletPerfab;
    public float fireInterval;
    private float lastFireTime;

    // Start is called before the first frame update
    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/Explosion_Red");

        player = GameObject.Find("Vic Viper");

        shotPos = transform.Find("ShotPos");

        if(shotPos == null)
        {
            shotPos = transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(squadonManager == null)
        {
            switch (movePattern)
            {
                case MovePattern.Static:
                    StaticMove();
                    break;
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
    /// 原地
    /// </summary>
    void StaticMove()
    {
        SetSpriteByAimDirection(GetAimDirection(player.transform.position));

        if (bulletPerfab != null)
        {
            shoot();
        }

        ClampPosition();
    }
    /// <summary>
    /// 直线巡逻
    /// </summary>
    void StraightMove()
    {
        Vector3 direction = -transform.right;
        Vector3 aimDirection = GetAimDirection(player.transform.position);//获取玩家和自己的位置关系
        float aimAngle = GetAngle(aimDirection);//获取玩家在x轴的度数

        bool facingLeft = transform.right.x > 0;
        bool targetOnLeftSide = aimDirection.x < 0;//玩家是否在左边

        //if(facingLeft)
        //{
        //    targetOnLeftSide = aimAngle > 85;
        //}
        //else
        //{
        //    targetOnLeftSide = aimAngle > 95;
        //}

        float aimAngleUpperLimit_L = 150;
        float aimAngleLowerLimit_L = 120;

        float aimAngleUpperLimit_R = 60;
        float aimAngleLowerLimit_R = 30;

        if (targetInRange)//玩家在射击范围内
        {
            aimAngleUpperLimit_L += 10;
            aimAngleLowerLimit_L -= 10;
            aimAngleUpperLimit_R += 10;
            aimAngleLowerLimit_R -= 10;
        }

        if (targetOnLeftSide)//如果玩家在左边
        {
            targetInRange = aimAngle < aimAngleUpperLimit_L && aimAngle > aimAngleLowerLimit_L;//玩家在射击范围内

            if (aimAngle > aimAngleUpperLimit_L)//玩家位置超过了射击范围最大度数，玩家在左边
            {
                direction = Vector3.left;       //移动方向设置为左
                transform.right = -direction;   
            }

            if (aimAngle < aimAngleLowerLimit_L)//玩家位置小于射击范围最小度数，玩家在右边
            {
                direction = Vector3.right;      //移动方向设置为右
                transform.right = -direction;   
            }
        }
        else//玩家不在左边
        {
            targetInRange = aimAngle < aimAngleUpperLimit_R && aimAngle > aimAngleLowerLimit_R;

            if (aimAngle > aimAngleUpperLimit_R)
            {
                direction = Vector3.left;
                transform.right = -direction;
            }

            if (aimAngle < aimAngleLowerLimit_R)
            {
                direction = Vector3.right;
                transform.right = -direction;
            }
        }


        //if (aimAngle > 95)
        //{
        //    direction = Vector3.left;
        //    transform.rotation = Quaternion.Euler(0, 0, 0);

        //    targetInRange = aimAngle < 150 && aimAngle > 120;
        //}
        //else if (aimAngle < 85)
        //{
        //    direction = Vector3.right;
        //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //    targetInRange = aimAngle < 60 && aimAngle > 30;
        //}

        if (!targetInRange)
        {
            Debug.Log("find");
            anim.SetBool("Stop", false);
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.right = -aimDirection.x * Vector3.right;

            anim.SetBool("Stop", true);

            if (bulletPerfab != null)
            {
                shoot();
            }
        }

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

        ClampPosition();
    }
    /// <summary>
    /// 正弦飞行
    /// </summary>
    void SineMove()
    {
        velocity_v = Vector3.up * Mathf.Sin(Mathf.PI * 2 * Time.time / changeDiectionPeriod) * sinAmp;

        Vector3 velocity =velocity_h *speed + velocity_v;

        transform.Translate(velocity * speed * Time.deltaTime, Space.World);

        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;

        Vector3 camPos = Camera.main.transform.position;
        float left = camPos.x - Camera.main.orthographicSize * Camera.main.aspect - 2f;
        float right = camPos.x + Camera.main.orthographicSize * Camera.main.aspect + 2f;
        float top = camPos.y + Camera.main.orthographicSize + 1f;
        float bottom = camPos.y - Camera.main.orthographicSize - 1f;

        if (pos.x < left || pos.y < bottom || pos.y > top)
        {
            Destroy(gameObject);
        }
    }

    Vector3 GetAimDirection(Vector3 targetPosition)
    {
        Vector3 aimDirection = (targetPosition - shotPos.position).normalized;
        return aimDirection;
    }

    void SetSpriteByAimDirection(Vector3 aimDirection)
    {
        float angle = Vector3.Angle(Vector3.right, aimDirection)+7.5f;

        int spriteIdx =Mathf.FloorToInt( angle / 15);

        spriteRenderer.sprite = turrentSprites[spriteIdx];

    }

    float GetAngle(Vector3 direction)
    {
        float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
        return angle;
    }

    public void shoot()
    {
        if(Time.time - lastFireTime > fireInterval)
        {
            Vector3 direction = GetAimDirection(player.transform.position);


            if (GetAngle(direction) < 0f)
            {
                direction.y = -direction.y;
            }

            GameObject bulletInstance = Instantiate(bulletPerfab, shotPos.position, Quaternion.identity);
            bulletInstance.GetComponent<BulletMove>().moveDirection =direction;

            Debug.DrawLine(shotPos.position, shotPos.position + direction * 5, Color.red, 5f);

            lastFireTime = Time.time;
        }
    }

    public void Hurt(int damage)
    {
        hp -= damage;

        if(hp<= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(squadonManager != null)
        {
            squadonManager.OnMenberDestroy(transform.position);
        }

        Instantiate(explosionPrefab, transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {

        if(shotPos != null&& player !=null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(shotPos.position, shotPos.position + GetAimDirection(player.transform.position) * 5);
        }
       
    }
}
