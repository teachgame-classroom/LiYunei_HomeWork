using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody body;

    public int score;
    public int level;

    public float speed = 10;
    public float jumpWeight = 1;

    private bool isOnGround = true;

    string savePath;


    // Start is called before the first frame update
    void Start()
    {

        body = GetComponent<Rigidbody>();
        savePath = Application.persistentDataPath + "/" + "score.sav";
        
        SaveData data = (SaveData)UseFileInUnity.Load(savePath);

        score = data.info.score;
        level = data.info.level;

        Debug.Log("Score:" +score +  "Level:" + level);


    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        body.AddForce((Vector3.forward * vertical + Vector3.right * horizontal) * speed);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                body.transform.position += Vector3.up * jumpWeight;
                isOnGround = false;
            }          
        }
        
        
    }

    private void AddScore()
    {
        score += 1;

        Debug.Log("+1");

        if (score % 5 == 0 && score != 0)
        {
            level += 1;
            Debug.Log("LevelUp");
        }

        Save();
    }

    private void Save()
    {
        SaveData data = new SaveData();
        data.info = new ScoreInfo(score, level);

        UseFileInUnity.Save(data, savePath);
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Score")
        {
            AddScore();
            isOnGround = true;
        }

        if (other.tag == "Ground")
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }
}
