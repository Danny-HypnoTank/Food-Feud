public class MassFreeze : BuffDebuff
{

    public override void Start(PlayerController parent, float dur = 5, bool refresh = true)
    {
        base.Start(parent, refresh: false);
        parent.StartCoroutine(Parent.PlayerStun.Freeze());
        ManageGame.instance.camShake.ShakeCamera();
        End();
    }

    public override void End()
    {
        base.End();
    }

}
