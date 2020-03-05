using UnityEngine;

/// <summary>
/// Enum to determine the state of the UI Element
/// </summary>
public enum UIElementState
{
    inactive,
    hover,
    pressed
}

/// <summary>
/// Class for controlling UI Elements
/// </summary>
public class UIElementController : MonoBehaviour
{

    [Header("Properties")]
    [SerializeField]
    private float scaleMultiplier;
    [SerializeField]
    private float lerpDuration;
    [SerializeField]
    private float lerpAngle;

    [Header("Graphics")]
    [SerializeField]
    private Sprite inactiveSprite;
    [SerializeField]
    private Sprite activeSprite;

    private float timer;
    private int direction;
    private Vector3 antiClockwiseLoc;
    private Vector3 clockwiseLoc;
    private UIElementState state;
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        //Initialisation
        spriteRender = GetComponent<SpriteRenderer>();
        antiClockwiseLoc = new Vector3(0, 180, -lerpAngle);
        clockwiseLoc = new Vector3(0, 180, lerpAngle);
        timer = 0;

        //Default state to inactive
        ChangeState(UIElementState.inactive);
    }

    private void Update()
    {
        //Run the hover action method if the state is hover
        if (state == UIElementState.hover)
            HoverAction();
    }

    /// <summary>
    /// Method to change the state of the UI Element
    /// </summary>
    /// <param name="newState">The state to change to (<see cref="UIElementState.inactive"/>, <see cref="UIElementState.hover"/> or <see cref="UIElementState.pressed"/>)</param>
    public void ChangeState(UIElementState newState)
    {
        //Set the state to the new state
        state = newState;

        //Run a method to change the appearance of the UI Element or log an error in the debug window
        switch (state)
        {
            case UIElementState.inactive:
                SetInactive();
                break;
            case UIElementState.hover:
                SetHover();
                break;
            case UIElementState.pressed:
                SetPressed();
                break;
            default:
                Debug.LogError("You broke something with the main menu UI, you silly, silly person.");
                break;
        }
    }

    private void SetInactive()
    {
        //Reset the scale/angle of the UI Element and set the sprite to the inactive sprite
        transform.localScale = Vector3.one;
        transform.eulerAngles = new Vector3(0, 180, 0);
        ChangeSprite(inactiveSprite);
    }

    private void SetHover()
    {
        //Scale the UI Element by the scale multiplier and set the sprite to the active sprite
        transform.localScale *= scaleMultiplier;
        ChangeSprite(activeSprite);
    }

    private void SetPressed()
    {
        //Reset the angle of the UI Element and scale it down from the original size by the scale multiplier
        transform.localScale = Vector3.one / scaleMultiplier;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private void ChangeSprite(Sprite sprite)
    {
        if (spriteRender.sprite != sprite)
            spriteRender.sprite = sprite;
    }

    private void HoverAction()
    {
        //Increment timer by deltaTime
        timer += Time.deltaTime;

        //If timer is greater than, or equal to, the duration of the lerp recalculate direction and reset the timer
        if (timer > lerpDuration)
        {
            direction = (direction + 1) % 2; //Returns 0 on an even number or 1 on an odd number, e.g. (0 + 1) % 2 = 1 or (1 + 1) % 2 = 0
            timer = 0;
        }

        //If state = 0 lerp the rotation clockwise, else if state = 1 lerp the rotation anti-clockwise
        if (direction == 0)
        {
            transform.eulerAngles = Vector3.Lerp(antiClockwiseLoc, clockwiseLoc, timer / lerpDuration);
        }
        else if (direction == 1)
        {
            transform.eulerAngles = Vector3.Lerp(clockwiseLoc, antiClockwiseLoc, timer / lerpDuration);
        }
    }

}
