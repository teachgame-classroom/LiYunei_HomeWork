using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionSaveData
{
    //public Vector3 position;

    public float x;
    public float y;
    public float z;

    public float yAngle;
}

public class PlayerWithSaveLoad : MonoBehaviour
{
    const string saveFileName = "position.sav";

    private GameObject playerPrefab;
    private GameObject player;

    private string savePath;

    // Start is called before the first frame update
    void Start()
    {
        playerPrefab = Resources.Load<GameObject>("player");
        savePath = Application.persistentDataPath + "/" + saveFileName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawnPos = TryLoadPlayerPosition();
            float yAngle = TryLoadPlayerAngle();

            Quaternion rot = Quaternion.Euler(0, yAngle, 0);

            player = (GameObject)Instantiate(playerPrefab,spawnPos,rot);
        }

        if (Input.GetKey(KeyCode.W))
        {
            if(player != null)
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            Save();
        }
    }

    Vector3 TryLoadPlayerPosition()
    {
        object posSave = SaveLoadManager.Load(savePath);

        if (posSave != null)
        {
            float x = (posSave as PositionSaveData).x;
            float y = (posSave as PositionSaveData).y;
            float z = (posSave as PositionSaveData).z;

            return new Vector3(x, y, z);
        }
        else
        {
            return Vector3.up;
        }
    }

    float TryLoadPlayerAngle()
    {
        object posSave = SaveLoadManager.Load(savePath);

        if(posSave != null)
        {
            float yAngle = (posSave as PositionSaveData).yAngle;

            return yAngle;
        }
        else
        {
            return 0;
        }
    }

    void Save()
    {
        PositionSaveData positionSaveData = new PositionSaveData();

        positionSaveData.x = player.transform.position.x;
        positionSaveData.y = player.transform.position.y;
        positionSaveData.z = player.transform.position.z;
        positionSaveData.yAngle = player.transform.rotation.eulerAngles.y;

        SaveLoadManager.Save(positionSaveData, savePath);
    }

}
