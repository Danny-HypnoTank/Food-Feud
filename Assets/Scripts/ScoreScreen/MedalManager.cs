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

    public int[] totalMedalCounts { get; private set; }

    public List<UnityEngine.UI.Text> medalCounts;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
        totalMedalCounts = new int[4];
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "NewMainMenu")
        {
            SetTotalMedalCount();
        }
    }

    private void Start()
    {
        StartCoroutine("LateStart", 1);


    }

    public int OnPlayerStunned(int i)
    {

        timesStunned[i - 1] += 1;
        return i;
    }

    public int OnPlayerDash(int i)
    {

        timesDashed[i - 1] += 1;
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
        return i;
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
        using (StreamWriter medalFile = new StreamWriter("Medal.csv"))
        {
            for (int i = 0; i < totalMedalCounts.Length; i++)
            {
                //Order of medals Saved -- 0 = Most Stunned, 1 = Most Stuns, 2 = Most Dashes, 3 =  most power ups used
                medalFile.WriteLine(totalMedalCounts[i]);
            }
        }
    }

    public void ReadMedalSaveFile()
    {
        using (StreamReader medalFileR = new StreamReader("Medal.csv"))
        {
            List<int> medalOrder = new List<int>();
            while (!medalFileR.EndOfStream)
            {
                medalOrder.Add(Convert.ToInt32(medalFileR.ReadLine()));
            }
            totalMedalCounts = medalOrder.ToArray();
        }
    }

    public void SetTotalMedalCount()
    {
        for(int i = 0; i <totalMedalCounts.Length; i++)
        {
            switch(i)
            {
                case (0):
                    {
                        medalCounts[i].text = "Stunned: " + totalMedalCounts[i].ToString();
                        break;
                    }
                case (1):
                    {
                        medalCounts[i].text = "Most Dashes: " + totalMedalCounts[i].ToString();
                        break;
                    }
                case (2):
                    {
                        medalCounts[i].text = "Most Stuns: " + totalMedalCounts[i].ToString();
                        break;
                    }
                case (3):
                    {
                        medalCounts[i].text = "Powerup Master: " + totalMedalCounts[i].ToString();
                        break;
                    }
            }
        }
    }
}
