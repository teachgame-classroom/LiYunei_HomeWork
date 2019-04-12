using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float v;
    private float h;

    private Vector3 Vertical;
    private Vector3 Horizontal;

    public int speed = 5;

    private GameObject bulletPrefab;
    private List<GameObject> bullets;

    private Vector3 moveDirection;

    private GameObject explodePrefab;

    void Start()
    {
        bulletPrefab = Resources.Load<GameObject>("Bullet_1");
        bullets = new List<GameObject>();

        explodePrefab = Resources.Load<GameObject>("Explode");
    }

    void Update()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        Vertical = Vector3.up * v;
        Horizontal = Vector3.right * h;

        transform.Translate(Vertical *Time.deltaTime* speed +Horizontal *Time.deltaTime* speed, Space.World);

        if (Input.GetKeyDown(KeyCode.J))
        {
            bullets.Add(Instantiate(bulletPrefab, transform.position, Quaternion.identity));
        }
        if(bullets != null)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                moveDirection = bullets[i].transform.right;
                bullets[i].transform.Translate(moveDirection * Time.deltaTime * speed, Space.World);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(explodePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
