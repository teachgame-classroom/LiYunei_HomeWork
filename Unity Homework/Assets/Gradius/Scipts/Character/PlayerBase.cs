using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Character
{
    protected const int KEY_W = 0;
    protected const int KEY_S = 1;
    protected const int KEY_A = 2;
    protected const int KEY_D = 3;
    protected const int KEY_J = 4;
    protected const int KEY_K = 5;
    protected const int KEY_1 = 6;
    protected const int KEY_2 = 7;

    protected bool[] KeyState = new bool[10];

    protected const int TOTAL_WEAPON = 3;
    protected Weapon[] weapons = new Weapon[TOTAL_WEAPON];
    protected int currentWeaponIdx = 0;
    public Missile missile { get; protected set; }
    public BulletDamage laser { get; protected set; }

    protected const int START_LIFE = 5;
    public int life { get; protected set; }

    protected int powerUp = 0;
    protected float finalSpeed = 0;
    public int speedLevel { get; protected set; }

    protected override string deathClipName { get { return "Sound Effect (17)"; } }
    protected string getPowerupClipName = "Sound Effect (14)";
    protected AudioClip getPowerupClip;
    protected string powerupClipName = "Sound Effect (16)";
    protected AudioClip powerupClip;

    protected GameObject optionPrefab;

    protected List<Transform> optionList = new List<Transform>();
    protected List<Vector3> trackList = new List<Vector3>();
    protected float trackNodeDistance = 0.025f * 0.025f;

    protected Transform spawnTans;
    protected float lastSpawnTime;
    protected float lastBlinkTime;

    protected bool isJustSpawn = false;
    protected bool isUpDouble = false;
    protected bool isUpLaser = false;
    protected bool isUpBarrier = false;

    protected override void Start()
    {
        base.Start();

        trackList.Add(transform.position);
    }

    protected override void Update()
    {
        UpdataKeyState();

        if (KeyState[KEY_K]) TryPowerUp();

        UpdataTrackList();

        for (int i = 0; i < optionList.Count; i++)
        {
            optionList[i].transform.position = Vector3.MoveTowards(optionList[i].transform.position, trackList[i * 8], finalSpeed * Time.deltaTime);
            shotPosTrans[i+1] = optionList.ToArray()[i];
        }

        if (invincible) if(Time.time- lastBlinkTime > 0.1f)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                lastBlinkTime = Time.time;
            }
        if (Time.time - lastSpawnTime > 3f)
        {
            invincible = false;
            spriteRenderer.enabled = true;
        }

        base.Update();
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();

<<<<<<< HEAD
        life = START_LIFE;
        UI.instance.OnLifeChange(life);
=======
        powerUp = 0;
        speedLevel = 0;

        isUpDouble = false;
        isUpLaser = false;
        isUpBarrier = false;

        SetSpeed();

        SetBarrierActive(false);
>>>>>>> parent of 07a3960... updata UI

        if(hurtTags.Length == 0)
        {
            hurtTags = new string[] { "Stage", "Enemy", "EnemyBullet" };
        }

        dieEffect = Resources.Load<GameObject>("Gradius/Prefabs/Effects/Explosion_player");
        optionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Option");
        getPowerupClip = Resources.Load<AudioClip>("Gradius/Prefabs/Sounds/" + getPowerupClipName);
        powerupClip = Resources.Load<AudioClip>("Gradius/Prefabs/Sounds/" + powerupClipName);

        spawnTans = Camera.main.transform.Find("PlayerSpawn");

        Spawn();
    }

    protected override void InitWeapon()
    {
        ShotPosMarker[] markers = GetComponentsInChildren<ShotPosMarker>();

        if (markers.Length > 0)
        {
            shotPosTrans = new Transform[10];

            for (int i = 0; i < markers.Length; i++)
            {
                shotPosTrans[i] = markers[i].transform;
            }
        }

        weapons[0] = new NormalWeapon(shotPosTrans,true);
        weapons[1] = new DoubleCannon(shotPosTrans,true);
        weapons[2] = new Laser(shotPosTrans,true);

        missile = new Missile(shotPosTrans,true);
        laser = new BulletDamage();

        currentWeaponIdx = 0;
        currentWeapon = weapons[currentWeaponIdx];
    }

    protected override void Move()
    {
        if (isJustSpawn)
        {
            if (Time.time - lastSpawnTime > 1)
            {
                transform.position += Vector3.right * finalSpeed * Time.deltaTime;

                float distanceToCamera = Camera.main.transform.position.x - transform.position.x;
                float distanceToExitSpawnState = Camera.main.orthographicSize * Camera.main.aspect *0.8f;

                if (distanceToCamera < distanceToExitSpawnState)
                {
                    isJustSpawn = false;
                }
            }
        }
        else
        {
            Vector3 moveDirection = Vector3.zero;

            if (KeyState[KEY_W]) moveDirection += Vector3.up;
            if (KeyState[KEY_S]) moveDirection += Vector3.down;
            if (KeyState[KEY_A]) moveDirection += Vector3.left;
            if (KeyState[KEY_D]) moveDirection += Vector3.right;

            if (moveDirection.y > 0) animator.SetInteger("Move", 1);
            else if (moveDirection.y < 0) animator.SetInteger("Move", 2);
            else animator.SetInteger("Move", 0);

            Move(moveDirection);
        }
    }

    protected override void Move(Vector3 moveDirection)
    {
<<<<<<< HEAD
        transform.Translate(moveDirection * finalSpeed * Time.deltaTime, Space.World);
    }

    protected override void Shoot()
    {
=======
        if (Input.GetKeyDown(KeyCode.C))
        {
            powerUp++;
            powerUp = powerUp % 7;
        }

>>>>>>> parent of 07a3960... updata UI
        if (KeyState[KEY_1]) TryChangePrimaryWeapon(1);//Normal or Double
        if (KeyState[KEY_2]) TryChangePrimaryWeapon(2);//Laser

        if (missile != null)
        {
            missile.TryShoot();
        }

        if (!isJustSpawn)
        {
            base.Shoot();
        }
    }

    protected void UpdataKeyState()
    {
        KeyState[KEY_W] = Input.GetKey(KeyCode.W);
        KeyState[KEY_S] = Input.GetKey(KeyCode.S);
        KeyState[KEY_A] = Input.GetKey(KeyCode.A);
        KeyState[KEY_D] = Input.GetKey(KeyCode.D);
        KeyState[KEY_J] = Input.GetKeyDown(KeyCode.J);
        KeyState[KEY_K] = Input.GetKeyDown(KeyCode.K);
        KeyState[KEY_1] = Input.GetKeyDown(KeyCode.Alpha1);
        KeyState[KEY_2] = Input.GetKeyDown(KeyCode.Alpha2);
    }

    void TryChangePrimaryWeapon(int newWeaponIdx)
    {
        switch (newWeaponIdx)
        {
            case 1:
                if (!isUpDouble)
                {
                    ChangePrimaryWeapon(0);
                }
                else ChangePrimaryWeapon(1);
                break;
            case 2:
                if (isUpLaser) ChangePrimaryWeapon(2);
                break;
        }
    }

    protected void ChangePrimaryWeapon(int newWeaponIdx)
    {
        powerUp = 0;

        newWeaponIdx = Mathf.Clamp(newWeaponIdx, 0, weapons.Length - 1);
        currentWeaponIdx = newWeaponIdx;

        currentWeapon = weapons[currentWeaponIdx];
    }

<<<<<<< HEAD
    void PowerupChange(bool isAdd)
    {
        if (isAdd)
        {
            powerup++;

            if (powerup > 6)
            {
                powerup = 0;
                life++;
                life = Mathf.Min(life,9);
                UI.instance.OnLifeChange(life);
            }
            UI.instance.OnPowerupChanged(powerup);
        }
        else
        {
            powerup = 0;
            UI.instance.OnPowerupChanged(powerup);
        }
    }

    void TryPowerUp()
    {
        if (powerup > 0)
        {
            UI.instance.OnPowerup();
            AudioSource.PlayClipAtPoint(powerupClip, Camera.main.transform.position);
        }

        switch (powerup)
=======
    void TryPowerUp()
    {
        switch (powerUp)
>>>>>>> parent of 07a3960... updata UI
        {
            case 1:
                PowerUpSpeed();
                break;
            case 2:
                PowerUpMissile();
                break;
            case 3:
                PowerUpDouble();
                break;
            case 4:
                PowerUpLaser();
                break;
            case 5:
                PowerUpOption();
                break;
            case 6:
                PowerUpBarrier();
                break;
        }
    }

    void PowerUpSpeed()
    {
<<<<<<< HEAD
        if (speedLevel < 5)
        {
            powerup = 0;
            speedLevel++;
            SetSpeed();
        }
=======
        powerUp = 0;

        speedLevel++;
        speedLevel = Mathf.Min(5, speedLevel);
>>>>>>> parent of 07a3960... updata UI

    }

    void SetSpeed()
    {
        finalSpeed = baseSpeed + speedLevel * 2;
    }

    void PowerUpMissile()
    {
        if (missile.level < 2)
        {
            powerUp = 0;
            missile.LevelUp();
        }
    }

    void PowerUpDouble()
    {
        if (isUpDouble == false)
        {
            powerUp = 0;
            isUpDouble = true;
        }
        ChangePrimaryWeapon(1);
    }

    void PowerUpLaser()
    {
<<<<<<< HEAD
        if (laser.laserCount < 5)
=======
        powerUp = 0;
        if (isUpLaser == false)
>>>>>>> parent of 07a3960... updata UI
        {
            powerup = 0;
            laser.laserCount++;
            isUpLaser = true;
            ChangePrimaryWeapon(2);
        }
    }

    void PowerUpOption()
    {
<<<<<<< HEAD
        powerup = 0;
        if (optionList.Count < 5)
=======
        powerUp = 0;
        CreatOption();

        for (int i = 0; i < weapons.Length; i++)
>>>>>>> parent of 07a3960... updata UI
        {
            CreatOption();

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].PowerOpint();
            }
            missile.PowerOpint();
        }
    }

    void CreatOption()
    {
        optionList.Add(Instantiate(optionPrefab, transform.position, Quaternion.identity).transform);
    }

    void UpdataTrackList()
    {
        if (Vector3.SqrMagnitude(transform.position - trackList[trackList.Count - 1]) > trackNodeDistance)
        {
            trackList.Add(transform.position);

            if (trackList.Count > (optionList.Count) * 8+1)
            {
                trackList.RemoveAt(0);
            }
        }
    }

    void PowerUpBarrier()
    {
        if (!isUpBarrier)
        {
            powerUp = 0;
            SetBarrierActive(true);
        }
    }

    private void SetBarrierActive(bool isActice)
    {
        transform.Find("Bullet_4_1").gameObject.SetActive(isActice);
        transform.Find("Bullet_4_2").gameObject.SetActive(isActice);
        isUpBarrier = isActice;
    }

    protected override void Hurt(int damage)
    {
        base.Hurt(damage);
        UI.instance.OnMetersChange(hp);
    }

    protected override void Die()
    {
        life--;
        UI.instance.OnLifeChange(life);
        PlayDieEffect();

        if (life > 0)
        {
            Spawn();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Spawn()
    {
        hp = maxHp;
        powerUp = 0;

        speedLevel = 0;
        SetSpeed();
        if (missile!=null) missile.Reset();
        isUpDouble = false;
        isUpLaser = false;
        if(laser!=null)laser.laserCount = 3;

        optionList.Clear();
        SetBarrierActive(false);

        ChangePrimaryWeapon(0);
        UI.instance.ResetAll();

        transform.position = spawnTans.position;
        invincible = true;
        isJustSpawn = true;
        lastSpawnTime = Time.time;
    }

<<<<<<< HEAD
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(collision.tag == "PowerUp")
        {
            PowerupChange(true);

            GameController.instance.AddScore(500);
            Destroy(collision.gameObject);

            AudioSource.PlayClipAtPoint(getPowerupClip, Camera.main.transform.position);
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("Power Up:{0}", powerup));
=======
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("Power Up:{0}", powerUp));
>>>>>>> parent of 07a3960... updata UI
        GUILayout.Label(string.Format("Current weapon:{0}", currentWeaponIdx));
        GUILayout.Label(string.Format("HP:{0}", hp));
        GUILayout.Label(string.Format("Speed:{0}", finalSpeed));
        GUILayout.Label(string.Format("Missle level:{0}",missile.level));
        GUILayout.EndVertical();
    }
}
