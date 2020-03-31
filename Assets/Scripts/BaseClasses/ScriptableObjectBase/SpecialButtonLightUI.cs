using UnityEngine;

[CreateAssetMenu(fileName = "NewSpecialUI", menuName = "SpecialUI/Light UI")]
public class SpecialButtonLightUI : SpecialUILogic
{
    [Header("Light Properties")]
    [SerializeField]
    private Sprite defaultLight;
    [SerializeField]
    private Sprite litLight;

    private float activationTime;
    private SpriteRenderer lightObject;

    public override void Initialisation(float time)
    {
        lightObject = GameObject.FindGameObjectWithTag("LightUI").GetComponent<SpriteRenderer>();

        if (lightObject != null)
            lightObject.sprite = defaultLight;

        activationTime = time;
    }

    public override void UpdateUI(float time)
    {
        if (lightObject != null)
        {
            if (Mathf.RoundToInt(time) >= activationTime)
                lightObject.sprite = litLight;
        }
    }
}
