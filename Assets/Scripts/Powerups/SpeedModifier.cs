using System.Collections;
using UnityEngine;

public class SpeedModifier : BuffDebuff
{

    float originalSpeed;

    public override void Start(PlayerController parent)
    {
        Parent = parent;

        originalSpeed = Parent.MoveSpeedModifier;
        Parent.SetProperty<float>(nameof(Parent.MoveSpeedModifier), 10);
        Parent.StartCoroutine(Timer());
    }

    public override void OnUpdate() { }

    public override void End()
    {
        Parent.SetProperty<float>(nameof(Parent.MoveSpeedModifier), originalSpeed);
        Parent.SetProperty<BuffDebuff>(nameof(Parent.CurrentPowerup), null);
    }
    public override IEnumerator Timer()
    {
        yield return new WaitForSeconds(10);
        End();
    }
}