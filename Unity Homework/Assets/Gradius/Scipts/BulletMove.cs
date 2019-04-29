using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletMoveType { Normal, Missile, Laser, Barrier }

public class BulletMove : MonoBehaviour
{
    public BulletMoveType bulletMoveType;
    public float speed = 15;
    public bool isMissle;
    public bool isBarrier;
    public Vector3 moveDirection;

    private string[] stageLayeMask = new string[]{"Stage"};

    void Start()
    {
        if(moveDirection == Vector3.zero)
        {
            moveDirection = transform.right;
        }
    }

    void Update()
    {
        switch (bulletMoveType)
        {
            case (BulletMoveType)0:
                CameraClamp();
                break;
            case (BulletMoveType)1:
                if (GetGroundNormal() != Vector3.zero)
                {
                    transform.up = GetGroundNormal();
                    moveDirection = transform.right;
                }
                CameraClamp();
                break;
            case (BulletMoveType)2:
                CameraClamp();
                break;
            case (BulletMoveType)3:
                transform.RotateAround(transform.parent.position, Vector3.forward, speed * Time.deltaTime);
                break;
        }
    }

    void CameraClamp()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        Vector3 pos = transform.position;

        Vector3 camPos = Camera.main.transform.position;
        float left = camPos.x - Camera.main.orthographicSize * Camera.main.aspect - 1f;
        float right = camPos.x + Camera.main.orthographicSize * Camera.main.aspect + 1f;
        float top = camPos.y + Camera.main.orthographicSize + 0.5f;
        float bottom = camPos.y - Camera.main.orthographicSize - 0.5f;

        if (pos.x < left || pos.x > right || pos.y < bottom || pos.y > top)
        {
            gameObject.SetActive(false);
        }
    }

    Vector3 GetGroundNormal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - transform.up * 0.2f,-transform.up,0.2f,LayerMask.GetMask(stageLayeMask));

        if(hit.transform != null)
        {
            //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red, 1f);
            return hit.normal;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
