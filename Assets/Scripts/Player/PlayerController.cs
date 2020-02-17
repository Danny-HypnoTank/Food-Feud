/* Created by:
 * Name: Danny Pym-Hember
 * SID: 1513999
 * Date Created: 29/09/2019
 * Last Modified 12/02/2020
 * Modified By: Dominik Waldowski,
 *              Danny Pym-Hember,
 *              James Sturdgess
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Player Attributes")]
    [SerializeField]
    private float _moveSpeedModifier = 5;
    //TODO: Add method to modify this property so it's not left exposed - public set BAD
    public float MoveSpeedModifier { get { return _moveSpeedModifier; } set { _moveSpeedModifier = value; } }
    [SerializeField]
    private float weaponSplashMultiplier = 1;

    private readonly float gravity = 300.0f;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    //TODO: Add methods to modify these properties so they're not left exposed - public set BAD
    public float MoveSpeed { get; set; }
    public Player Player { get; set; }

    [Header("Dash Settings")]
    [SerializeField]
    private float dashDuration;          //if dash duration too small it causes animation glitch
    [SerializeField]
    private float dashPowerMin;
    [SerializeField]
    private float dashPowerMax;
    [SerializeField]
    private float dashDistanceMin;      //Unsure if even necessary
    [SerializeField]
    private float dashDistanceMax;      //Unsure if even necessary
    [SerializeField]
    private float dashCooldownMin;
    [SerializeField]
    private float dashCooldownMax;

    private float dashPower;
    private float dashAmount;
    private bool canDash;
    private Vector3 dashPosition;
    public bool IsDashing { get; private set; }

    [Header("Graphical Objects")]
    [SerializeField]
    private GameObject[] trail;
    [SerializeField]
    private Image fillBar;

    [Header("Layer Masks")]
    [SerializeField]
    private LayerMask scoreLayer;

    //Fields
    private ObjectAudioHandler audioHandler;
    private CharacterController chc;

    //Auto Properties
    public DrawColor DrawColor { get; private set; }
    public PlayerBase PlayerBase { get; private set; }
    public DazeState PlayerStun { get; private set; }

    private void Awake()
    {
        chc = GetComponent<CharacterController>();
        PlayerStun = GetComponent<DazeState>();
        PlayerBase = GetComponent<PlayerBase>();
        audioHandler = GetComponent<ObjectAudioHandler>();
        DrawColor = ManageGame.instance.GetComponent<DrawColor>();
    }

    private void Start()
    {
        IsDashing = false;
        canDash = true;
        dashAmount = 0;
        MoveSpeed = Player.Speed;

        ToggleTrails(false);
        UpdateFillBar();
        Splat();
    }

    private void Update()
    {
        if (ManageGame.instance.IsTimingDown == true)
        {
            if (!PlayerStun.Stunned)
            {
                moveInput = new Vector3(Input.GetAxisRaw($"Horizontal{Player.playerNum}"), 0f, Input.GetAxisRaw($"Vertical{Player.playerNum}"));
                moveVelocity = moveInput * (MoveSpeed + (_moveSpeedModifier));

                if (Input.GetButton($"Dash{Player.playerNum}") && !IsDashing && !PlayerStun.Stunned && canDash)
                {
                    if (dashAmount < 1)
                        dashAmount += Time.deltaTime;
                    else
                        dashAmount = 1;

                    UpdateFillBar();
                }

                if(Input.GetButtonUp($"Dash{Player.playerNum}") && !IsDashing && !PlayerStun.Stunned && canDash)
                {
                    float distanceToDash = 0;
                        
                    distanceToDash.CalculateFromPercentage(dashDistanceMin, dashDistanceMax, dashAmount);
                    dashPower.CalculateFromPercentage(dashPowerMin, dashPowerMax, dashAmount);

                    StartCoroutine(DashTimer(distanceToDash));
                    StartCoroutine(DashCooldown());
                }

                if (!Input.GetButton($"Dash{Player.playerNum}"))
                {
                    if (dashAmount > 0)
                        dashAmount -= Time.deltaTime;
                    else
                        dashAmount = 0;

                    UpdateFillBar();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsDashing)
            {
                SecondaryObjCollector objCollector = other.GetComponent<SecondaryObjCollector>();
                if (objCollector.HasSecondaryObj && objCollector.SecondaryObj != null)
                    objCollector.DropSecondaryObj();

                PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
                if (!otherPlayer.IsDashing)
                {
                    if (!otherPlayer.PlayerStun.Stunned)
                        StartCoroutine(otherPlayer.PlayerStun.Stun(dashAmount));

                    Splat();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        moveVelocity.y -= gravity * Time.fixedDeltaTime;

        if (IsDashing)
        {
            Vector3 direction = dashPosition - this.transform.position;
            Vector3 movement = direction.normalized * dashPower * Time.fixedDeltaTime;

            if(movement.sqrMagnitude > 0.1f)
                chc.transform.LookAt(chc.transform.position + movement);

            chc.Move(movement);
        }
        else if (!IsDashing)
        {
            if (!PlayerStun.Stunned)
            {
                chc.Move(moveVelocity * Time.fixedDeltaTime);

                if (moveInput.sqrMagnitude > 0.1f)
                    transform.rotation = Quaternion.LookRotation(moveInput);
            }
        }
    }

    public void Splat()
    {

        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(transform.position + Vector3.up, -transform.up, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("PaintableEnvironment"))
            {
                //audioHandler.SetSFX("Splat");

                PaintSizeMultiplier paintMultiplier = hit.collider.GetComponent<PaintSizeMultiplier>();

                float _smult;
                if (paintMultiplier)
                    _smult = paintMultiplier.multiplier * weaponSplashMultiplier;
                else
                    _smult = 1f * weaponSplashMultiplier;

                int _id = Player.playerNum;
                for(int i = 0; i < 10; i++)
                {
                    DrawColor.DrawOnSplatmap(hit, _id, Player, _smult);
                }
            }
        }

        if(Physics.Raycast(ray, out hit, scoreLayer))
        {
            if(hit.collider.CompareTag("ScoreGrid"))
            {
                ScoreSquare square = hit.collider.GetComponent<ScoreSquare>();

                if(square.Value != Player.skinId)
                    square.SetValue(Player.skinId);
            }
        }
    }

    private IEnumerator DashTimer(float distance)
    {
        IsDashing = true;
        dashPosition = (this.transform.position) + (this.transform.forward * distance); //Unsure if even necessary

        PlayerBase.audioHandler.SetSFX("Whoosh");
        ToggleTrails(true);

        yield return new WaitForSeconds(dashDuration);

        IsDashing = false;

        ToggleTrails(false);
    }

    private IEnumerator DashCooldown()
    {
        canDash = false;

        float cooldown = 0;
        cooldown.CalculateFromPercentage(dashCooldownMin, dashCooldownMax, dashAmount);

        yield return new WaitForSeconds(cooldown);

        canDash = true;
    }

    private void ToggleTrails(bool value)
    {
        foreach (GameObject t in trail)
        {
            t.SetActive(value);
        }
    }

    private void UpdateFillBar()
    {
        if (fillBar != null)
            fillBar.fillAmount = dashAmount;
    }
}