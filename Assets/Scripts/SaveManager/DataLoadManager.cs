/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 27/10/2019
 * Last Modified: 27/10/2019
 */
using UnityEngine;
using System.IO;

public class DataLoadManager : MonoBehaviour
{
    private SaveData data;
    private void Start()
    {
        try
        {
            data = GameObject.Find("MedalManager").GetComponent<SaveData>();
        }
        catch
        {
            data = FindObjectOfType<SaveData>();
        }
        if (File.Exists(Application.persistentDataPath + "/PlayerData.cst"))
        {
            data.Load();
            data.Save();
        }
        else
        {
            NewSaveFile();
        }
    }

    private void NewSaveFile()
    {
        SoundManager.Instance.SoundVol = 1;
        SoundManager.Instance.MusicVol = 1;
        SoundManager.Instance.MasterVol = 1;
        data.Save();
    }

}
