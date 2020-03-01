using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassFreeze : BuffDebuff
{

    public override void Start(PlayerController parent, float dur = 5)
    {
        base.Start(parent, dur);
        parent.StartCoroutine(Parent.PlayerStun.Stun(2));
        End();
    }

    public override void End()
    {
        base.End();
    }

}
