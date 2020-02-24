/// <summary>
/// Class for the speedup buff
/// </summary>
public class MoveSlowly : BuffDebuff
{

    //Field to store the original speed of the player
    float originalSpeed;

    public override void Start(PlayerController parent, float dur = 5)
    {
        //Call the base implementation of Start
        base.Start(parent, 10);
        //Set the originalSpeed to the player's speed modifier
        originalSpeed = Parent.MoveSpeedModifier;
        //Double the player's speed modifier
        Parent.SetProperty<float>(nameof(Parent.MoveSpeedModifier), originalSpeed / 2);
    }

    public override void OnUpdate(float deltaTime)
    {
        //Call the base implementation of OnUpdate
        base.OnUpdate(deltaTime);
    }

    public override void End()
    {
        //Set the player's speed modifier back to the default
        Parent.SetProperty<float>(nameof(Parent.MoveSpeedModifier), originalSpeed);
        //Call the base implementation of End
        base.End();
    }
}