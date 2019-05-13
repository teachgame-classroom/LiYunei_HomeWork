using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string[] hurtTags;
    public int maxHp=1;

    public bool invincible = false;
    public bool dropPowerUp = false;

    public int hp { get; protected set; }

    public float baseSpeed =1;
    public bool drawMovetrail = false;

    protected Transform[] shotPosTrans;
    protected Weapon currentWeapon;

    protected List<Vector3> tracks = new List<Vector3>();
    protected float lastRecordTime;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected GameObject dieEffect;

    protected GameObject powerUpPrefab;

    protected abstract string deathClipName { get; }
    protected AudioClip deathClip;

    protected virtual void Start()
    {
        InitCharacter();
        InitWeapon();
    }

    protected virtual void Update()
    {
        Move();
        Shoot();

        if (drawMovetrail)
        {
            RecordMoveTrail();
        }
    }

    protected virtual void InitCharacter()
    {
        hp = maxHp;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        deathClip = Resources.Load<AudioClip>("Gradius/Prefabs/Sounds/"+deathClipName);

        if (dropPowerUp)
        {
            powerUpPrefab = Resources.Load<GameObject>("Gradius/Prefabs/PowerUp");
        }
    }

    protected virtual void InitWeapon()
    {
        ShotPosMarker[] markers = GetComponentsInChildren<ShotPosMarker>();

        if (markers.Length > 0)
        {
            shotPosTrans = new Transform[markers.Length];

            for (int i = 0; i < markers.Length; i++)
            {
                shotPosTrans[i] = markers[i].transform;
            }
        }
    }

    protected virtual void Move()
    {
        Move(Vector3.left);
    }

    protected virtual void Move(Vector3 moveDirection)
    {
        transform.Translate(moveDirection * baseSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void Shoot()
    {
        if (currentWeapon != null)
        {
            currentWeapon.TryShoot();
        }
    }

    protected virtual void Hurt(int damage)
    {
        if(!invincible)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Die();
            }
        }
    }
    
    protected virtual void PlayDieEffect()
    {
        Instantiate(dieEffect, transform.position, Quaternion.identity);
    }

    protected virtual void Die()
    {
        if (dropPowerUp)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }

        if (dieEffect)
        {
            PlayDieEffect();
        }

        AudioSource.PlayClipAtPoint(deathClip, Camera.main.transform.position);

        Destroy(gameObject);
    }


    protected void RecordMoveTrail()
    {
        if (Time.time - lastRecordTime > 0.1f)
        {
            tracks.Add(transform.position);
            if (tracks.Count > 48)
            {
                tracks.RemoveAt(0);
            }
            lastRecordTime = Time.time;
        }
    }

    protected void OnDrawGizmos()
    {
        if (drawMovetrail)
        {
            for(int i = 0; i < tracks.Count; i++)
            {
                Gizmos.DrawSphere(tracks[i], 0.1f);
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible)
        {
            for (int i = 0; i < hurtTags.Length; i++)
            {
                if (collision.tag == hurtTags[i])
                {
                    int damage = maxHp;
                    BulletDamage bullet = collision.GetComponent<BulletDamage>();

                    if (bullet) damage = bullet.damage;

                    Hurt(damage);

                    bullet.OnHit();
                }
            }
        }
    }
}
