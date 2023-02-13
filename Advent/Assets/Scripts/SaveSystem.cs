using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static bool DoLoad { get; set; }

    public static void SaveGame(OverseerGlobalProcesses overseer, GameObject player, Clicker clicker, Browser browser, Miner miner)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(overseer, player, clicker, browser, miner);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/game.sav";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = bf.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }
}
