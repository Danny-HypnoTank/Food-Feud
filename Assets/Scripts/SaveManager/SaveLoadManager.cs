/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 27/10/2019
 * Last Modified: 27/10/2019
 */
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
{
    //Saves data
    public static void SavePlayerData(SaveData saveData)
    {
        BinaryFormatter binaryFormatt = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/PlayerData.rve", FileMode.Create);
        PlayerData data = new PlayerData(saveData);

        binaryFormatt.Serialize(fileStream, data);
        fileStream.Close();
    }
    //Deletes data
    public static void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.rve"))
        {
            File.Delete(Application.persistentDataPath + "/PlayerData.rve");
        }
    }

    //loads data
    public static int[] LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.rve"))
        {
            BinaryFormatter binaryFormatt = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/PlayerData.rve", FileMode.Open);
            PlayerData data = binaryFormatt.Deserialize(fileStream) as PlayerData;
            fileStream.Close();
            return data.settings;
        }
        else
        {
            Debug.LogError("File does not exist !");
            return null;
        }
    }

}
[System.Serializable]
public class PlayerData
{
    public int[] settings;
    public PlayerData(SaveData saveData)
    {
        settings = new int[2];
        settings[0] = saveData.MusicVal;
        settings[1] = saveData.SoundVal;
    }
}
