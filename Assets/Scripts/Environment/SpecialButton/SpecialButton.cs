using System;
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

    [Header("Special Logic")]
    [SerializeField]
    private SpecialButtonLogic logic;

    [Header("Graphics")]
    [SerializeField]
    private GameObject[] imageToDisplay;
    [SerializeField]
    private GameObject indicator;

    [Header("Settings")]
    [SerializeField]
    private Vector3 activePosition;

    [SerializeField]
    private Type test;

    //Fields
    private float timer;
    private bool finishedUI;
    private Vector3 inactivePosition;
    
    public PlayerController ActivatorPlayer { get; private set; }

    //Auto Properties
    public bool IsActive { get; private set; }

    //Full Properties
    public float ActivationTime { get { return _activationTime; } }

    public void Initialisation()
    {
        //Set initial values
        IsActive = false;
        finishedUI = false;
        inactivePosition = transform.localPosition;

        logic.Initialisation();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the colliding object is a player
        if (other.CompareTag("Player"))
        {
            //Cache reference to the player = other.GetComponent<PlayerController>();
            ActivatorPlayer = other.GetComponent<PlayerController>();

            if (ActivatorPlayer.IsDashing)
            {
                //If the button can be triggered give all other players the debuff else stun the colliding player
                if (CanBeTriggered(ActivatorPlayer.DashAmount))
                {
                    logic.DoAction();

                    //Start the coroutine to display the image for the special debuff
                    StartCoroutine(DisplayTheImage());

                    //Set the button to inactive so it can't be triggered again
                    timer = 0;
                    IsActive = false;
                    finishedUI = false;
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

    public void UpdateVisuals()
    {
        timer += Time.deltaTime;

        if (!finishedUI)
        {
            MoveButton();
            CheckTimerFinished();
        }
    }

    private void CheckTimerFinished()
    {
        if(timer >= ActivationTime && !IsActive)
            ActivateButton();
    }

    private void MoveButton()
    {
        transform.localPosition = Vector3.Lerp(inactivePosition, activePosition, timer / ActivationTime);
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