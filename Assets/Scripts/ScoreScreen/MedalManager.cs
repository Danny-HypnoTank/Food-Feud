using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;
public class MedalManager : MonoBehaviour
{
    private static MedalManager _instance;
    private SaveData saveData;

    public static MedalManager Instance { get { return _instance; } }

    public List<int> timesStunned;

    public List<int> timesDashed;

    public List<int> timesStunnedOthers;

    public List<int> timesPowersCollected;

    public GameObject GotStunnedMedal;

    public GameObject StunnedOthersMedal;

    public GameObject MostDashesMedal;

    public GameObject MostPowersCollected;

    public GameObject UntouchableMedal;

    public List<int> totalMedalCounts = new List<int>();

    public List<UnityEngine.UI.Text> medalCounts;

    public List<UnityEngine.UI.Text> bonusStatCounts;

    public int statsDash;

    public int statsPowerups;

    public int statsStuns;

    public int statsGames;



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            CountMedals();
        }
    }

    private void Awake()
    {
        saveData = this.gameObject.GetComponent<SaveData>();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void CountMedals()
    {
        int totaltimesStunned = 0, totaltimesDashed = 0, totaltimesStunnedOthers = 0, totaltimesPowersCollected = 0;
        Debug.Log(timesStunned[0]);
        for (int i = 0; i < timesStunned.Count; i++)
        {
            totaltimesStunned += timesStunned[i];
            totaltimesDashed += timesDashed[i];
            totaltimesStunnedOthers += timesStunnedOthers[i];
            totaltimesPowersCollected += timesPowersCollected[i];
        }
        Debug.Log(totaltimesStunned);
        totalMedalCounts[0] = totaltimesStunned;
        totalMedalCounts[1] = totaltimesDashed;
        totalMedalCounts[2] = totaltimesStunnedOthers;
        totalMedalCounts[3] = totaltimesPowersCollected;
        medalCounts[0].text = "Most stunned others: " + totalMedalCounts[0].ToString();
        medalCounts[1].text = "Most dashes: " + totalMedalCounts[1].ToString();
        //bonusStatCounts[2].text = "Games played:  " + totalGamesPlayed.ToString();
        medalCounts[2].text = "Most Stuns " + totalMedalCounts[2].ToString();
        medalCounts[3].text = "Most powerups: " + totalMedalCounts[3].ToString();
    }

    void CountStats()
    {
        bonusStatCounts[0].text = "Total stuns: " + statsStuns;
        bonusStatCounts[1].text = "Total dashes: " + statsDash;
        bonusStatCounts[2].text = "Total games " + statsGames;
        bonusStatCounts[3].text = "Total powerups: " + statsPowerups;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "NewMainMenu")
        {
            CountMedals();
            CountStats();
        }
        else if(scene.name == "BinEndRoundScene" || scene.name == "EndRoundScene" || scene.name == "SinkEndRoundScene")
        {
            statsGames++;
        }
    }

    private void Start()
    {
        StartCoroutine("LateStart", 1);
        if (SceneManager.GetActiveScene().name == "NewMainMenu")
        {
            StartCoroutine("Delay");
        }

    }

    public int OnPlayerStunned(int i)
    {

        timesStunned[i - 1] += 1;
        statsStuns++;
        return i;
    }

    public int OnPlayerDash(int i)
    {

        timesDashed[i - 1] += 1;

        statsDash++;

        return i;
    }

    public int OnOtherPlayerStunned(int i)
    {

        timesStunnedOthers[i - 1] += 1;
        return i;
    }

    public int OnCollected(int i)
    {

        timesPowersCollected[i - 1] += 1;

        statsPowerups++;

        return i;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        CountMedals();
    }

    IEnumerator LateStart(float wait)
    {
        yield return new WaitForSeconds(wait);

        foreach (DazeState d in FindObjectsOfType<DazeState>())
        {
            d.stunEvent += OnPlayerStunned;
        }

        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            p.dashEvent += OnPlayerDash;
        }

        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            p.stunOtherEvent += OnOtherPlayerStunned;
        }

        foreach (PlayerController p in FindObjectsOfType<PlayerController>())
        {
            p.collectEvent += OnCollected;
        }

        //FindObjectOfType<DazeState>().stunEvent += OnPlayerStunned;
    }

    public int GetTopStunned()
    {
        int max = 0;
        int current = timesStunned[0];
        for (int i = 0; i < timesStunned.Count; i++)
        {
            if (timesStunned[i] > max)
            {
                max = timesStunned[i];
                current = i;
            }
        }
        totalMedalCounts[0] += 1;

        
        return current;
    }

    public int GetUntouchable()
    {
        int max = 0;
        int current = 0;
        for (int i = 0; i < timesStunned.Count; i++)
        {
            if (timesStunned[i] == 0)
            {
                current = i;
            }
        }
        return current;
    }

    public int GetTopStunOther()
    {
        int max = 0;
        int current = 0;
        for (int i = 0; i < timesStunnedOthers.Count; i++)
        {
            if (timesStunnedOthers[i] > max)
            {
                max = timesStunnedOthers[i];
                current = i;
            }
        }
        totalMedalCounts[1] += 1;

        return current;
    }

    public int GetTopDashes()
    {
        int max = 0;
        int current = 0;
        for (int i = 0; i < timesDashed.Count; i++)
        {
            if (timesDashed[i] > max)
            {
                max = timesDashed[i];
                current = i;
            }
        }
        totalMedalCounts[2] += 1;
        return current;
    }

    public int GetTopPowerPickup()
    {
        int max = 0;
        int current = 0;
        for (int i = 0; i < timesPowersCollected.Count; i++)
        {
            if (timesPowersCollected[i] > max)
            {
                max = timesPowersCollected[i];
                current = i;
            }
        }
        totalMedalCounts[3] += 1;

        return current;
    }

    public void SpawnMedal(Vector3 pos, int whichMedal)
    {

        switch(whichMedal)
        {
            case (0):
                {
                    GameObject _medal1 = Instantiate(GotStunnedMedal, pos + (Vector3.up * 14), Quaternion.Euler(0, -90, 90));
                    _medal1.transform.localScale = new Vector3(5, 5, 5);
                    break;
                }
            case (1):
                {
                    GameObject _medal2 = Instantiate(StunnedOthersMedal, pos + (Vector3.up * 14), Quaternion.Euler(0, -90, 90));
                    _medal2.transform.localScale = new Vector3(5, 5, 5);
                    break;
                }
            case (2):
                {
                    GameObject _medal3 = Instantiate(MostDashesMedal, pos + (Vector3.up * 14), Quaternion.Euler(0, -90, 90));
                    _medal3.transform.localScale = new Vector3(5, 5, 5);
                    break;
                }
            case (3):
                {
                    GameObject _medal4 = Instantiate(MostPowersCollected, pos + (Vector3.up * 14), Quaternion.Euler(0, -90, 90));
                    _medal4.transform.localScale = new Vector3(5, 5, 5);
                    break;
                }
            case (4):
                {
                    GameObject _medal5 = Instantiate(UntouchableMedal, pos + (Vector3.up * 14), Quaternion.Euler(0, -90, 90));
                    _medal5.transform.localScale = new Vector3(5, 5, 5);
                    break;
                }
        } 
    }

    public void WriteMedalSaveFile()
    {
        try
        {
            saveData.Save();
        }
        catch(Exception e)
        {
            Debug.LogWarning("Failed Save Attempt! Exception Erorr: " + e);
        }
       /* using (StreamWriter medalFile = new StreamWriter(Application.persistentDataPath + "Medal.cvs"))
        {
            for (int i = 0; i < totalMedalCounts.Length; i++)
            {
                //Order of medals Saved -- 0 = Most Stunned, 1 = Most Stuns, 2 = Most Dashes, 3 =  most power ups used
                medalFile.WriteLine(totalMedalCounts[i]);
                Debug.Log(totalMedalCounts[i]);
            }
        }*/
    }

    public void ReadMedalSaveFile()
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + "/PlayerData.cst"))
            {
                saveData.Load();
            }
            else
            {
                WriteMedalSaveFile();
            }
        }
        catch
        {
            Debug.LogWarning("Did not save");
        }
       /* if (File.Exists(Application.persistentDataPath + "Medal.cvs"))
        {
            using (StreamReader medalFileR = new StreamReader(Application.persistentDataPath + "Medal.cvs"))//("Medal.csv"))
            {

                List<int> medalOrder = new List<int>();
                while (!medalFileR.EndOfStream)
                {
                    medalOrder.Add(Convert.ToInt32(medalFileR.ReadLine()));
                }
                totalMedalCounts = medalOrder.ToArray();
            }
        }
        else
        {
            Debug.LogWarning("File does not exist!/Was not generated or code error!");
        }*/
    }

}
