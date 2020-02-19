public abstract class BuffDebuff
{

    /// <summary>
    /// The player using the powerup
    /// </summary>
    public PlayerController Parent { get; protected set; }

    protected float duration; //Duration of the powerup
    protected float elapsedTime; //How much time has elapsed

    public virtual void Start(PlayerController parent, float dur = 5)
    {
        Parent = parent;
        elapsedTime = 0;
        duration = dur;
    }

    public virtual void OnUpdate(float deltaTime)
    {
        elapsedTime += deltaTime;

        if (elapsedTime >= duration)
            End();
    }

    public virtual void End() => Parent.SetProperty<BuffDebuff>(nameof(Parent.CurrentPowerup), null);

}
