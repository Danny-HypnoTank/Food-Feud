using UnityEngine;

public class LightUI : MonoBehaviour, ISpecialUI
{
    [Header("Light Properties")]
    [SerializeField]
    private SpriteRenderer lightObject;
    [SerializeField]
    private Sprite defaultLight;
    [SerializeField]
    private Sprite litLight;

    private float activationTime;

    private void Awake()
    {
        if (lightObject != null)
            lightObject.sprite = defaultLight;
    }

    public void GetActivationTime(float time)
    {
        activationTime = time;
    }

    public void UpdateUI(float time)
    {
        if (lightObject != null)
        {
            if (Mathf.RoundToInt(time) >= activationTime)
                lightObject.sprite = litLight;
        }
    }
}
