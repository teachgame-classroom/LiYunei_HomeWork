using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_CollectItem : MonoBehaviour
{
    public int collectavleCounts = 10;
    public int pickupRadiusRange = 2;
    public int pickupRadiusSize = 4;
    private float pickupRadius = 5;

    public Material pickupMat;
    public Material normalMat;

    private int score;

    private GameObject playerPrefab;
    private GameObject collectablePrefabs;

    private GameObject player;
    private List<GameObject> collectables = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        playerPrefab = Resources.Load<GameObject>("Player");
        collectablePrefabs = Resources.Load<GameObject>("Collectable");

        SpawnPlayer();
        SpawnCollectables();

    }

    // Update is called once per frame
    void Update()
    {
        pickupRadius = Mathf.Sin(Time.time) * pickupRadiusRange + pickupRadiusSize;

        if (Input.GetKey(KeyCode.W))
        {
            if (player != null)
            {
                player.transform.position += player.transform.forward * 10 * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (player != null)
            {
                player.transform.position -= player.transform.forward * 10 * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (player != null)
            {
                player.transform.Rotate(0, -90 * Time.deltaTime, 0);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (player != null)
            {
                player.transform.Rotate(0, 90 * Time.deltaTime, 0);
            }
        }

        CheckPickup();
    }

    void CheckPickup()
    {
        bool pickup = Input.GetKeyDown(KeyCode.Space);

        float sqrPickupRaidus = pickupRadius * pickupRadius;

        for(int i = 0; i < collectables.Count; i++)
        {
            Vector3 collectableToPlayer = player.transform.position - collectables[i].transform.position;
            float sqrMag = collectableToPlayer.sqrMagnitude;

            if (sqrMag < sqrPickupRaidus)
            {
                if (pickup)
                {
                    if (collectables[i].activeSelf)
                    {
                        collectables[i].SetActive(false);
                        AddScore();
                    }
                }
                else
                {
                    collectables[i].GetComponent<MeshRenderer>().material = pickupMat;
                }
            }
            else
            {
                collectables[i].GetComponent<MeshRenderer>().material = normalMat;
            }
        }
    }

    void AddScore()
    {
        score += 1;
    }

    void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, Vector3.up, Quaternion.identity);
    }

    void SpawnCollectables()
    {
        for(int i = 0;i < collectavleCounts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * 25;
            Vector3 spawnPos = new Vector3(randomCircle.x, 1, randomCircle.y);

            GameObject collectable = Instantiate(collectablePrefabs,spawnPos,Quaternion.identity);

            collectables.Add(collectable);
        }
    }

    private void OnDrawGizmos()
    {
        if (player !=null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.transform.position,pickupRadius);
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("Score:{0}", score));
        GUILayout.EndVertical();
    }
}
