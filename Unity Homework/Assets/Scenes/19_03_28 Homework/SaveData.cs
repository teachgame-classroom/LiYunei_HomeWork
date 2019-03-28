using System.Collections;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public ScoreInfo[] infos;
}

[System.Serializable]
public class ScoreInfo
{
    public int score;
    public int level;

    public ScoreInfo(string infoString)
    {

       string[] info = ExtractScoreInfo(infoString);

        this.score = int.Parse(info[0]);
        this.level = int.Parse(info[1]);
    }

    public ScoreInfo(int score, int level)
    {
        this.score = score;
        this.level = level;
    }


    string[] ExtractScoreInfo(string scoreString)
    {
        string[] ret = scoreString.Split(',');
        return ret;
    }
}
