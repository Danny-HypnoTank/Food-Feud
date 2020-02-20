/// <summary>
/// Base class for buffs and debuffs. Check <see cref="SpeedUp"/>, <see cref="TrailPowerup"/> and/or <see cref="SplatRelease"/> for examples.
/// </summary>
public abstract class BuffDebuff
{

    /// <summary>
    /// The player using the powerup
    /// </summary>
    public PlayerController Parent { get; protected set; }


    /// <summary>
    /// Duration of the buff/debuff
    /// </summary>
    protected float duration;
    /// <summary>
    /// How much time has elapsed since the buff/debuff became active
    /// </summary>
    protected float elapsedTime;

    /// <summary>
    /// Start method for logic when the pickup is picked up. Call <see cref="base.Start()"/> at the very start.
    /// </summary>
    /// <param name="parent">The player the buff/debuff applies to</param>
    /// <param name="dur">Duration of the buff/debuff. Default is 5.</param>
    public virtual void Start(PlayerController parent, float dur = 5)
    {
        //Set the parent of the buff/debuff
        Parent = parent;
        //Initialise elapsed time at 0
        elapsedTime = 0;
        //Set duration to the duration specified in the parameters
        duration = dur;
    }

    /// <summary>
    /// Method for ongoing logic while the buff/debuff is active. Call <see cref="base.OnUpdate()"/> at the very start.
    /// </summary>
    /// <param name="deltaTime"><see cref="Time.DeltaTime"/></param>
    public virtual void OnUpdate(float deltaTime)
    {
        //Add deltaTime to the elapsedTime
        elapsedTime += deltaTime;

        //If elapsedTime hits or exceeds the duration, call the End method
        if (elapsedTime >= duration)
            End();
    }

    /// <summary>
    /// Method for logic when the buff/debuff ends. Call <see cref="base.End()"/> at the very end.
    /// </summary>
    public virtual void End() => Parent.SetProperty<BuffDebuff>(nameof(Parent.CurrentPowerup), null); //Set the player's current buff/debuff to null

}
