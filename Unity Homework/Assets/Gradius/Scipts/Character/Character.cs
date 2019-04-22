using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public bool drawMovetrail = false;

    protected List<Vector3> tracks = new List<Vector3>();
    protected float lastRecordTime;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        InitCharacter();
    }

    protected virtual void Update()
    {
        Move();

        if (drawMovetrail)
        {
            RecordMoveTrail();
        }
    }

    protected virtual void InitCharacter()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Move()
    {
        Move(Vector3.left);
    }

    protected void Move(Vector3 moveDirection)
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    protected void RecordMoveTrail()
    {
        if (Time.time - lastRecordTime > 0.1f)
        {
            tracks.Add(transform.position);
            if (tracks.Count > 48)
            {
                tracks.RemoveAt(0);
            }
            lastRecordTime = Time.time;
        }
    }

    protected void OnDrawGizmos()
    {
        if (drawMovetrail)
        {
            for(int i = 0; i < tracks.Count; i++)
            {
                Gizmos.DrawSphere(tracks[i], 0.1f);
            }
        }
    }
}
