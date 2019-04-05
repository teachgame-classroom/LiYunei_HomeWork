using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicViper : MonoBehaviour
{
    //private float radius = 1;
    public float speed = 10;
    public float shootAngle = 30f;

    private Transform shotPosTrans;

    private GameObject[] bullets;
    private GameObject targetIconPrefab;
    private GameObject targetIcon;

    private float lastFireTime = 0;
    private float fireInterval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Gradius/Prefabs/Bullets");
        targetIconPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/TargetIcon");
        shotPosTrans = transform.Find("ShotPos");

        Vector3 v = MouseTarget();
        targetIconPrefab.transform.position = v;
        targetIcon = Instantiate(targetIconPrefab, targetIconPrefab.transform.position, Quaternion.identity);
    }

 

    // Update is called once per frame
    void Update()
    {
        //radius = (Mathf.Sin(Time.time*4) * 0.25f + 1)*0.5f; 

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += (Vector3.right * h+Vector3.up * v) * speed * Time.deltaTime;

        targetIcon.transform.position = MouseTarget();

        if (Time.time - lastFireTime > fireInterval)
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }

    private static Vector3 MouseTarget()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v = Vector3.Scale(v, Vector3.right + Vector3.up);
        return v;
    }

    float GetAngle(Vector3 direction)
    {
        float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
        return angle;
    }

    void Shoot()
    {
        Vector3 direction = (MouseTarget() - shotPosTrans.position).normalized;

        if(GetAngle(direction) > shootAngle)
        {
            direction = Quaternion.Euler(0, 0, shootAngle) * Vector3.right;
        }

        if(GetAngle(direction) < -shootAngle)
        {
            direction = Quaternion.Euler(0, 0, -shootAngle) * Vector3.right;
        }

        GameObject bullet = Instantiate(bullets[0], shotPosTrans.position, Quaternion.identity);
        bullet.transform.right = direction;
        bullet.GetComponent<BulletMove>().moveDirection = direction;
    }

    private void OnDrawGizmos()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v = Vector3.Scale(v, Vector3.right + Vector3.up);
        Vector3 direction = v - transform.position;

        //Gizmos.DrawLine(transform.position, transform.position + direction);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(v, radius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Is collider " + collision.gameObject.name);
    }

}
