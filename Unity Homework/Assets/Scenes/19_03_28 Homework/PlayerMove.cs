using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody body;

    public int score;
    public int level;

    public float speed = 10;
    public float jumpWeight = 100;

    public bool colloider = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        body.AddForce((Vector3.forward * vertical + Vector3.right * horizontal) * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(Vector3.up*jumpWeight);
        }
        
    }

    private void AddScore()
    {
        score += 1;

        Debug.Log("+1");

        if(score % 5 == 0&&score != 0)
        {
            level += 1;
            Debug.Log("LevelUp");
        }
    }

    private bool OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Score")
        {
            AddScore();

            return colloider = true;
        }
        else
        {
            return colloider = false;
        }
       
    }
}
