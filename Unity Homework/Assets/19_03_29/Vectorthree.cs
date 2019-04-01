using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Vectorthree : MonoBehaviour
{
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;

    public float pointSize = 0.2f;          //sphere radius

    public Color colorA = Color.red;
    public Color colorB = Color.green;
    public Color colorC = Color.cyan;

    public float XAxisLength=10;               //X direction length
    public float YAxisLength=10;               //Y direction length

    public Vector3 movePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movePoint = movePoint - Vector3.zero;

        Vector3 ab = b - a;
        Debug.Log(string.Format("The distance from A to B is {0}", ab.magnitude));

        Vector3 oc = c - Vector3.zero;
        Debug.Log(string.Format("The distance from C to origin is {0}", ab.magnitude));

        if (Input.GetKeyDown(KeyCode.A))
        {
            movePoint -= Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            movePoint += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            movePoint += Vector3.up;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            movePoint -= Vector3.up;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(Vector3.zero, pointSize);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, Vector3.right*XAxisLength);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, Vector3.up*YAxisLength);

        Gizmos.color = colorA;
        Gizmos.DrawSphere(a,pointSize);

        Gizmos.color = colorB;
        Gizmos.DrawSphere(b, pointSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(a,b);

        Gizmos.color = colorC;
        Gizmos.DrawSphere(c,pointSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Vector3.zero, c);

        Gizmos.color = Color.white;
        Vector3 ab = b - a;
        Gizmos.DrawLine(movePoint, movePoint + ab);
        //Gizmos.DrawLine(movePoint, Vector3.zero);

    }
}
