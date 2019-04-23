using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePattern { Static, Straight, ZigSaw, Sine, InvertSine, Evade, Normal}
public enum BulletMovePattern { AimAtPlayer, Horizontal, Vertical}

public class Enemy : MonoBehaviour
{
    public MovePattern movePattern;
    public BulletMovePattern bulletMovePattern = BulletMovePattern.AimAtPlayer;

    private float straightMoveTotalDistance;

    public float speed = 5;
    public float changeDiectionPeriod = 1f;
    public float sinAmp = 1;

    private bool targetInRange;

    private float lastChangDirectionTime = 0;
    private Vector3 velocity_h = Vector3.left ;
    private Vector3 velocity_v = Vector3.up;

    public int hp = 1;
    public bool invincible = false;
    public bool canColliderDesity = true;
    private GameObject[] explosionPrefab = new GameObject[2];
    public bool explosionAttachToPattent = false;

    public bool playDamageEffect = false;
    public Color EffectColor = new Color(1,0,0);

    /// <summary>
    /// 该敌人所属小队，敌人生成时由小队脚本指定
    /// </summary>
    public SquadonManager squadonManager;

    public bool waitForplayer = true;
    public bool setActivePos = false;
    private bool activated = false;
    public float activeFloat = 2f;
    public float activePos = 0;
    /// <summary>
    /// 激活摄像机距离
    /// </summary>
    private float activeDistance;

    private List<Vector3> tracks = new List<Vector3>();
    private float lastRecordTime = 0;

    private CameraMove cameraMove;

    private GameObject player;
    private GameObject[] playerBullets;

    public float turretMaxAngle = 180;
    public float turretMinAngle = 0;

    public Sprite[] turrentSprites;
    private SpriteRenderer spriteRenderer;
    private Collider2D col2D;
    private Transform shotPos;

    private Animator anim;

    public GameObject bulletPerfab;
    public GameObject bulletEffect;
    private GameObject effect;
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
        explosionPrefab[0] = Resources.Load<GameObject>("Gradius/Prefabs/Effects/Explosion_Red");
        explosionPrefab[1] = Resources.Load<GameObject>("Gradius/Prefabs/Effects/LoopExplosion");
        powerUpPrefab = Resources.Load<GameObject>("Gradius/Prefabs/PowerUp");
        cameraMove = Camera.main.GetComponent<CameraMove>();
        player = GameObject.Find("Vic Viper");

        shotPos = transform.Find("ShotPos");


        if(shotPos == null)
        {
            shotPos = transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();

        activeDistance = Camera.main.orthographicSize * Camera.main.aspect + activeFloat;

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
                if (setActivePos)
                {
                    transform.position += Vector3.left * 0.5f * speed * Time.deltaTime;

                    float distanceToCamera = transform.position.x - Camera.main.transform.position.x;
                    float distanceToExitSpawnState = Camera.main.orthographicSize * Camera.main.aspect * activePos;

                    if (distanceToCamera < distanceToExitSpawnState)
                    {
                        SetEnemyActive(true);
                    }
                }
                else
                {
                    SetEnemyActive(true);
                }
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
                    case MovePattern.InvertSine:
                        InvertSineMove();
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
        Vector3 aimDirection = GetAimDirection(player.transform.position);
        float angle = Vector3.SignedAngle(Vector3.right, aimDirection, Vector3.forward);
        bool plyaerNotInTurretAngle = (angle < turretMinAngle || angle > turretMaxAngle);

        if (plyaerNotInTurretAngle) { return; }

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

        transform.Translate(velocity *speed* Time.deltaTime+ CameraMoveDriection() * Time.deltaTime, Space.World);
    }
    /// <summary>
    /// 正弦飞行
    /// </summary>
    void SineMove()
    {
        velocity_v = Vector3.up * Mathf.Sin(Mathf.PI * 2 * Time.time / changeDiectionPeriod) * sinAmp;

        Vector3 velocity =velocity_h *speed + velocity_v;

        transform.Translate(velocity * speed * Time.deltaTime+ CameraMoveDriection() * Time.deltaTime, Space.World);
    }
    /// <summary>
    /// 跳跃移动
    /// </summary>
    void InvertSineMove()
    {
        float sine = 1 * Mathf.Sin(Mathf.PI * 2 * Time.time / changeDiectionPeriod) * sinAmp;

        Debug.Log(sine);

        if (sine > -2.5 && sine < 2.5)
        {
            sine = -sine;
        }
        velocity_v = Vector3.up * sine;

        Vector3 velocity = velocity_h * speed + velocity_v;

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

            if (bottomDistance < 5)
            {
                if (evade < 0)
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
                transform.Translate(velocity * Time.deltaTime + CameraMoveDriection() * Time.deltaTime, Space.World);
            }
        }
        else
        {
            velocity = velocity_v + evadeVelocity;
            transform.Translate(velocity * Time.deltaTime + CameraMoveDriection() * Time.deltaTime, Space.World);
            if (Time.time - lastEvadeTime > 1f)
            {

                isEvading = false;
            }
        }

        Shoot();
    }

    private Vector3 CameraMoveDriection()
    {
        return cameraMove.moveDirection * cameraMove.speed;
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
        Vector3 aimDirection = Vector3.left;

        switch (bulletMovePattern)
        {
            case BulletMovePattern.Horizontal:
                aimDirection = Vector3.left;
                break;
            case BulletMovePattern.Vertical:
                aimDirection = Vector3.up;
                break;
            case BulletMovePattern.AimAtPlayer:
                aimDirection = (targetPosition - shotPos.position).normalized;
                break;
            default:
                aimDirection = Vector3.left;
                break;
        }
        return aimDirection;
    }

    void SetSpriteByAimDirection(Vector3 aimDirection)
    {
        if (turrentSprites.Length == 0) return;

        float angleStep =( turretMaxAngle - turretMinAngle )/ (turrentSprites.Length-1);

        float angle = Vector3.SignedAngle(Vector3.right, aimDirection,Vector3.forward) + angleStep / 2;

        angle = Mathf.Clamp(angle, turretMinAngle, turretMaxAngle);

        int spriteIdx = Mathf.Abs(Mathf.FloorToInt(angle / angleStep))-1;

        spriteIdx = Mathf.Clamp(spriteIdx, 0, turrentSprites.Length - 1);

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = turrentSprites[spriteIdx];
        }

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
            //Debug.Log(hit.transform.name);
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
        if (bulletEffect != null)
        {
            if (Time.time - lastFireTime +2> fireInterval)
            {
                if (effect == null)
                {
                    effect = Instantiate(bulletEffect, shotPos.position, Quaternion.identity);
                }
            }
            if (Time.time - lastFireTime > fireInterval)
            {
                Destroy(effect);
            }
        }

        if (Time.time - lastFireTime > fireInterval)
        {
            Vector3 direction = GetAimDirection(player.transform.position);

            GameObject bulletInstance = Instantiate(bulletPerfab, shotPos.position, Quaternion.identity);
            bulletInstance.GetComponent<BulletMove>().moveDirection = direction;

            lastFireTime = Time.time;
        }


    }

    public void Hurt(int damage)
    {
        if (!invincible)
        {
            if (hp > 0)
            {
                hp -= damage;
                if (hp <= 0)
                {
                    Die();
                }
                else if(playDamageEffect)
                {
                    spriteRenderer.color = EffectColor;
                    Invoke("ChangeSprite", 0.1f);
                }
            }
        }
    }

    public void ChangeSprite()
    {
        spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        if(squadonManager != null)
        {
            squadonManager.OnMenberDestroy(transform.position);
        }
        if (!explosionAttachToPattent)
        {
            Instantiate(explosionPrefab[0], transform.position, Quaternion.identity);
        }
        else
        {
            GameObject explosion = Instantiate(explosionPrefab[1], transform.position, Quaternion.identity);

            explosion.transform.SetParent(transform.parent);
            LoopExplosion loopExplosion = explosion.GetComponent<LoopExplosion>();

            if (loopExplosion)
            {
                float xMax = col2D.bounds.center.x + col2D.bounds.extents.x - transform.position.x;
                float xMin = col2D.bounds.center.x - col2D.bounds.extents.x - transform.position.x;
                float yMax = col2D.bounds.center.y + col2D.bounds.extents.y - transform.position.y;
                float yMin = col2D.bounds.center.y - col2D.bounds.extents.y - transform.position.y;

                loopExplosion.explosionAreaMin = Vector2.right * xMin + Vector2.up * yMin;
                loopExplosion.explosionAreaMax = Vector2.right * xMax + Vector2.up * yMax;
            }
        }


        if (dropPowerUp)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
        Destroy(effect);
        Destroy(gameObject);
    }

    void DrawFollor()
    {
        if(Time.time -lastRecordTime > 0.1f)
        {
            tracks.Add(transform.position);
            if(tracks.Count > 48)
            {
                tracks.RemoveAt(0);
            }
            lastRecordTime = Time.time;
        }
    }

    private void OnDrawGizmos()
    {
        if (shotPos != null&& player !=null)
        {
            //Gizmos.color = Color.white;
            //Gizmos.DrawLine(shotPos.position, shotPos.position + GetAimDirection(player.transform.position) * 5);
        }
       
    }
}
