using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType {Normal,Missile,Laser,Barrier }

public class BulletDamage : MonoBehaviour
{
    public BulletType bulletType;
    public int damage = 1;
    public int laserCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if((int)bulletType != 3)
        {
            if (enemy != null)
            {
                if(transform.tag != "EnemyBullet")
                {
                    enemy.Hurt(damage);

                }
                if ((int)bulletType == 2)
                {
                    laserCount--;
                    if (laserCount == 0)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.tag == "EnemyBullet")
            {
                Destroy(collision.gameObject);
            }
            if (enemy != null)
            {
                enemy.Hurt(damage);
            }
        }
    }

}
