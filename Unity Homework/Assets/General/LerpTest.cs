using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public float pointSize = 0.1f;
    //public float moveSpeed = 5;

    public Vector3 a;
    public Vector3 b;

    public float t = 0.5f;

    private float currentPathLength =0;
    private float currentPathDesireTime = 0;

    private Transform[] waypoints;
    private int currentWaypointIdx = 0;

    private GameObject cube;
    public Vector3 c;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new Transform[transform.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }

        //cube = GameObject.Find("Cube");
        //cube.transform.position = waypoints[currentWaypointIdx].position;
        currentWaypointIdx += 1;

        currentPathLength = Vector3.Distance(waypoints[currentWaypointIdx].position, waypoints[currentWaypointIdx-1].position);

        //currentPathDesireTime = currentPathLength / moveSpeed;

        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        FollowThePath();
        //cube.transform.position = MoveAlongPath(cube.transform.position);

        /*
        if(cube.transform.position == waypoints[currentWaypointIdx].position)
        {
            if (currentWaypointIdx == waypoints.Length - 1)
            {
                return;
            }
            currentWaypointIdx += 1;
            t = 0;
        }

        Vector3 a = waypoints[currentWaypointIdx - 1].position;
        Vector3 b = waypoints[currentWaypointIdx].position;

        t += Time.deltaTime/currentPathDesireTime;

        Vector3 newPos = Vector3.Lerp(a, b, t);

        cube.transform.position = newPos;
        */
    }

    private Vector3 MoveAlongPath(Vector3 currentPosition)
    {
        if (currentPosition == waypoints[currentWaypointIdx].position)
        {
            if (currentWaypointIdx == waypoints.Length - 1)
            {
                return currentPosition;
            }
            currentWaypointIdx += 1;
            t = 0;
        }

        Vector3 a = waypoints[currentWaypointIdx - 1].position;
        Vector3 b = waypoints[currentWaypointIdx].position;

        t += Time.deltaTime / currentPathDesireTime;

        Vector3 newPos = Vector3.Lerp(a, b, t);

        return newPos;
    }

    void FollowThePath()
    {
        t += Time.deltaTime;
        c = Vector3.Lerp(a, b, t);

        cube.transform.position = c;

    }

    private void OnDrawGizmos()
    {
        if(waypoints == null)
        {
            waypoints = new Transform[transform.childCount];

            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = transform.GetChild(i);
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(a,pointSize );

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(b, pointSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(a, b);

        //c = Vector3.Lerp(a, b, t);
        c = Vector3.MoveTowards(a, b, t);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(c, pointSize);



        for(int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(waypoints[i].position, Vector3.one * pointSize);

            if (i < waypoints.Length - 1)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}
