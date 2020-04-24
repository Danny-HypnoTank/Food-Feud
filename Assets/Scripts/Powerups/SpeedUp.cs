/// <summary>
/// Class for the speedup buff
/// </summary>
public class SpeedUp : BuffDebuff
{

    public override void Start(PlayerController parent, float dur = 5, bool refresh = true)
    {
        //Call the base implementation of Start
        base.Start(parent, 10);
        //Double the player's speed modifier
        Parent.SetProperty<float>(nameof(Parent.MoveSpeedModifier), Parent.BaseSpeed * 2);
        Parent.PlayerBase.Animator.SetFloat("walkAnimSpeed", 2);
    }

    public override void OnUpdate(float deltaTime)
    {
        //Call the base implementation of OnUpdate
        base.OnUpdate(deltaTime);
    }

    public override void End()
    {
        //Set the player's speed modifier back to the default
        Parent.SetProperty<float>(nameof(Parent.MoveSpeedModifier), Parent.BaseSpeed);
        Parent.PlayerBase.Animator.SetFloat("walkAnimSpeed", 1);
        //Call the base implementation of End
        base.End();
    }
}