using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadonManager : MonoBehaviour
{
    /// <summary>
    /// 小队的敌人数量
    /// </summary>
    public int memberCount = 5;
    /// <summary>
    /// 小队的移动速度
    /// </summary>
    public float moveSpeed = 10;

    /// <summary>
    /// 移动路线的路径点
    /// </summary>
    private Transform[] waypoints;

    /// <summary>
    /// 小队全灭后掉落的物品Prefab
    /// </summary>
    private GameObject powerupPrefab;
    /// <summary>
    /// 敌人Prefab
    /// </summary>
    private GameObject[] enemyPrefabs;

    /// <summary>
    /// 小队中全部敌人
    /// </summary>
    private GameObject[] members;
    /// <summary>
    /// 每个敌人的当前移动路径点
    /// </summary>
    private int[] memberWaypointIdx;

    /// <summary>
    /// 存放玩家
    /// </summary>
    private GameObject Player;
    /// <summary>
    /// 小队成员是否已被激活
    /// </summary>
    private bool isMemberActivated;

    /// <summary>
    /// 激活小队成员的摄像机距离
    /// </summary>
    private float activeDistance;

    // Start is called before the first frame update
    void Start()
    {
        powerupPrefab = Resources.Load<GameObject>("Gradius/Prefabs/PowerUp");
        enemyPrefabs = Resources.LoadAll<GameObject>("Gradius/Prefabs/Enemies");

        members = new GameObject[memberCount];
        memberWaypointIdx = new int [memberCount];

        // 生成小队中的每个敌人
        for(int i = 0; i<memberCount; i++)
        {
            members[i] = Instantiate(enemyPrefabs[0], transform.position + Vector3.right * i*0.5f, Quaternion.identity);

            members[i].GetComponent<Enemy>().squadonManager = this;

            members[i].SetActive(false);
        }

        waypoints = new Transform[transform.childCount];

        for(int i =0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }

        Player = GameObject.Find("Vic Viper");

        activeDistance = Camera.main.orthographicSize * Camera.main.aspect + 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMemberActivated)
        {
            if (IsCameraCloseEnough())
            {
                ActivateMembers();
            }
        }
        else
        {
            //让每个小队成员沿路径点移动
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] != null)
                {
                    members[i].transform.position = MoveAlongPath(members[i].transform.position, i);
                }
            }
        }
    }

    /// <summary>
    /// 检查摄像机与生产点的距离
    /// </summary>
    /// <returns></returns>
    private bool IsCameraCloseEnough()
    {
        float CameraDistanceX = transform.position.x - Camera.main.transform.position.x;

        return CameraDistanceX < activeDistance;
    }

    private void ActivateMembers()
    {
        for(int i = 0; i< members.Length; i++)
        {
            members[i].SetActive(true);
        }

        isMemberActivated = true;
    }

    /// <summary>
    /// 计算单个小队成员沿路径移动的位置
    /// </summary>
    /// <param name="currentPosition"></param>当前位置
    /// <param name="memberIdx"></param>成员编号
    /// <returns></returns>
    private Vector3 MoveAlongPath(Vector3 currentPosition, int memberIdx)
    {
        Vector3 newPos = currentPosition;
        
        //获取要移动的小队成员的目标路径点
        int waypointIdx = memberWaypointIdx[memberIdx];

        newPos = Vector3.MoveTowards(currentPosition, waypoints[waypointIdx].position, moveSpeed * Time.deltaTime);
        
        //如果这一帧已经到达目标路径点
        if (newPos == waypoints[waypointIdx].position)
        {
            //如果已经到达最后一个路径点，路径点编号不在增加，停留在终点位置
            if(waypointIdx == waypoints.Length - 1)
            {
                Vector3 pos = newPos;
                OutDestroy(members[memberIdx]);
                return pos;
            }

            //如果后面还有路径点，路径点编号加一
            waypointIdx += 1;
            memberWaypointIdx[memberIdx] = waypointIdx;
        }

        return newPos;
    }

    void OutDestroy(GameObject member)
    {
        Destroy(member);
        memberCount--;
        if(memberCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnMenberDestroy(Vector3 diePosition)
    {
        Debug.Log(diePosition);

        memberCount--;

        if(memberCount <= 0)
        {
            Instantiate(powerupPrefab, diePosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if(waypoints == null)
        {
            waypoints = new Transform[transform.childCount];

            for(int i = 0; i<waypoints.Length; i++)
            {
                waypoints[i] = transform.GetChild(i);
            }
        }

        for(int i =0; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(waypoints[i].position, Vector3.one * 0.1f);

            if (i < waypoints.Length - 1)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}
