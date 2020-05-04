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
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/PlayerData.cst", FileMode.Create);
        PlayerData data = new PlayerData(saveData);

        binaryFormatt.Serialize(fileStream, data);
        fileStream.Close();
    }
    //Deletes data
    public static void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.cst"))
        {
            File.Delete(Application.persistentDataPath + "/PlayerData.cst");
        }
    }

    //loads data
    public static float[] LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.cst"))
        {
            BinaryFormatter binaryFormatt = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/PlayerData.cst", FileMode.Open);
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
    public float[] settings;
    // public int
    public PlayerData(SaveData saveData)
    {
        //size set
        settings = new float[18];  //2,6,10,14
        //Assign Values
        settings[0] = saveData.MusicVal;
        settings[1] = saveData.SoundVal;
        //One
        settings[2] = saveData.PlayerOneData[0];
        settings[3] = saveData.PlayerOneData[1];
        settings[4] = saveData.PlayerOneData[2];
        settings[5] = saveData.PlayerOneData[3];
        //Two
        settings[6] = saveData.PlayerTwoData[0];
        settings[7] = saveData.PlayerTwoData[1];
        settings[8] = saveData.PlayerTwoData[2];
        settings[9] = saveData.PlayerTwoData[3];
        //Three
        settings[10] = saveData.PlayerThreeData[0];
        settings[11] = saveData.PlayerThreeData[1];
        settings[12] = saveData.PlayerThreeData[2];
        settings[13] = saveData.PlayerThreeData[3];
        //Four
        settings[14] = saveData.PlayerFourData[0];
        settings[15] = saveData.PlayerFourData[1];
        settings[16] = saveData.PlayerFourData[2];
        settings[17] = saveData.PlayerFourData[3];
    }
}
