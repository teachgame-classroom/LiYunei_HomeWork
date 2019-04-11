using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public bool isBarrier = false;
    public bool isLaser =false;
    public int laserCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        Vector3 camPos = Camera.main.transform.position;
        float left = camPos.x - Camera.main.orthographicSize * Camera.main.aspect -1f;
        float right = camPos.x + Camera.main.orthographicSize * Camera.main.aspect + 1f;
        float top = camPos.y + Camera.main.orthographicSize +0.5f;
        float bottom = camPos.y - Camera.main.orthographicSize - 0.5f;

        if (pos.x < left || pos.x > right || pos.y < bottom || pos.y > top)
        {
            if(isBarrier == false)
            {
                Destroy(gameObject);
            }
        }
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
                if(isBarrier == false)
                {
                    Destroy(gameObject);
                }
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
