public class SpeedModifier : BuffDebuff
{

    float originalSpeed;

    public override void Start(PlayerController parent, float dur = 5)
    {
        base.Start(parent, 10);
        originalSpeed = Parent.MoveSpeedModifier;
        Parent.SetProperty<float>(nameof(Parent.MoveSpeedModifier), originalSpeed * 2);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }
    public override void End()
    {
        Parent.SetProperty<float>(nameof(Parent.MoveSpeedModifier), originalSpeed);
        base.End();
    }
}