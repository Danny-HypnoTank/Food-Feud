using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    [SerializeField]
    private NewCharacterSelectionController newSelection;
    [SerializeField]
    private Player player;
    [SerializeField]
    private bool isActive = false;
    [SerializeField]
    private Transform[] characters;
    [SerializeField]
    private Transform pin;
    [SerializeField]
    private Transform defaultPinLocation;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool isOnFridge = false;
    private bool isAxis = false;
    [SerializeField]
    private bool isPinMoving = false;
    [SerializeField]
    private Transform pinDirection;
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private int position;

    public bool IsActive { get => isActive; set => isActive = value; }
    public bool IsLocked { get => isLocked; set => isLocked = value; }

    private void OnEnable()
    {
        isLocked = false;
        isPinMoving = false;
        isOnFridge = false;
        target = null;
        isActive = false;
        pin.position = defaultPinLocation.position;
    }

    private void Start()
    {
        newSelection = this.GetComponentInParent<NewCharacterSelectionController>();
    }

    private void Update()
    {
        if (isPinMoving == false)
        {
            if (Input.GetButtonDown("Dash" + player.playerNum))
            {
                if (isActive == false)
                {
                    isActive = true;
                    InsertPlayer();
                }
                else if (isActive == true)
                {
                    if (characters[position].GetComponent<CharacterPin>().OwnedBy == null)
                    {
                        LockPlayer();
                    }
                }
            }
            if (Input.GetButtonDown("BackButton" + player.playerNum))
            {
                if (isActive == true)
                {
                    UnlockPlayer();
                }
            }
        }
        if (isOnFridge == true && isPinMoving == false && isLocked == false)
        { 
            if (Input.GetAxis("Horizontal" + player.playerNum) > 0.3f)
            {
                if (isAxis == false)
                {
                    isAxis = true;
                    if (target != null)
                    {
                        Transform oldTarget = target;
                        if (target.GetComponentInParent<CharacterPin>().TakeDirection("Right", (player.playerNum - 1)) != null)
                        { 
                            pinDirection = target.GetComponentInParent<CharacterPin>().TakeDirection("Right", (player.playerNum - 1));
                            target = target.GetComponentInParent<CharacterPin>().Right;
                            position = target.GetComponentInParent<CharacterPin>().PinId;
                            if (target != null)
                            {
                                StopCoroutine("MovePinAround");
                                StartCoroutine("MovePinAround");

                            }
                            else
                            {
                                target = oldTarget;
                            }
                        }
                    }

                }

            }
            else if (Input.GetAxis("Horizontal" + player.playerNum) < -0.3f)
            {
                if (isAxis == false)
                {
                    isAxis = true;
                    if (target != null)
                    {
                        Transform oldTarget = target;
                        if (target.GetComponentInParent<CharacterPin>().TakeDirection("Left", (player.playerNum - 1)) != null)
                        {
                            pinDirection = target.GetComponentInParent<CharacterPin>().TakeDirection("Left", (player.playerNum - 1));
                            target = target.GetComponentInParent<CharacterPin>().Left;
                            position = target.GetComponentInParent<CharacterPin>().PinId;
                            if (target != null)
                            {
                                StopCoroutine("MovePinAround");
                                StartCoroutine("MovePinAround");
                            }
                            else
                            {
                                target = oldTarget;
                            }
                        }
                    }

                }
            }
            else if (Input.GetAxis("Vertical" + player.playerNum) > 0.3f)
            {
                if (isAxis == false)
                {
                    isAxis = true;
                    if (target != null)
                    {
                        Transform oldTarget = target;
                        if (target.GetComponentInParent<CharacterPin>().TakeDirection("Up", (player.playerNum - 1)) != null)
                        {
                            pinDirection = target.GetComponentInParent<CharacterPin>().TakeDirection("Up", (player.playerNum - 1));
                            target = target.GetComponentInParent<CharacterPin>().Up;
                            position = target.GetComponentInParent<CharacterPin>().PinId;
                            if (target != null)
                            {
                                StopCoroutine("MovePinAround");
                                StartCoroutine("MovePinAround");
                            }
                            else
                            {
                                target = oldTarget;
                            }
                        }
                    }

                }
            }
            else if (Input.GetAxis("Vertical" + player.playerNum) < -0.3f)
            {
                if (isAxis == false)
                {
                    isAxis = true;
                    if (target != null)
                    {
                        Transform oldTarget = target;
                        if (target.GetComponentInParent<CharacterPin>().TakeDirection("Down", (player.playerNum - 1)) != null)
                        {
                            
                            pinDirection = target.GetComponentInParent<CharacterPin>().TakeDirection("Down", (player.playerNum - 1));
                            target = target.GetComponentInParent<CharacterPin>().Down;
                            position = target.GetComponentInParent<CharacterPin>().PinId;
                            if (target != null)
                            {
                                StopCoroutine("MovePinAround");
                                StartCoroutine("MovePinAround");
                            }
                            else
                            {
                                target = oldTarget;
                            }
                        }
                    }

                }
            }
            else
            {
                   isAxis = false;
            }
        }
    }

    private IEnumerator MovePin()
    {
        isPinMoving = true;
        bool arrived = false;
        while (!arrived)
        {
            pin.position = Vector3.MoveTowards(pin.position, target.position, speed * Time.deltaTime);
            if (Vector3.Distance(pin.position, target.position) < 0.01f) arrived = true;
            yield return null;
        }
        isPinMoving = false;
        yield return null;
    }

    private IEnumerator MovePinAround()
    {
        isPinMoving = true;
        bool arrived = false;
        while (!arrived)
        {
            pin.position = Vector3.MoveTowards(pin.position, pinDirection.position, speed * Time.deltaTime);
            if (Vector3.Distance(pin.position, pinDirection.position) < 0.01f) arrived = true;
            yield return null;
        }
        isPinMoving = false;
        yield return null;
    }

    private void CancelPlayer()
    {
        target = defaultPinLocation;
        StopCoroutine("MovePin");
        StartCoroutine("MovePin");
        isOnFridge = false;
    }

    private void InsertPlayer()
    {
        target = characters[2].GetComponent<CharacterPin>().PlayerPinLocations[player.playerNum -1];
        position = 2;
        StopCoroutine("MovePin");
        StartCoroutine("MovePin");
        isOnFridge = true;
    }

    public void LockPlayer()
    {
        isLocked = true;
        characters[position].GetComponent<CharacterPin>().OwnedBy = pin;
        // Debug.Log("Click! OWN PIN " + characters[position].name);
        characters[position].GetComponent<CharacterPin>().OwnPin();
        player.isLocked = true;
        player.isActivated = true;
        player.skinId = position;
        pinDirection = characters[position];
        StopCoroutine("MovePinAround");
        StartCoroutine("MovePinAround");
        newSelection.CheckSubmission();
    }

    public void UnlockPlayer()
    {
        if (isLocked == true)
        {
            target.GetComponentInParent<CharacterPin>().OwnedBy = null;
            isLocked = false;
            player.isLocked = false;
            player.isActivated = false;
            pinDirection = target.GetComponentInParent<CharacterPin>().PlayerPinLocations[player.playerNum - 1];
            StopCoroutine("MovePinAround");
            StartCoroutine("MovePinAround");
            target.GetComponentInParent<CharacterPin>().UnOwnPin();
        }
        else if (isLocked == false)
        {
            isActive = false;
            target.GetComponentInParent<CharacterPin>().UnOwnPin();
            CancelPlayer();
        }
    }
}
