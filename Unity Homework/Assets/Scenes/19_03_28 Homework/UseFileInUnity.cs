using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class UseFileInUnity : MonoBehaviour
{
    public SaveData data;

    private PlayerMove player;

    string savePath;
    string[] lines;


    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.persistentDataPath + "/" + "score.txt";

        if (File.Exists(savePath))
        {
            lines = File.ReadAllLines(savePath);

            ScoreInfo[] infos = new ScoreInfo[lines.Length];        //生成多少infos数组

            for (int i = 0; i < lines.Length; i++)
            {
                infos[i] = new ScoreInfo(lines[i]);                 //把每一行放进infos里拆开
            }

            SaveData data = new SaveData();
            data.infos = infos;                                     //把更新后的infos放到score里

            SaveData Data = (SaveData)Load(savePath);
        }
        else
        {
            Save(data, savePath);
        }
    }


    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        PlayerMove playerMove = player.GetComponent<PlayerMove>();

        if (playerMove.colloider)
        {
            data.infos[0] = new ScoreInfo(playerMove.score, playerMove.level);
            Save(data, savePath);
        }
    }

    public void Save(object data, string path)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(path);

        bf.Serialize(file, data);
        file.Close();
    }


    public object Load(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream readStream = File.Open(path, FileMode.Open);
        object ret = (SaveData)bf.Deserialize(readStream);

        readStream.Close();

        return ret;
    }
}
