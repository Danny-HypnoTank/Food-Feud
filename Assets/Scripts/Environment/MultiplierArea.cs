using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierArea : MonoBehaviour
{
    private void OnEnable()
    {
        Vector3 overlapDimensions = transform.localScale / 2;
        overlapDimensions.y = transform.localScale.y;

        Collider[] squaresHit = Physics.OverlapBox(transform.position, overlapDimensions, Quaternion.identity);

        for (int i = 0; i < squaresHit.Length; i++)
        {
            ScoreSquare current = squaresHit[i].GetComponent<ScoreSquare>();
            if (current != null)
                current.SetMultiplier(2);
        }
    } 
}