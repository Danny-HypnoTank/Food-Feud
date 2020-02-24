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
    SlowDown
}

public class PowerupHolder : MonoBehaviour
{

    [Header("Powerup Properties")]
    [SerializeField]
    private Powers powerHeld; //Which buff/debuff does the object hold
    [SerializeField]
    private bool isDebuff; //Is the buff/debuff a debuff
    [SerializeField]
    private float speed; //The speed at which the object rotates and bobs up and down
    private float originalY;
    private PowerupNode parent;

    //Dictionary to contain the buff/debuff classes
    private readonly Dictionary<string, Type> powers = new Dictionary<string, Type>()
    {
            {"Speedup", typeof(SpeedUp)},
            {"Trail", typeof(TrailPowerup)},
            {"Splat", typeof(SplatRelease)},
            {"StunImmunity", typeof(StunImmunity)},
            {"Teleport", typeof(Teleport)},
            {"SlowDown", typeof(MoveSlowly)}
    };

    private void Awake()
    {
        //Set the parent node
        parent = transform.parent.gameObject.GetComponent<PowerupNode>();
        //Set the original y position
        originalY = transform.position.y;
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
            //Get String value of powerheld
            string power = Enum.GetName(typeof(Powers), powerHeld);

            //If this is a buff, give it to the player who entered the trigger, else give it to everyone else
            if (!isDebuff)
            {
                if (player.CurrentPowerup == null)
                {
                    //Give player the buff
                    player.PickUpPowerUp((BuffDebuff)Activator.CreateInstance(powers[power]));

                    //Call the Collected method on the parent
                    parent.Collected();
                    //Disable this object
                    gameObject.SetActive(false);
                }
            }
            else
            {
                //Find all player objects
                GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

                //Loop through player objects
                for (int i = 0; i < playerObjects.Length; i++)
                {
                    //Cache reference to currently checked player
                    PlayerController playerToCheck = playerObjects[i].GetComponent<PlayerController>();
                    //If the checked player is not the player who entered the trigger, give them the debuff
                    if (playerToCheck != player)
                        playerToCheck.PickUpPowerUp((BuffDebuff)Activator.CreateInstance(powers[power]));
                }

                //Call the Collected method on the parent
                parent.Collected();
                //Disable this object
                gameObject.SetActive(false);
            }  
        }
    }
}