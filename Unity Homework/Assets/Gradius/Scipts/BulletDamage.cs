using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public bool isLaser =false;
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
        Debug.Log("The bullet hit " + collision.gameObject.name);

        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy != null)
        {
            enemy.Hurt(1);
            if (isLaser == false)
            {
                Destroy(gameObject);
            }
            else
            {
                laserCount--;
                if(laserCount == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}
