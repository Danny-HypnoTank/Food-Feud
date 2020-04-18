using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSquare : MonoBehaviour
{
    
    public int Value { get; private set; }

    public int Multiplier { get; private set; }

    private void Awake()
    {
        Value = -1;
        Multiplier = 1;
    }

    public void SetValue(int x)
    {
        Value = x;
    }

    public void SetMultiplier(int x)
    {
        Multiplier = x;
    }

}
