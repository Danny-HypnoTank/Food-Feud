using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    /// <summary>
    /// Extension method for calculating a value using a percentage and a given range
    /// </summary>
    /// <param name="x">The float to modify</param>
    /// <param name="min">The minimum value of the range</param>
    /// <param name="max">The maximum value of the range</param>
    /// <param name="percentage">The percentage you're looking for</param>
    public static void CalculateFromPercentage(this ref float x, float min, float max, float percentage)
    {
        x = (percentage * (max - min)) + min;
    }

    /// <summary>
    /// Extension method for changing the value of a property via Reflection
    /// </summary>
    /// <typeparam name="T">The type of the property (E.g. <see cref="string"/> or <see cref="int"/></typeparam>
    /// <param name="propertyName">The name of the property (E.g. <see cref="nameof(MoveSpeed)"/>)</param>
    /// <param name="newValue">The new value of the property (E.g. "Steve" or 7)</param>
    public static void SetProperty<T>(this object obj, string propertyName, T newValue)
    {
        //Implicit variable since the explicit type can vary
        var property = obj.GetType().GetProperty(propertyName);

        //If the property is found, set the new value
        if (property != null)
            property.SetValue(obj, newValue);
        else
            Debug.LogError($"Property not found: {propertyName} on onbject: {obj}");
    }

}
