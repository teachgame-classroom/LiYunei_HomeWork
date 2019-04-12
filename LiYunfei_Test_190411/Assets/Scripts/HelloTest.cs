using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloTest : MonoBehaviour
{
    public int myInt = 255;

    private string myStr;

    public Vector3 myVec = new Vector3(30, 40, 50);

    private int[] mayArray = new int[100];

    void Start()
    {
        myStr = "Hello unity";

        for(int i =0; i<mayArray.Length; i++)
        {
            mayArray[i] = i + 1;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(Sum1to100());
        }
    }

    private int Sum1to100()
    {
        int sum = 0;

        for (int i = 0; i < mayArray.Length; i++)
        {
            sum += mayArray[i];
        }

        return sum;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(myVec, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(myVec, transform.position);
    }
}
