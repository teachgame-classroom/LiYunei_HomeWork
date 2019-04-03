using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicViper : MonoBehaviour
{
    //private float radius = 1;
    public float speed = 10;

    private Transform shotPosTrans;

    private GameObject moveUp;

    private GameObject iconPrefab;
    private GameObject icon;

    private GameObject[] bullets;

    private float lastFireTime = 0;
    private float fireInterval = 0.5f;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Gradius/Prefabs/Bullets");
        iconPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/TargetLocation");
        shotPosTrans = transform.Find("ShotPos");

        anim = GetComponent<Animator>();

        TargetLocation();
    }

    // Update is called once per frame
    void Update()
    {
        //radius = (Mathf.Sin(Time.time*4) * 0.25f + 1)*0.5f; 

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(v > 0.5f)
        {
            anim.SetInteger("MoveVertical", 1);
        }
        else if(v< -0.5f)
        {
            anim.SetInteger("MoveVertical", 2);
        }
        else
        {
            anim.SetInteger("MoveVertical", 0);
        }

        transform.position += (Vector3.right * h+Vector3.up * v) * speed * Time.deltaTime;

        icon.transform.position = MousePos();

        if (Time.time - lastFireTime > fireInterval)
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }

    void Shoot()
    {
        Vector3 direction = MousePos()- shotPosTrans.position;

        GameObject bullet = Instantiate(bullets[0], shotPosTrans.position, Quaternion.identity);
        bullet.transform.right = direction.normalized;
    }

    private void TargetLocation()
    {
        icon = Instantiate(iconPrefab, MousePos(), Quaternion.identity);
    }

    private static Vector3 MousePos()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v = Vector3.Scale(v, Vector3.right + Vector3.up);
        return v;
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
