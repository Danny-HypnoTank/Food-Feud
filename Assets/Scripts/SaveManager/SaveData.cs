/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 27/10/2019
 * Last Modified: 27/10/2019
 */
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private int musicVal;
    private int soundVal;

    public int SoundVal { get => soundVal; set => soundVal = value; }
    public int MusicVal { get => musicVal; set => musicVal = value; }

    //save function saves all data
    public void Save()
    {
       soundVal =  (int)SoundManager.Instance.SoundVol;
        musicVal = (int)SoundManager.Instance.MusicVol;
        SaveLoadManager.SavePlayerData(this);
    }

    //load function loads all data
    public void Load()
    {
        //applying loaded values
        int[] loadedSettings = SaveLoadManager.LoadSettings();
        SoundManager.Instance.MusicVol = loadedSettings[0];
        SoundManager.Instance.SoundVol = loadedSettings[1];
    }
}
