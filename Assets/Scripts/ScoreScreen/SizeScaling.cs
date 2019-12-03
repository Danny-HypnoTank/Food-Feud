/*Created by
 * Name:Prakhar, Chethan
 * Date created:18/11/19
 * Date modified:24/11/19
 * Sid:1843555,1831604
 */
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SizeScaling : MonoBehaviour
{
    public Vector3 minScale;
    public Vector3 maxScale;
    //public bool repeatable;
    public float speed = 2.0f;
    public float duration = 5.0f;

    [SerializeField]
    private Player[] players;
   

  IEnumerator Start()
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
                    maxScale = new Vector3(150, players[i].playerScore, 150);
                    minScale = new Vector3(150, 0, 150);
                    // maxScale = new Vector3(players[i].playerScore(10));

                }
            }
        }
        yield return RepeatLerp(minScale, maxScale, duration);


    }


    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.Translate(Vector3.up * rate * Time.deltaTime);
            transform.localScale = Vector3.Lerp(a, b, i);
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