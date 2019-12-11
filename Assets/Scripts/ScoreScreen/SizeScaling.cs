/*Created by
 * Name:Prakhar, Chethan
 * Date created:18/11/19
 * Date modified:10/12/19
 * Sid:1843555,1831604
 */
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class SizeScaling : MonoBehaviour
{
    public Vector3 minScale;
    public Vector3 maxScale;
    //public bool repeatable;
    public float speed = 2.0f;
    public float duration = 5.0f;

    [SerializeField]
    private Player[] players;

    [SerializeField]
    private GameObject[] podiums;
    private int[] ScaleFactor;
    private float[] scores;


    private void Start()
    {

        ScaleFactor = new int[4];
        scores = new float[4];
        PercentageCalc();
        Scale();

    }

    private void Scale()
    {
        minScale = transform.localScale;
        // while (repeatable)
        
            
            //yield return RepeatLerp(maxScale, minScale, duration);
        
        for (int i = 0; i < players.Length; i++)
        {
            //players[i].
           
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isActivated == true)
            {
                if (players[i].isLocked == true)
                {
                    maxScale = new Vector3(150, (int)players[i].scorePercentage, 150);
                    minScale = new Vector3(150, 0, 150);
                    StartCoroutine(RepeatLerp(minScale, maxScale, duration,podiums[i]));
                    // maxScale = new Vector3(players[i].playerScore(10));

                }
            }
        }


    }

    private void PercentageCalc()
    {

        float total = 0;

        foreach(Player p in players)
        {

            total += p.playerScore;

        }

        for (int i = 0; i < 4; i++)
        {

            players[i].scorePercentage = (players[i].playerScore / total) * 100;

        }
        
    }

    private void SetScaleFactor()
    {

        for(int i = 0; i < 4; i++)
        {

            scores[i] = players[i].scorePercentage;

        }

        Array.Sort(scores);

        for (int i = 0; i < 4; i++)
        {

            switch(i)
            {

                case 0:
                    ScaleFactor[0] = 100;
                    break;

                case 1:
                    ScaleFactor[1] = 75;
                    break;

                case 2:
                    ScaleFactor[2] = 50;
                    break;

                case 3:
                    ScaleFactor[3] = 25;
                    break;

            }

        }

        var index = players.Max();


    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time, GameObject podium)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            podium.transform.Translate(Vector3.up * (rate/2) * Time.deltaTime);
            podium.transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    public void MainMenuReturnBtn()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void Update()
    {
        if (Input.GetButtonDown("BackButton"))
        {
            MainMenuReturnBtn();
        }
    }
}