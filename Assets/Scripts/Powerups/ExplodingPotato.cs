/// <summary>
/// Class for the speedup buff
/// </summary>
public class ExplodingPotato : BuffDebuff
{

    //Field to store the original speed of the player

    public override void Start(PlayerController parent, float dur = 5)
    {
        //Call the base implementation of Start
        base.Start(parent, 10);
        
    }

    public override void OnUpdate(float deltaTime)
    {
        //Call the base implementation of OnUpdate
        base.OnUpdate(deltaTime);
    }
    public override void End()
    {
        Parent.PlayerStun.StartCoroutine("Stun",5);
        //Call the base implementation of End
        base.End();
    }
}