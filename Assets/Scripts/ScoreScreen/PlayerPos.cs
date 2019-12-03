/*Created by
 * Name:Prakhar, Chethan
 * Date created:24/11/19
 * Sid:1843555,1831604
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    Vector3 currentPosition;
    Vector3 previousPosition;
    public GameObject holder;
    private void Update()
    {
        currentPosition = holder.transform.position;
        if (holder.transform.position != previousPosition)
        {
            holder.transform.position = currentPosition;
            this.transform.position = holder.transform.position;
        }
        previousPosition = currentPosition;
    }

    private void PlayerPRefs()
    {
        PlayerPrefs.SetFloat("Player1Score", 20);


    }

    private void GetPrefs()
    {
        float playerOne = PlayerPrefs.GetFloat("PlayerScore");
    }
}
