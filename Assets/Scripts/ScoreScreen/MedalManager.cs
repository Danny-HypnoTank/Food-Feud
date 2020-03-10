using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MedalManager : MonoBehaviour
{
    private static MedalManager _instance;

    public static MedalManager Instance { get { return _instance; } }

    public List<int> timesStunned;

    public GameObject StunMedal;

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



    IEnumerator LateStart(float wait)
    {
        yield return new WaitForSeconds(wait);

        foreach (DazeState d in FindObjectsOfType<DazeState>())
        {
            d.stunEvent += OnPlayerStunned;
        }

        //FindObjectOfType<DazeState>().stunEvent += OnPlayerStunned;
    }

    public int GetTopStunned()
    {
        int max = 0;
        int current = 0;
        for (int i = 0; i < timesStunned.Count; i++)
        {
            if (timesStunned[i] > max)
            {
                max = timesStunned[i];
                current = i;
            }
        }




        return current;
    }

    public void SpawnMedal(Vector3 pos)
    {
        GameObject _medal = Instantiate(StunMedal, pos + (Vector3.up * 14), Quaternion.Euler(0, -90, 90));
        _medal.transform.localScale = new Vector3(5, 5, 5);
    }
}
