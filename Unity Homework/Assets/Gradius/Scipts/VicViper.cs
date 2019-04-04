﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicViper : MonoBehaviour
{
    private float radius = 1;
    public float speed = 10;

    private Transform shotPosTrans;

    private GameObject[] bullets;

    private float lastFireTime = 0;
    private float fireInterval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        bullets = Resources.LoadAll<GameObject>("Gradius/Prefabs/Bullets");
        shotPosTrans = transform.Find("ShotPos");
    }

    // Update is called once per frame
    void Update()
    {
        radius = (Mathf.Sin(Time.time*4) * 0.25f + 1)*0.5f; 

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += (Vector3.right * h+Vector3.up * v) * speed * Time.deltaTime;

        if (Time.time - lastFireTime > fireInterval)
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }

    void Shoot()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v = Vector3.Scale(v, Vector3.right + Vector3.up);
        Vector3 direction = v - transform.position;

        GameObject bullet = Instantiate(bullets[0], shotPosTrans.position, Quaternion.identity);
        bullet.transform.right = direction.normalized;
    }

    private void OnDrawGizmos()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v = Vector3.Scale(v, Vector3.right + Vector3.up);
        Vector3 direction = v - transform.position;

        //Gizmos.DrawLine(transform.position, transform.position + direction);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(v, radius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Is collider " + collision.gameObject.name);
    }

}