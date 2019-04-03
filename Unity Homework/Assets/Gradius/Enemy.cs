using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed = 5;
    private int hp = 1;
    private GameObject explosionPrefab;

    /// <summary>
    /// 该敌人所属小队，敌人生成时由小队脚本指定
    /// </summary>
    public SquadonManager squadonManager;

    // Start is called before the first frame update
    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Gradius/Prefabs/Effects/Explosion_Red");
    }

    // Update is called once per frame
    void Update()
    {

        //transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }

    public void Hurt(int damage)
    {
        hp -= damage;

        if(hp<= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        squadonManager.OnMenberDestroy(transform.position);
        Instantiate(explosionPrefab, transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
