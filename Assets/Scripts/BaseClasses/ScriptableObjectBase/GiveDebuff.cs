using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewButtonLogic", menuName = "Special Button/Debuff Giver")]
public class GiveDebuff : SpecialButtonLogic
{

    private BuffDebuff power;
    private int playerCount;
    private PlayerController activatorPlayer;

    public override void DoAction()
    {
        activatorPlayer = ManageGame.instance.SpecialButton.ActivatorPlayer;

        for (int i = 0; i < playerCount; i++)
        {
            //If the player to check isn't the colliding player, give them the debuff
            if (ManageGame.instance.allPlayerControllers[i] != activatorPlayer)
                ManageGame.instance.allPlayerControllers[i].PickUpPowerUp(power);
        }
    }

    public override void Initialisation()
    {
        playerCount = ManageGame.instance.allPlayerControllers.Count;
        power = new MassFreeze();
    }
}
