using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType {Normal,Missile,Laser,Barrier }

public class BulletDamage : MonoBehaviour
{
    public BulletType bulletType;
    public int damage = 1;
    public int laserCount = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(bulletType == (BulletType)3)
        {
            if(collision.tag == "EnemyBullet")
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

    public void OnHit()
    {
        switch (bulletType)
        {
            case (BulletType)0:
                //Destroy(gameObject);
                gameObject.SetActive(false);
                break;
            case (BulletType)1:
                gameObject.SetActive(false);
                //Destroy(gameObject);
                break;
            case (BulletType)2:
                laserCount--;
                if(laserCount == 0)
                {
                    gameObject.SetActive(false);
                    //Destroy(gameObject);
                }
                break;
            case (BulletType)3:
                break;
        }
    }
}
