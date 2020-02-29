using System;
using System.Collections.Generic;
using UnityEngine;

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

    //Fields
    private string powerName;
    private Vector3 inactivePosition;
    private Vector3 activePosition;
    private readonly Dictionary<string, Type> powers = new Dictionary<string, Type>
    {

        {"MassFreeze", typeof(MassFreeze)}

    };

    //Auto Properties
    public bool IsActive { get; private set; }
    public bool HasBeenUsed { get; private set; }

    //Full Properties
    public float ActivationTime { get { return _activationTime; } }

    private void Awake()
    {
        //Set IsActive to false and get the string value of power
        IsActive = false;
        HasBeenUsed = false;
        powerName = Enum.GetName(typeof(SpecialPowers), debuff);
        inactivePosition = transform.position;
        activePosition = new Vector3(-54.5f, 3, 0);

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
                    //Find all the players
                    GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

                    for (int i = 0; i < allPlayers.Length; i++)
                    {
                        //Cache a reference to the current player to check
                        PlayerController playerToCheck = allPlayers[i].GetComponent<PlayerController>();

                        //If the player to check isn't the colliding player, give them the debuff
                        if (playerToCheck != player)
                            playerToCheck.PickUpPowerUp((BuffDebuff)Activator.CreateInstance(powers[powerName]));
                    }

                    //Set the button to inactive so it can't be triggered again
                    transform.position = inactivePosition;
                    IsActive = false;
                    HasBeenUsed = true;
                }
                else
                    player.PlayerStun.Stun(player.dashAmount);

            }
        }
    }

    public void ActivateButton()
    {
        //Set IsActive to true and put button in the active position
        IsActive = true;
        transform.position = activePosition;
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

}