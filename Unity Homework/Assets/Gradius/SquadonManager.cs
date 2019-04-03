using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadonManager : MonoBehaviour
{
    public float pointSize = 0.1f;

    /// <summary>
    /// 小队的敌人数量
    /// </summary>
    public int memberCount = 5;
    /// <summary>
    /// 小队的移动速度
    /// </summary>
    public float moveSpeed = 5;

    /// <summary>
    /// 移动路线的路径点
    /// </summary>
    private Transform[] waypoints;
    private int currentWaypointIdx = 0;

    /// <summary>
    /// 小队全灭后掉落的物品Prefab
    /// </summary>
    private GameObject powerupPrefab;
    /// <summary>
    /// 敌人Prefab
    /// </summary>
    private GameObject[] enemyPrefabs;

    /// <summary>
    /// 小队中的全部敌人
    /// </summary>
    private GameObject[] members;
    /// <summary>
    /// 每个敌人的当前移动路劲点
    /// </summary>
    private int[] memberWaypointIdx;

    // Start is called before the first frame update
    void Start()
    {
        //加载Prefab
        powerupPrefab = Resources.Load<GameObject>("Gradius/Prefabs/PowerUp");
        enemyPrefabs = Resources.LoadAll<GameObject>("Gradius/Prefabs/Enemies");

        members = new GameObject[memberCount];
        memberWaypointIdx = new int[memberCount];

        //生成小队中的敌人
        for(int i = 0; i<memberCount; i++)
        {
            members[i] = Instantiate(enemyPrefabs[0], transform.position+Vector3.right*i, Quaternion.identity);

            members[i].GetComponent<Enemy>().squadonManager = this;
        }

        //查找所有子对象，存放到waypoints数组，这些就是路径点
        waypoints = new Transform[transform.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < members.Length; i++)
        {
            if (members[i] != null)
            {
                members[i].transform.position = MoveAlongPath(members[i].transform.position, i);
            }
        }
    }
    /// <summary>
    /// 计算摸个小队成员沿路劲移动的位置
    /// </summary>
    /// <param name="currentPosition">当前位置</param>
    /// <param name="memberIdx">成员编号</param>
    /// <returns></returns>
    private Vector3 MoveAlongPath(Vector3 currentPosition, int memberIdx)
    {
        Vector3 newPos = currentPosition;
        
        //获取要移动的小队成员位的目标路径点
        int waypointIdx = memberWaypointIdx[memberIdx];

        newPos = Vector3.MoveTowards(currentPosition, waypoints[waypointIdx].position, moveSpeed * Time.deltaTime);

        if(newPos == waypoints[waypointIdx].position)
        {
            if (waypointIdx == waypoints.Length - 1)
            {
                return currentPosition;
            }

            waypointIdx += 1;
            memberWaypointIdx[memberIdx] = waypointIdx;
            
        }
        return newPos;
    }

    public void OnMenberDestroy(Vector3 diePosition)
    {
        memberCount--;

        if(memberCount <= 0)
        {
            Instantiate(powerupPrefab, diePosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null)
        {
            waypoints = new Transform[transform.childCount];

            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = transform.GetChild(i);
            }
        }

        for (int i = 0; i < waypoints.Length; i++)
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
