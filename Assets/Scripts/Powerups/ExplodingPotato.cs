/// <summary>
/// Class for the exploding potato mechanic
/// </summary>
public class ExplodingPotato : BuffDebuff
{

    public override void Start(PlayerController parent, float dur = 5, bool refresh = true)
    {
        //Call the base implementation of Start
        base.Start(parent, 10, false);

    }

    public override void OnUpdate(float deltaTime)
    {
        //Call the base implementation of OnUpdate
        base.OnUpdate(deltaTime);
    }


    public override void End()
    {
        Parent.PlayerStun.StartCoroutine("Stun", 5);
        //Call the base implementation of End
        base.End();
    }

    public void OnHit(PlayerController other)
    {
        other.PickUpPowerUp(this);
        Parent.PickUpPowerUp(null);
    }
}