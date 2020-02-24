using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : BuffDebuff
{

    //private TeleportObject teleportObject;
    private Vector3 startLocation;

    public override void Start(PlayerController parent, float dur = 5)
    {
        base.Start(parent);
        //teleportObject = Parent.TeleportObject.GetComponent<TeleportObject>();
    }

    public override void OnUpdate(float deltaTime)
    {
        if(Input.GetButtonDown($"Shoot{Parent.Player.playerNum}"))
        {
            Parent.chc.detectCollisions = false;
            Parent.chc.Move(Parent.transform.forward * 25);
            Parent.chc.detectCollisions = true;
            End();
        }
    }

    public override void End()
    {
        base.End();
    }

}