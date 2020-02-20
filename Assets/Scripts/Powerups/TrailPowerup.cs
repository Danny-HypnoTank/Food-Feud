using UnityEngine;

/// <summary>
/// Class for the trail buff
/// </summary>
public class TrailPowerup : BuffDebuff
{

    public override void Start(PlayerController parent, float dur = 5)
    {
        //Call the base implementation of Start
        base.Start(parent, 10);
    }

    public override void OnUpdate(float deltaTime)
    {
        //Call the base implementation of OnUpdate
        base.OnUpdate(deltaTime);
        //Whenever the elapsed time can be divided by 0.25 with a remainder that is less than 0.1 (e.g. 6.253 % = 0.003)
        //and if the player is moving, call the splat method
        if (elapsedTime % 0.25 < 0.1 && Parent.MoveVelocity != Vector3.zero)
            Parent.Splat();
    }

    public override void End()
    {
        //Call the base implementation of End
        base.End();
    }
}
