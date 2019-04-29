using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GradiusSave
{
    public int hiscore;
}

public class GameController : MonoBehaviour
{
    public static GameController instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.Find("GameController").GetComponent<GameController>();
            }
            return _instance;
        }
    }
    public static GameController _instance;

    public int score { get; private set; }
    public int highscore { get; private set; }

    void Start()
    {
        InitScore();
        UI.instance.OnScoreChange(score);
    }

    void Update()
    {
        
    }

    void InitScore()
    {
        score = 0;
    }

    public void AddScore(int amount)
    {
        score += amount;
        score = Mathf.Min(score, 9999999);
        if(score > highscore)
        {
            highscore = score;
        }
        UI.instance.OnScoreChange(score);
    }

    void saveHighScore()
    {
        GradiusSave save = new GradiusSave();
        save.hiscore = highscore;

        FileStream fs = File.Create(Application.persistentDataPath + "/" + "gradius.sav");

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, save);

        fs.Close();
    }

    void loadHighScore()
    {
        if (File.Exists(Application.persistentDataPath + "/" + "gradius.sav"))
        {
            FileStream fs = File.Open(Application.persistentDataPath + "/" + "gradius.sav", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            GradiusSave sav = (GradiusSave)bf.Deserialize(fs);
            highscore = sav.hiscore;

            fs.Close();
        }
    }
}
