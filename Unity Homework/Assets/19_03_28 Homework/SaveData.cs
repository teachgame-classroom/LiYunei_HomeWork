using System.Collections;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public ScoreInfo info;
}

[System.Serializable]
public class ScoreInfo
{
    public int score;
    public int level;

    public float x;
    public float y;
    public float z;

    public ScoreInfo(int score, int level)
    {
        this.score = score;
        this.level = level;
    }

    public ScoreInfo(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
