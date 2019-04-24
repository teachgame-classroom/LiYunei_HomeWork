using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
    private bool isRecorded;
    private float before;
    private float after;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public StopWatch()
    {

    }

    public void Before()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            before = Time.realtimeSinceStartup;
            isRecorded = true;
            Debug.Log("Before" + before);
        }
    }

    public void After()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isRecorded)
        {
            isRecorded = false;
            after = Time.realtimeSinceStartup;
            Debug.Log("Time:" + (after - before) * 1000 + "ms");
        }
    }
}
