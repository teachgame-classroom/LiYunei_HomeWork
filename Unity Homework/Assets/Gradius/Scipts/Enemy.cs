using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern { Static, Straight, ZigSaw, Sine, Evade, Normal}
public enum Position { Up,Down}

public class Enemy : MonoBehaviour
{
    public MovePattern movePattern;
    public Position position;

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

    public bool waitForplayer = true;
    private bool activated = false;
    public float activefloat = 2f;
    /// <summary>
    /// 激活摄像机距离
    /// </summary>
    private float activeDistance;

    private GameObject player;
    private GameObject[] playerBullets;

    public Sprite[] turrentSprites;
    private SpriteRenderer spriteRenderer;
    private Collider2D col2D;
    private Transform shotPos;

    private Animator anim;

    public GameObject bulletPerfab;
    public float fireInterval;
    private float lastFireTime;

    private string[] stageLayeMask = new string[] { "Stage" };

    private Vector3 scaleVec = new Vector3(-1, 1, 1);

    private bool isEvading;
    private Vector3 evadeVelocity;
    private float lastEvadeTime;

    private GameObject powerUpPrefab;
    public bool dropPowerUp = false;

    // Start is called before the first frame update
    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/Explosion_Red");
        powerUpPrefab = Resources.Load<GameObject>("Gradius/Prefabs/PowerUp");

        player = GameObject.Find("Vic Viper");

        shotPos = transform.Find("ShotPos");


        if(shotPos == null)
        {
            shotPos = transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();

        activeDistance = Camera.main.orthographicSize * Camera.main.aspect + activefloat;

        if (waitForplayer == true)
        {
            SetEnemyActive(false);
            
        }
        else
        {
            SetEnemyActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated == false)
        {
            if (IsCameraCloseEnough())
            {
                SetEnemyActive(true);
            }
        }
        else
        {
            if (squadonManager == null)
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
                    case MovePattern.Evade:
                        EvadeMove();
                        break;
                    case MovePattern.Normal:
                        break;
                }
            }
        }
        IsCameraLeaveEnough();
    }

    private void SetEnemyActive(bool isActive)
    {
        col2D.enabled = isActive;
        activated = isActive;
        if(anim != null)
        {
            anim.enabled = isActive;
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
            Shoot();
        }
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

        Vector3 groundNormal = GetGroundNormal();
        Vector3 footPos = GetFootPos();

        if (targetOnLeftSide)//如果玩家在左边
        {
            targetInRange = aimAngle < aimAngleUpperLimit_L && aimAngle > aimAngleLowerLimit_L;//玩家在射击范围内

            if (aimAngle > aimAngleUpperLimit_L)//玩家位置超过了射击范围最大度数，玩家在左边
            {
                direction = Vector3.left;       //移动方向设置为左
                direction = Vector3.ProjectOnPlane(direction, groundNormal);

                Quaternion rot = Quaternion.LookRotation(Vector3.forward, groundNormal);
                transform.rotation = rot;
                //transform.right = -direction;
                //transform.up = groundNormal;
            }

            if (aimAngle < aimAngleLowerLimit_L)//玩家位置小于射击范围最小度数，玩家在右边
            {
                direction = Vector3.right;      //移动方向设置为右
                direction = Vector3.ProjectOnPlane(direction, groundNormal);

                Quaternion rot = Quaternion.LookRotation(Vector3.back, groundNormal);
                transform.rotation = rot;
            }
        }
        else//玩家不在左边
        {
            targetInRange = aimAngle < aimAngleUpperLimit_R && aimAngle > aimAngleLowerLimit_R;

            if (aimAngle > aimAngleUpperLimit_R)
            {
                direction = Vector3.left;
                direction = Vector3.ProjectOnPlane(direction, groundNormal);

                Quaternion rot = Quaternion.LookRotation(Vector3.forward, groundNormal);
                transform.rotation = rot;
            }

            if (aimAngle < aimAngleLowerLimit_R)
            {
                direction = Vector3.right;
                direction = Vector3.ProjectOnPlane(direction, groundNormal);
                Quaternion rot = Quaternion.LookRotation(Vector3.back, groundNormal);
                transform.rotation = rot;
            }
        }

        if (groundNormal != Vector3.zero)
        {
            transform.position = footPos;
        }

        Debug.DrawLine(transform.position, transform.position + direction, Color.cyan);

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
            anim.SetBool("Stop", false);
            transform.Translate((direction) * speed * Time.deltaTime, Space.World);
        }
        else
        {

            if (targetOnLeftSide)
            {
                transform.right =  Vector3.right;
                Quaternion rot = Quaternion.LookRotation(Vector3.forward, groundNormal);
                transform.rotation = rot;
            }
            else
            {
                transform.right =  Vector3.left;
                Quaternion rot = Quaternion.LookRotation(Vector3.back, groundNormal);
                transform.rotation = rot;
            }

            anim.SetBool("Stop", true);

            if (bulletPerfab != null)
            {
                Shoot();
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
    /// <summary>
    /// 规避
    /// </summary>
    void EvadeMove()
    {
        velocity_v = Vector3.up * Mathf.Sin(Mathf.PI * 2 * Time.time / changeDiectionPeriod) * sinAmp;
        Vector3 velocity = velocity_v;

        if (!isEvading)
        {
            float evade = GetPlayerBullets();

            float top = Camera.main.transform.position.y + Camera.main.orthographicSize;
            float bottom = Camera.main.transform.position.y - Camera.main.orthographicSize;

            float topDistance = top - transform.position.y;
            float bottomDistance = transform.position.y - bottom;

            if(bottomDistance < 5)
            {
                if(evade < 0)
                {
                    evade = -evade;
                }
            }
            else if (topDistance < 5)
            {
                if (evade > 0)
                {
                    evade = -evade;
                }
            }

            if (Mathf.Abs(evade) > 0.001f)
            {
                isEvading = true;
                evadeVelocity = Vector3.up * evade;
                lastEvadeTime = Time.time;
            }
            else
            {
                velocity = velocity_v;
                transform.Translate(velocity * Time.deltaTime + Vector3.right * Time.deltaTime, Space.World);
            }
        }
        else
        {
            velocity =velocity_v+evadeVelocity;
            transform.Translate(velocity * Time.deltaTime + Vector3.right * Time.deltaTime, Space.World);
            if (Time.time - lastEvadeTime > 1f)
            {

                isEvading = false;
            }
        }

        Shoot();
    }

    bool IsCameraCloseEnough()
    {
        float right = transform.position.x - Camera.main.transform.position.x;

        return right < activeDistance;
    }

    void IsCameraLeaveEnough()
    {
        float left = Camera.main.transform.position.x - transform.position.x;

        if (activeDistance < left )
        {
            if (squadonManager == null)
            {
                Destroy(gameObject);
            }
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

    float GetAngle(Vector3 direction,Vector3 distance)
    {
        float angle = Vector3.SignedAngle(distance, direction, Vector3.forward);
        return angle;
    }

    Vector3 GetGroundNormal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.5f, -transform.up, 1f, LayerMask.GetMask(stageLayeMask));

        if (hit.transform != null)
        {
            Debug.Log(hit.transform.name);
            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red, 1f);

            return hit.normal;
        }
        else
        {
            return Vector3.zero;
        }
    }

    Vector3 GetFootPos()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.5f, -transform.up, 1f, LayerMask.GetMask(stageLayeMask));

        if (hit.transform != null)
        {
            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red, 1f);
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    int GetPlayerBullets()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 3f);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag != "PlayerBullet") continue;

            float distance = BulletsDistance(transform.position, cols[i].transform.position);
            if (distance < 3f)
            {
                int test = Random.Range(-5, 5);
                return test;
            }
        }
        return 0;
    }

    public float BulletsDistance(Vector3 selfPos, Vector3 bulletPos)
    {
        Vector3 distance = bulletPos - selfPos;
        
        return distance.magnitude;
    }

    public void Shoot()
    {
        if(Time.time - lastFireTime > fireInterval)
        {
            Vector3 direction = GetAimDirection(player.transform.position);

            if (movePattern == 0)
            {
                if (position == 0)
                {
                    if (GetAngle(direction) > 0f)
                    {
                        direction.y = -direction.y;
                    }
                }
                else
                {
                    if (GetAngle(direction) < 0f)
                    {
                        direction.y = -direction.y;
                    }
                }
            }

            GameObject bulletInstance = Instantiate(bulletPerfab, shotPos.position, Quaternion.identity);
            bulletInstance.GetComponent<BulletMove>().moveDirection =direction;

            Debug.DrawLine(shotPos.position, shotPos.position + direction * 5, Color.red, 5f);

            lastFireTime = Time.time;
        }
    }

    public void Hurt(int damage)
    {
        if(hp > 0)
        {
            hp -= damage;

            if (hp <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        if(squadonManager != null)
        {
            squadonManager.OnMenberDestroy(transform.position);
        }

        Instantiate(explosionPrefab, transform.position,Quaternion.identity);

        if (dropPowerUp)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if(shotPos != null&& player !=null)
        {
            //Gizmos.color = Color.white;
            //Gizmos.DrawLine(shotPos.position, shotPos.position + GetAimDirection(player.transform.position) * 5);
        }
       
    }
}
