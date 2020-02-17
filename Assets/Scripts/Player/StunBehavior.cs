/*
 * Created by:
 * Name: James Sturdgess
 * Sid: 1314371
 * Date Created: 01/11/2019
 * Last Modified 10/11/2019
 * Modified By: Danny Pym-Hember, Dominik
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for handling the behavior of the Stun Bar
/// </summary>
public class StunBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stunStars;

    /// <summary>
    /// Property to contain the progress of the Stun Bar
    /// </summary>
    public float StunProgress;
    /// <summary>
    /// Property for if the player is recovering their stamina
    /// </summary>
    public bool Recovering { get; private set; }
    public GameObject[] StunStars { get => stunStars; set => stunStars = value; }


    /// <summary>
    /// The bar
    /// </summary>
  //  [SerializeField]
   // private Image bar;

    /// <summary>
    /// Initialisation
    /// </summary>
    void Start()
    {

        StunProgress = 0;

        Recovering = false;

    }

    private void Update()
    {
        if (StunProgress >= 0.3f && StunProgress <= 0.69f)
        {
            stunStars[0].SetActive(true);
            stunStars[1].SetActive(false);
            stunStars[2].SetActive(false);
            stunStars[3].SetActive(false);
        }
        else if (StunProgress >= 0.7f && StunProgress <= 0.99f)
        {
            stunStars[0].SetActive(true);
            stunStars[1].SetActive(true);
            stunStars[2].SetActive(false);
            stunStars[3].SetActive(false);
        }
        else if (StunProgress > 1)
        {
            stunStars[0].SetActive(true);
            stunStars[1].SetActive(true);
            stunStars[2].SetActive(true);
            stunStars[3].SetActive(true);
        }
        if(StunProgress <= 0)
        {
            stunStars[0].SetActive(false);
            stunStars[1].SetActive(false);
            stunStars[2].SetActive(false);
            stunStars[3].SetActive(false);
        }
    }

    /// <summary>
    /// Method for adding to the Stun bar
    /// </summary>
    /// <param name="value">The value to add to the bar</param>
    public void AddStun(float value, DazeState daze, Player player)
    {

        if (StunProgress < 1)
        {
            StunProgress += value;
           // bar.fillAmount = StunProgress;

        }
    }

    /// <summary>
    /// Coroutine to handle restoration of the Stun Bar
    /// </summary>
    public IEnumerator RemoveStun(DazeState daze)
    {

        //Initial Delay
        yield return new WaitForSeconds(1);

        Recovering = true;

        //Recover Stun Bar in increments
        for (float i = StunProgress; i > 0; i -= 0.1f)
        {
            StunProgress = i;
          //  bar.fillAmount = StunProgress;
            yield return new WaitForSeconds(0.5f);
        }

        StunProgress = 0;
       // bar.fillAmount = StunProgress;

        Recovering = false;

        //daze.Stunned = false;

    }

}
