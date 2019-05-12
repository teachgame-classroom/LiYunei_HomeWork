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
            if(_instance == null)
            {
                _instance = GameObject.Find("Canvas").GetComponentInChildren<UI>();
            }
            return _instance;                        
        }
    }
    private static UI _instance;

    private GameObject player;
    private Transform meterTrans;
    private string[] weaponName = new string[] { "SpeedUp", "Missile", "Double", "Laser", "Option", "Barrier" };

    private Image[] speedUpImages;
    private Image[] missileImages;
    private Image[] doubleImages;
    private Image[] laserImages;
    private Image[] optionImages;
    private Image[] barrierImages;

    private int[] weaponStates = new int[6];
    private KeyCode[] testKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };

    private int currentPowerupPanelIdx = -1;
    private int testPowerup = 0;

    private int maxHp = 0;
    private int presentHp = 0;
    private int life = 0;

    void Start()
    {
        player = GameObject.Find("Player");
        maxHp = player.GetComponent<PlayerBase>().maxHp;
        meterTrans = transform.Find("StatusPanel/MaxHp/PresentHp");

        speedUpImages = GetWeaponImages(weaponName[0]);
        missileImages = GetWeaponImages(weaponName[1]);
        doubleImages = GetWeaponImages(weaponName[2]);
        laserImages = GetWeaponImages(weaponName[3]);
        optionImages = GetWeaponImages(weaponName[4]);
        barrierImages = GetWeaponImages(weaponName[5]);
    }

    void Update()
    {
        presentHp = player.GetComponent<PlayerBase>().hp;
        float fillMeter = (float)presentHp / maxHp;
        meterTrans.localScale = Vector3.right * fillMeter + Vector3.up + Vector3.forward;
        life = player.GetComponent<PlayerBase>().life;
        transform.Find("StatusPanel/Life").GetComponent<Text>().text =life.ToString();

        for(int i =0; i<testKeys.Length; i++)
        {
            if (Input.GetKeyDown(testKeys[i]))
            {
                weaponStates[i]++;
                if (weaponStates[i] > 3)
                {
                    weaponStates[i] = 0;
                }

                ChangeWeaponPanelState(i, weaponStates[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            testPowerup++;
            if(testPowerup > 6)
            {
                testPowerup = 0;
            }
            OnPowerupChanged(testPowerup);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnPowerup();
        }
    }

    public void OnPowerup()
    {
        ChangeWeaponPanelState(currentPowerupPanelIdx, 2);
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
}
