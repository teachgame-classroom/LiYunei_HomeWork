using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class UseFileInUnity
{
    public static void Save(object data, string path)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(path);

        bf.Serialize(file, data);
        file.Close();
    }


    public static object Load(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream readStream = File.Open(path, FileMode.Open);
        object ret = (SaveData)bf.Deserialize(readStream);

        readStream.Close();

        return ret;
    }
}
