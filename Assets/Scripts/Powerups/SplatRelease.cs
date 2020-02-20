using UnityEngine;

/// <summary>
/// Class for the splat release buff
/// </summary>
public class SplatRelease : BuffDebuff
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
        //If the player releases the dash button and isn't dashing, run the splat method
        if (Input.GetButtonUp($"Dash{Parent.Player.playerNum}") && !Parent.IsDashing)
            Parent.Splat();
    }

    public override void End()
    {
        //Call the base implementation of End
        base.End();
    }

}
