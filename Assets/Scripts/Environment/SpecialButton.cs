using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

enum SpecialPowers
{
    MassFreeze,
}

public class SpecialButton : MonoBehaviour
{

    [Header("Power Settings")]
    [SerializeField]
    private SpecialPowers debuff;

    [Header("Activation Settings")]
    [SerializeField, Range(0, 1)]
    private float triggerThreshold;
    [SerializeField]
    private float _activationTime;

    [Header("Fridge Graphics")]
    [SerializeField, Tooltip("Leave empty in Sink level")]
    private Image specialBar;

    [Header("Sink Graphics")]
    [SerializeField, Tooltip("Leave empty in Fridge level")]
    private SpriteRenderer lightObject;
    [SerializeField]
    private Sprite defaultLight;
    [SerializeField]
    private Sprite litLight;

    [Header("Graphics")]
    [SerializeField]
    private GameObject[] imageToDisplay;
    [SerializeField]
    private GameObject indicator;

    //Fields
    private string powerName;
    private BuffDebuff power;
    private List<PlayerController> playerToCheck;
    private readonly Dictionary<string, Type> powers = new Dictionary<string, Type>
    {
        {"MassFreeze", typeof(MassFreeze)}
    };

    //Auto Properties
    public bool IsActive { get; private set; }
    public bool HasBeenUsed { get; private set; }

    //Full Properties
    public float ActivationTime { get { return _activationTime; } }

    public  void Initialisation()
    {
        //Set initial values
        IsActive = false;
        HasBeenUsed = false;
        if(lightObject != null)
            lightObject.sprite = defaultLight;

        //Instantiate an object based on the special power
        powerName = Enum.GetName(typeof(SpecialPowers), debuff);
        power = (BuffDebuff)Activator.CreateInstance(powers[powerName]);

        //Instantiate the list of players
        playerToCheck = ManageGame.instance.allPlayerControllers;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the colliding object is a player
        if (other.CompareTag("Player"))
        {
            //Cache reference to the player
            PlayerController player = other.GetComponent<PlayerController>();

            if (player.IsDashing)
            {
                //If the button can be triggered give all other players the debuff else stun the colliding player
                if (CanBeTriggered(player.dashAmount))
                {
                    for (int i = 0; i < playerToCheck.Count; i++)
                    {
                        //If the player to check isn't the colliding player, give them the debuff
                        if (playerToCheck[i] != player)
                            playerToCheck[i].PickUpPowerUp(power);
                    }

                    //Start the coroutine to display the image for the special debuff
                    StartCoroutine(DisplayTheImage());

                    //Set the button to inactive so it can't be triggered again
                    IsActive = false;
                    HasBeenUsed = true;
                    indicator.SetActive(false);
                }
                else
                    player.PlayerStun.Stun(player.dashAmount, null);
            }
        }
    }

    public void ActivateButton()
    {
        IsActive = true; //Set IsActive to true and put button in the active position
        indicator.SetActive(true);
    }

    public void UpdateBar(float time)
    {
        if (specialBar != null)
        {
            if (time < ActivationTime)
            {
                //Calculate the current progress of the in-game time towards activation time as a percentage, and set the value to that percentage
                float value = 0;
                value = time / ActivationTime;
                specialBar.fillAmount = value;
            }
        }
        else if (lightObject != null)
        {
            if (time >= ActivationTime)
                lightObject.sprite = litLight;
        }
        else
            Debug.LogError("Error in SpecialButton.cs: No power-up indicator set!");
    }

    private bool CanBeTriggered(float dashAmount)
    {
        //Default result is false
        bool result = false;

        //If IsActive is true and dashAmount is greater than or equal to the triggerThreshold set result to true
        if (IsActive && dashAmount >= triggerThreshold)
            result = true;

        //Return the result
        return result;
    }

    private IEnumerator DisplayTheImage()
    {
        imageToDisplay.ToggleGameObjects(true);
        yield return new WaitForSeconds(1.5f);
        imageToDisplay.ToggleGameObjects(false);
    }

}