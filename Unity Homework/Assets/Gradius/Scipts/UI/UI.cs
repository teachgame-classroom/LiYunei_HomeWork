using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Canvas").GetComponentInChildren<UI>();
            }
            return _instance;
        }
    }
    private static UI _instance;

    private GameObject player;
    private PlayerBase playerBase;
    private Transform meterTrans;
    private string[] weaponName = new string[] { "SpeedUp", "Missile", "Double", "Laser", "Option", "Barrier" };

    private Image[] speedUpImages;
    private Image[] missileImages;
    private Image[] doubleImages;
    private Image[] laserImages;
    private Image[] optionImages;
    private Image[] barrierImages;

    private Text lifeText
    {
        get
        {
            if(_lifeText == null)
            {
                _lifeText = transform.Find("StatusPanel/Life").GetComponent<Text>();
            }
            return _lifeText;
        }
    }

    private Text _lifeText;

    private Text nameText;
    private Text scoreText;

    private int[] weaponStates = new int[6];
    //private KeyCode[] testKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };

    private int currentPowerupPanelIdx = -1;
    private int testPowerup = 0;

    //private int life = 0;
    private int maxHp = 0;
    private int presentHp = 0;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerBase = player.GetComponent<PlayerBase>();
        nameText = transform.Find("StatusPanel/PlayerName").GetComponent<Text>();
        maxHp = player.GetComponent<PlayerBase>().maxHp;
        meterTrans = transform.Find("StatusPanel/MaxHp/PresentHp");
        scoreText = transform.Find("StatusPanel/Score").GetComponent<Text>();

        speedUpImages = GetWeaponImages(weaponName[0]);
        missileImages = GetWeaponImages(weaponName[1]);
        doubleImages = GetWeaponImages(weaponName[2]);
        laserImages = GetWeaponImages(weaponName[3]);
        optionImages = GetWeaponImages(weaponName[4]);
        barrierImages = GetWeaponImages(weaponName[5]);
    }

    void Update()
    {

    }

    public void OnPowerup()
    {
        //ChangeWeaponPanelState(currentPowerupPanelIdx, 2);
        CanChangeWeaponPanel(currentPowerupPanelIdx);
        currentPowerupPanelIdx = -1;

    }

    Image[] GetWeaponImages(string weaponName)
    {
        Image[] images = new Image[3];

        Transform weaponTrans = transform.Find("WeaponPanel/" + weaponName);

        images[0] = weaponTrans.GetChild(0).GetComponent<Image>();
        images[1] = weaponTrans.GetChild(1).GetComponent<Image>();
        images[2] = weaponTrans.GetChild(2).GetComponent<Image>();

        return images;
    }

    void CanChangeWeaponPanel(int weaponId)
    {
        switch (weaponId)
        {
            case 0:
                if (playerBase.speedLevel<5)
                    ChangeWeaponPanelState(weaponId, 0);
                else ChangeWeaponPanelState(weaponId, 2);
                break;
            case 1:
                if (playerBase.missile.level < 1)
                    ChangeWeaponPanelState(weaponId, 0);
                else ChangeWeaponPanelState(weaponId, 2);
                break;
            case 2:
                ChangeWeaponPanelState(weaponId, 2);
                break;
            case 3:
                if (playerBase.laser.laserCount < 5)
                    ChangeWeaponPanelState(weaponId, 0);
                else ChangeWeaponPanelState(weaponId, 2);
                break;
            case 4:
                ChangeWeaponPanelState(weaponId, 2);
                break;
            case 5:
                ChangeWeaponPanelState(weaponId, 2);
                break;
        }
    }

    public void ChangeWeaponPanelState(int weaponId, int statId)
    {
        weaponStates[weaponId] = statId;
        switch (weaponId)
        {
            case 0:
                ToggleWeaponImage(speedUpImages, statId);
                break;
            case 1:
                ToggleWeaponImage(missileImages, statId);
                break;
            case 2:
                ToggleWeaponImage(doubleImages, statId);
                break;
            case 3:
                ToggleWeaponImage(laserImages, statId);
                break;
            case 4:
                ToggleWeaponImage(optionImages, statId);
                break;
            case 5:
                ToggleWeaponImage(barrierImages, statId);
                break;
            default:
                break;
        }
    }

    void ToggleWeaponImage(Image[] images, int stateId)
    {
        switch (stateId)
        {
            case 0:
                images[0].enabled = false;
                images[1].enabled = false;
                images[2].enabled = false;
                break;
            case 1:
                images[0].enabled = true;
                images[1].enabled = false;
                images[2].enabled = false;
                break;
            case 2:
                images[0].enabled = false;
                images[1].enabled = true;
                images[2].enabled = false;
                break;
            case 3:
                images[0].enabled = false;
                images[1].enabled = false;
                images[2].enabled = true;
                break;
            default:
                break;
        }
    }

    public void OnPowerupChanged(int powerupLevel)
    {
        //powerupLevel = Mathf.Clamp(powerupLevel, 0, 6);
        int newtPowerupPanelIdx = powerupLevel - 1;

        if (powerupLevel > 0)
        {
            if (weaponStates[newtPowerupPanelIdx] == 0)
            {
                ChangeWeaponPanelState(newtPowerupPanelIdx, 1);
            }

            if (weaponStates[newtPowerupPanelIdx] == 2)
            {
                ChangeWeaponPanelState(newtPowerupPanelIdx, 3);
            }
        }

        if (this.currentPowerupPanelIdx > -1)
        {
            if (weaponStates[this.currentPowerupPanelIdx] == 3)
            {
                ChangeWeaponPanelState(this.currentPowerupPanelIdx,2);
            }
            if(weaponStates[this.currentPowerupPanelIdx] == 1)
            {
                ChangeWeaponPanelState(currentPowerupPanelIdx, 0);
            }
        }

        this.currentPowerupPanelIdx = powerupLevel-1;
    }

    public void OnLifeChange(int life)
    {
        lifeText.text = life.ToString();
    }

    public void OnNameChange(string name)
    {
        nameText.text = name;
    }

    public void OnMetersChange(int presentHp)
    {
        //presentHp = player.GetComponent<PlayerBase>().hp;
        float fillMeter = (float)presentHp / maxHp;
        meterTrans.localScale = Vector3.right * fillMeter + Vector3.up + Vector3.forward;
    }

    public void OnScoreChange(int score)
    {
        scoreText.text = score.ToString("D7");
    }

    public void ResetAll()
    {
        for(int i =0; i< weaponStates.Length; i++)
        {
            ChangeWeaponPanelState(i, 0);
        }
    }
}
