/// <summary>
/// Class for the stun immunity buff
/// </summary>
public class StunImmunity : BuffDebuff
{
    public override void Start(PlayerController parent, float dur = 5, bool refresh = true)
    {
        base.Start(parent, refresh: false);
        Parent.SImunnityObj.SetActive(true);
    }

    public override void End()
    {
        Parent.SImunnityObj.SetActive(false);
        Parent.SetProperty<StunImmunity>(nameof(Parent.StunImmunityPowerup), null);
    }
}
