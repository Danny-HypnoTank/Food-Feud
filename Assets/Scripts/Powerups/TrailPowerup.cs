public class TrailPowerup : BuffDebuff
{

    public override void Start(PlayerController parent, float dur = 5)
    {
        base.Start(parent);
        Parent = parent;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Parent.Splat();
    }

    public override void End()
    {
        base.End();
    }
}
