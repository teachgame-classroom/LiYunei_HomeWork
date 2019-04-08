using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern { Static, Straight, ZigSaw, Sine}

public enum MoveDirection { Left, Right, Up, Down }

public class Enemy : MonoBehaviour
{
    public MovePattern movePattern;

    public MoveDirection straightmoveDirection;
    private  float straightMoveDistance = 1;

    private float straightMoveTotalDistance;

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
    }
    /// <summary>
    /// 直线飞行
    /// </summary>
    void StraightMove()
    {
        Vector3 direction = Vector3.zero;
        bool findTarget = false;

        if(GetStraightAngle(GetAimDirection(player.transform.position)) > 95)
        {
            direction = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 0, 0);

            findTarget = GetAngle(GetAimDirection(player.transform.position)) < 150 && GetAngle(GetAimDirection(player.transform.position)) > 120;
        }
        if (GetStraightAngle(GetAimDirection(player.transform.position)) < 85)
        {
            direction = Vector3.right;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            findTarget = GetAngle(GetAimDirection(player.transform.position)) < 60 && GetAngle(GetAimDirection(player.transform.position)) > 30;
        }

        if (straightMoveDistance > 0)
        {
            if (!findTarget)
            {
                anim.SetBool("Stop", false);
                transform.Translate(direction * speed * Time.deltaTime,Space.World);
            }
            else
            {
                anim.SetBool("Stop", true);

                if (bulletPerfab != null)
                {
                    shoot();
                }
            }
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    Vector3 GetStraightMoveDirection()
    {
        switch (straightmoveDirection)
        {
            case MoveDirection.Left:
                return Vector3.left;
            case MoveDirection.Right:
                return Vector3.right;
            case MoveDirection.Up:
                return Vector3.up;
            case MoveDirection.Down:
                return Vector3.down;
            default:
                return Vector3.right;
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

    float GetStraightAngle(Vector3 direction)
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

    private void Die()
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
