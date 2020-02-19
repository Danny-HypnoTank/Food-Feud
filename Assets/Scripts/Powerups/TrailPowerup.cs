using System.Collections;
using UnityEngine;

public class TrailPowerup : BuffDebuff
{

    public override void Start(PlayerController parent)
    {
        Parent = parent;
        IsContinuous = true;
        Parent.StartCoroutine(Timer());
    }

    public override void OnUpdate()
    {
        Parent.Splat();
    }

    public override void End()
    {

        Parent.SetProperty<BuffDebuff>(nameof(Parent.CurrentPowerup), null);
    }

    public override IEnumerator Timer()
    {
        yield return new WaitForSeconds(5);
        End();
    }
}
