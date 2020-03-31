using UnityEngine;

public abstract class SpecialUILogic : ScriptableObject
{
    public abstract void Initialisation(float time);
    public abstract void UpdateUI(float time);
}
