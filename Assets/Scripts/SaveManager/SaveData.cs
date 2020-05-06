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
    private float musicVal;
    private float soundVal;
    private float[] playerOneData;
    private float[] playerTwoData;
    private float[] playerThreeData;
    private float[] playerFourData;

    private int statsDash;

    private int statsPowerups;

    private int statsStuns;

    private int statsGames;



    //save function saves all data
    public void Save()
    {
        playerOneData = new float[4];
        playerTwoData = new float[4];
        playerThreeData = new float[4];
        playerFourData = new float[4];
        soundVal =  (float)SoundManager.Instance.SoundVol;
        musicVal = (float)SoundManager.Instance.MusicVol;
        //One
        playerOneData[0] = MedalManager.Instance.timesStunned[0];
        playerOneData[1] = MedalManager.Instance.timesDashed[0];
        playerOneData[2] = MedalManager.Instance.timesStunnedOthers[0];
        playerOneData[3] = MedalManager.Instance.timesPowersCollected[0];
        //Two
        playerTwoData[0] = MedalManager.Instance.timesStunned[1];
        playerTwoData[1] = MedalManager.Instance.timesDashed[1];
        playerTwoData[2] = MedalManager.Instance.timesStunnedOthers[1];
        playerTwoData[3] = MedalManager.Instance.timesPowersCollected[1];
        //Three
        playerThreeData[0] = MedalManager.Instance.timesStunned[2];
        playerThreeData[1] = MedalManager.Instance.timesDashed[2];
        playerThreeData[2] = MedalManager.Instance.timesStunnedOthers[2];
        playerThreeData[3] = MedalManager.Instance.timesPowersCollected[2];
        //Four
        playerFourData[0] = MedalManager.Instance.timesStunned[3];
        playerFourData[1] = MedalManager.Instance.timesDashed[3];
        playerFourData[2] = MedalManager.Instance.timesStunnedOthers[3];
        playerFourData[3] = MedalManager.Instance.timesPowersCollected[3];
        statsDash = MedalManager.Instance.statsDash;
        statsPowerups = MedalManager.Instance.statsPowerups;
        statsStuns = MedalManager.Instance.statsStuns;
        StatsGames = MedalManager.Instance.statsGames;
        SaveLoadManager.SavePlayerData(this);
    }
    /*public List<int> timesStunned;

    public List<int> timesDashed;

    public List<int> timesStunnedOthers;

    public List<int> timesPowersCollected;*/

    //load function loads all data
    public void Load()
    {
        //applying loaded values
        try
        {
            float[] loadedSettings = SaveLoadManager.LoadSettings();
            SoundManager.Instance.MusicVol = loadedSettings[0];
            SoundManager.Instance.SoundVol = loadedSettings[1];
            //Player one
            MedalManager.Instance.timesStunned[0] = (int)loadedSettings[2];
            MedalManager.Instance.timesDashed[0] = (int)loadedSettings[3];
            MedalManager.Instance.timesStunnedOthers[0] = (int)loadedSettings[4];
            MedalManager.Instance.timesPowersCollected[0] = (int)loadedSettings[5];
            //Player Two
            MedalManager.Instance.timesStunned[1] = (int)loadedSettings[6];
            MedalManager.Instance.timesDashed[1] = (int)loadedSettings[7];
            MedalManager.Instance.timesStunnedOthers[1] = (int)loadedSettings[8];
            MedalManager.Instance.timesPowersCollected[1] = (int)loadedSettings[9];
            //Player Three
            MedalManager.Instance.timesStunned[2] = (int)loadedSettings[10];
            MedalManager.Instance.timesDashed[2] = (int)loadedSettings[11];
            MedalManager.Instance.timesStunnedOthers[2] = (int)loadedSettings[12];
            MedalManager.Instance.timesPowersCollected[2] = (int)loadedSettings[13];
            //Player Four
            MedalManager.Instance.timesStunned[3] = (int)loadedSettings[14];
            MedalManager.Instance.timesDashed[3] = (int)loadedSettings[15];
            MedalManager.Instance.timesStunnedOthers[3] = (int)loadedSettings[16];
            MedalManager.Instance.timesPowersCollected[3] = (int)loadedSettings[17];
            MedalManager.Instance.statsStuns = (int)loadedSettings[18];
            MedalManager.Instance.statsPowerups = (int)loadedSettings[19];
            MedalManager.Instance.statsStuns = (int)loadedSettings[20];
            MedalManager.Instance.statsGames = (int)loadedSettings[21];
        }
        catch
        {
            Debug.LogWarning("Failed to load no data has been loaded!");
        }

    }

    public float SoundVal { get => soundVal; set => soundVal = value; }
    public float MusicVal { get => musicVal; set => musicVal = value; }
    public float[] PlayerOneData { get => playerOneData; set => playerOneData = value; }
    public float[] PlayerTwoData { get => playerTwoData; set => playerTwoData = value; }
    public float[] PlayerThreeData { get => playerThreeData; set => playerThreeData = value; }
    public float[] PlayerFourData { get => playerFourData; set => playerFourData = value; }
    public int StatsDash { get => statsDash; set => statsDash = value; }
    public int StatsPowerups { get => statsPowerups; set => statsPowerups = value; }
    public int StatsStuns { get => statsStuns; set => statsStuns = value; }
    public int StatsGames { get => statsGames; set => statsGames = value; }
}
