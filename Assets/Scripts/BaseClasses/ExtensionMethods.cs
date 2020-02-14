using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    public static void CalculateFromPercentage(this ref float x, float min, float max, float percentage)
    {
        x = (percentage * (max - min)) + min;
    }

}
