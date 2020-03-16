using System;
using System.Collections.Generic;
using UnityEngine;

//Enum with the names of buffs/debuffs
enum Powers
{
    Speedup,
    Trail,
    Splat,
    StunImmunity,
    Teleport,
    SlowDown,
    ExplodingPotato,
    PullIn
}

public class PowerupHolder : MonoBehaviour
{

    [Header("Powerup Properties")]
    [SerializeField]
    private Powers powerHeld; //Which buff/debuff does the object hold
    [SerializeField]
    private bool isDebuff; //Is the buff/debuff a debuff

    [Header("Object Properties")]
    [SerializeField]
    private float speed; //The speed at which the object rotates and bobs up and down

    private float originalY; //The original value of the objects Y position
    private BuffDebuff power; //The object of the powerup
    private PowerupNode parent; //The parent node
    List<PlayerController> playerToCheck; //List to store references to all players

   

    //Dictionary to contain the buff/debuff classes
    private readonly Dictionary<Powers, Type> powers = new Dictionary<Powers, Type>()
    {
        {Powers.Speedup, typeof(SpeedUp)},
        {Powers.Trail, typeof(TrailPowerup)},
        {Powers.Splat, typeof(SplatRelease)},
        {Powers.StunImmunity, typeof(StunImmunity)},
        {Powers.Teleport, typeof(Teleport)},
        {Powers.SlowDown, typeof(MoveSlowly)},
        {Powers.ExplodingPotato, typeof(ExplodingPotato)},
        {Powers.PullIn, typeof(PullIn)}
    };

    private void Awake()
    {
        //Set the parent node
        parent = transform.parent.gameObject.GetComponent<PowerupNode>();
        //Set the original y position
        originalY = transform.position.y;

        //Instantiate new instance of power
        power = (BuffDebuff)Activator.CreateInstance(powers[powerHeld]);

        //Cache references to all PlayerControllers
        playerToCheck = ManageGame.instance.allPlayerControllers;
    }

    private void Update()
    {
        //Set the current position of the object
        Vector3 position = transform.position;
        //Rotate the object by the specified speed on the Y-axis
        transform.Rotate(0, speed, 0);
        //Make the object bob up and down on a Sine wave
        transform.position = new Vector3(position.x, originalY + Mathf.Sin(Time.time * speed), position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if player has entered trigger
        if (other.CompareTag("Player"))
        {
            //Cache reference to player
            PlayerController player = other.GetComponent<PlayerController>();

            //If this is a buff give it to the player who entered the trigger, else give it to everyone else
            if (!isDebuff)
            {
                //If the player doesn't have a power up give them the buff, else if the player's powerup is the same as this refresh the buff duration
                if (player.CurrentPowerup == null)
                {
                    //Give player the buff
                    player.PickUpPowerUp(power);

                    Collect(other);
                }
                else if (player.CurrentPowerup.GetType() == power.GetType())
                {
                    //Check if the player's buff can be refreshed
                    if (power.canRefresh)
                    {
                        //Refresh the duration on the player's buff
                        player.CurrentPowerup.RefreshDuration();

                        Collect(other);
                    }
                }
            }
            else
            {
                //Loop through player objects
                for (int i = 0; i < playerToCheck.Count; i++)
                {
                    //If the checked player is not the player who entered the trigger, give them the debuff
                    if (playerToCheck[i] != player)
                        playerToCheck[i].PickUpPowerUp((BuffDebuff)Activator.CreateInstance(powers[powerHeld]));
                }

                Collect(other);
            }
        }
    }

    private void Collect(Collider other)
    {
        //Call the Collected method on the parent and disable object
        

        parent.Collected();
        gameObject.SetActive(false);
    }

    
}