using UnityEngine;
/// <summary>
/// Class for the speedup buff
/// </summary>
public class PullIn : BuffDebuff
{

    //Field to store the original speed of the player
    GameObject[] playerObjects;
    public override void Start(PlayerController parent, float dur = 5, bool refresh = true)
    {
        //Call the base implementation of Start
        base.Start(parent, 10);
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
    }

    public override void OnUpdate(float deltaTime)
    {
        //Call the base implementation of OnUpdate
        base.OnUpdate(deltaTime);
        for (int i = 0; i < playerObjects.Length; i++)
        {
            //Cache reference to currently checked player
            PlayerController playerToCheck = playerObjects[i].GetComponent<PlayerController>();
            //If the checked player is not the player who entered the trigger, give them the debuff
            if (playerToCheck != Parent)
            {
                Vector3 forceDirection = Parent.transform.position - playerToCheck.transform.position;
                playerToCheck.GetComponent<CharacterController>().Move(forceDirection * 0.002f);
                //playerToCheck.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * 1f * Time.fixedDeltaTime);
            }
        }
    }



    public override void End()
    {

        //Call the base implementation of End
        base.End();
    }
}
