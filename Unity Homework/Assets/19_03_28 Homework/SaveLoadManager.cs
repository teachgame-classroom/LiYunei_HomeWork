using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager
{
    public static void Save(object data, string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(path);

        bf.Serialize(fs,data);

        fs.Close();
    }

    public static object Load(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(path, FileMode.Open);

            object ret = bf.Deserialize(fs);

            fs.Close();

            return ret;
        }
        else
        {
            return null;
        }

    }
}
