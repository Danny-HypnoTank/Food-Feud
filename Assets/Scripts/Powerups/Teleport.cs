﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : BuffDebuff
{

    //private TeleportObject teleportObject;
    private Vector3 startLocation;


    public override void Start(PlayerController parent, float dur = 5, bool refresh = true)
    {
        base.Start(parent, refresh: false);
        //teleportObject = Parent.TeleportObject.GetComponent<TeleportObject>();
    }

    public override void OnUpdate(float deltaTime)
    {
        if(Input.GetButtonDown($"Shoot{Parent.Player.playerNum}"))
        {
            Parent.chc.detectCollisions = false;

            ParticleSystem smokeSystem;
            smokeSystem = Parent.smokeParticles;
            smokeSystem.Play();
            Parent.chc.Move(Parent.transform.forward * 25);
            Parent.chc.detectCollisions = true;
            smokeSystem.Play();
            End();
        }
    }

    public override void End()
    {
        base.End();
    }

}