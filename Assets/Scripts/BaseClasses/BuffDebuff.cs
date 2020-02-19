using System.Collections;

public abstract class BuffDebuff
{

    /// <summary>
    /// The player using the powerup
    /// </summary>
    public PlayerController Parent { get; protected set; }

    /// <summary>
    /// Is the powerup active - default: false
    /// </summary>
    public bool Active { get; protected set; } = false;

    /// <summary>
    /// Is the powerup continuous - default: false
    /// </summary>
    public bool IsContinuous { get; protected set; } = false;
    public abstract void Start(PlayerController parent);

    public abstract void OnUpdate();
    public abstract void End();

    public void Activate()
    {

        Active = true;

    }

    public abstract IEnumerator Timer();

}
