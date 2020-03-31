using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum SpecialPowers
{
    MassFreeze,
    Flood,
    Trash
}
public class SpecialButton : MonoBehaviour
{

    [Header("Power Settings")]
    [SerializeField]
    private SpecialPowers debuff;

    [Header("Activation Settings")]
    [SerializeField, Range(0, 1)]
    private float triggerThreshold;
    [SerializeField]
    private float _activationTime;

    [Header("Trash properties")]
    [SerializeField]
    private TrashDropper dropper;

    [Header("Flood Properties")]
    [SerializeField]
    private SinkPullIn massFlood;

    [Header("Graphics")]
    [SerializeField]
    private GameObject[] imageToDisplay;
    [SerializeField]
    private GameObject indicator;

    [Header("Settings")]
    [SerializeField]
    private Vector3 activePosition;
    [SerializeField]
    private float lerpTime;

    //Fields
    private float timer;
    private bool finishedUI;
    private Vector3 inactivePosition;
    private BuffDebuff power;
    private ISpecialUI specialUILogic;

    private readonly Dictionary<SpecialPowers, BuffDebuff> powers = new Dictionary<SpecialPowers, BuffDebuff>
    {
        {SpecialPowers.MassFreeze, new MassFreeze()},
        {SpecialPowers.Flood, new MassFreeze()},
        {SpecialPowers.Trash, null}
    };

    //Auto Properties
    public bool IsActive { get; private set; }
    public bool HasBeenUsed { get; private set; }

    //Full Properties
    public float ActivationTime { get { return _activationTime; } }

    private void Awake()
    {
        specialUILogic = GetComponent<ISpecialUI>();
    }

    public void Initialisation()
    {
        //Set initial values
        IsActive = false;
        HasBeenUsed = false;
        finishedUI = false;
        inactivePosition = transform.localPosition;
        specialUILogic.GetActivationTime(ActivationTime);

        //Instantiate an object based on the special power
        power = powers[debuff];
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the colliding object is a player
        if (other.CompareTag("Player"))
        {
            //Cache reference to the player
            PlayerController player = other.GetComponent<PlayerController>();

            if (player.IsDashing)
            {
                //If the button can be triggered give all other players the debuff else stun the colliding player
                if (CanBeTriggered(player.dashAmount))
                {
                    if (debuff == SpecialPowers.Trash && dropper != null)
                    {
                        dropper.DropTrash();
                    }
                    else if (debuff == SpecialPowers.Flood)
                    {
                        massFlood.gameObject.SetActive(true);
                    }
                    else
                    {
                        for (int i = 0; i < ManageGame.instance.allPlayerControllers.Count; i++)
                        {
                            //If the player to check isn't the colliding player, give them the debuff
                            if (ManageGame.instance.allPlayerControllers[i] != player)
                                ManageGame.instance.allPlayerControllers[i].PickUpPowerUp(power);
                        }
                    }

                    //Start the coroutine to display the image for the special debuff
                    StartCoroutine(DisplayTheImage());

                    //Set the button to inactive so it can't be triggered again
                    IsActive = false;
                    HasBeenUsed = true;
                    transform.localPosition = inactivePosition;
                    indicator.SetActive(false);
                }
            }
        }
    }

    public void ActivateButton()
    {
        IsActive = true; //Set IsActive to true and put button in the active position
        transform.localPosition = activePosition;
        finishedUI = true;
        indicator.SetActive(true);
    }

    public void UpdateVisuals(float time)
    {
        if (!finishedUI)
        {
            MoveButton();
            specialUILogic.UpdateUI(time);
        }
    }

    private void MoveButton()
    {
        timer += Time.deltaTime;
        transform.localPosition = Vector3.Lerp(inactivePosition, activePosition, timer / lerpTime);
    }

    private bool CanBeTriggered(float dashAmount)
    {
        //Default result is false
        bool result = false;

        //If IsActive is true and dashAmount is greater than or equal to the triggerThreshold set result to true
        if (IsActive && dashAmount >= triggerThreshold)
            result = true;

        //Return the result
        return result;
    }

    private IEnumerator DisplayTheImage()
    {
        imageToDisplay.ToggleGameObjects(true);
        yield return new WaitForSeconds(1.5f);
        imageToDisplay.ToggleGameObjects(false);
    }
}