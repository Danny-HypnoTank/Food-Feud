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
        data = this.gameObject.GetComponent<SaveData>();
        if (File.Exists(Application.persistentDataPath + "/PlayerData.rve"))
        {
           // SaveLoadManager.DeleteSave();
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
