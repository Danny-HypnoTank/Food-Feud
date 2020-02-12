/*
 * Created by:
 * Name: Danny Pym-Hember
 * Sid: 1513999
 * Date Created: 29/09/2019
 * Last Modified 06/10/2019
 * Modified By: Dominik Waldowski, Danny Pym-Hember
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Variables and References:
    private readonly float gravity = 300.0f;                      //stores value for gravity
    [SerializeField]
    private float moveSpeedModifier = 5;

    private CharacterController chc;

    private Vector3 moveInput;
    private Vector3 moveVelocity;

    public DazeState PlayerStun { get; private set; }
    private PlayerBase playerBase;

    [Header("Dash Settings")]
    [SerializeField]
    private float dashDuration;          //if dash duration too small it causes animation glitch
    [SerializeField]
    private float dashPowerMin;
    private float dashPower;
    [SerializeField]
    private float dashPowerMax;
    [SerializeField]
    private float dashDistanceMin;
    [SerializeField]
    private float dashDistanceMax;
    [SerializeField] private float dashCooldownTime;
    private Vector3 dashPosition;
    private bool canDash;
    private float dashAmount;

    [SerializeField]
    private GameObject[] trail;

    [SerializeField]
    private float weaponSplashMultiplier = 1;

    private Player player;                  //stores player data

    public Player Player { get => player; set => player = value; }
    public bool IsDashing { get; private set; }

    private ObjectAudioHandler audioHandler;

    private bool isDashing;

    private DrawColor drawColor;
    public DrawColor DrawColor { get { return drawColor; } private set { drawColor = value; } }

    public float MoveSpeedModifier { get => moveSpeedModifier; set => moveSpeedModifier = value; }
    public float MoveSpeed { get; set; }

    [SerializeField]
    private Image fillBar;

    private void Start()
    {
        isDashing = false;
        canDash = true;
        dashAmount = 0;
        MoveSpeed = Player.Speed;

        chc = GetComponent<CharacterController>();
        PlayerStun = GetComponent<DazeState>();
        playerBase = GetComponent<PlayerBase>();
        audioHandler = GetComponent<ObjectAudioHandler>();
        drawColor = ManageGame.instance.GetComponent<DrawColor>();

        ToggleTrails(false);
        UpdateFillBar();
        Splat();
    }

    private void Update()
    {
        //Player Movement with joysticks or keyboard
        if (ManageGame.instance.IsTimingDown == true)
        {

            if (!PlayerStun.Stunned)
            {

                moveInput = new Vector3(Input.GetAxisRaw($"Horizontal{player.playerNum}"), 0f, Input.GetAxisRaw($"Vertical{player.playerNum}"));
                moveVelocity = moveInput * (MoveSpeed + (moveSpeedModifier));


                if (Input.GetButton($"Dash{player.playerNum}") && !isDashing && !PlayerStun.Stunned && canDash)
                {

                    if (dashAmount < 1)
                        dashAmount += Time.deltaTime;
                    else
                        dashAmount = 1;

                    UpdateFillBar();

                }
                if(Input.GetButtonUp($"Dash{player.playerNum}") && !isDashing && !PlayerStun.Stunned && canDash)
                {

                    float distanceToDash = (dashAmount * (dashDistanceMax - dashDistanceMin)) + dashDistanceMin;
                    dashPower = (dashAmount * (dashPowerMax - dashPowerMin)) + dashPowerMin;

                    StartCoroutine(DashTimer(distanceToDash));
                    StartCoroutine(DashCooldown());

                }
                if(!Input.GetButton($"Dash{player.playerNum}"))
                {

                    if (dashAmount > 0)
                        dashAmount -= Time.deltaTime;
                    else
                        dashAmount = 0;

                    UpdateFillBar();

                }
                else
                {
                    moveVelocity = moveInput * (player.Speed + (moveSpeedModifier));
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isDashing)
            {

                PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
                if (other.GetComponent<SecondaryObjCollector>().HasSecondaryObj == true &&
                   other.GetComponent<SecondaryObjCollector>().SecondaryObj != null)
                {
                    other.GetComponent<SecondaryObjCollector>().DropSecondaryObj();
                }

                if (!otherPlayer.IsDashing)
                {

                    if (!otherPlayer.PlayerStun.Stunned)
                        StartCoroutine(otherPlayer.PlayerStun.Stun(otherPlayer.Player));

                    Splat();

                }

            }
        }
    }

    private void FixedUpdate()
    {
        moveVelocity.y -= gravity * Time.fixedDeltaTime;
        if (isDashing == true)
        {
            // Dashing();
            Vector3 direction = dashPosition - this.transform.position;
            Vector3 movement = direction.normalized * dashPower * Time.fixedDeltaTime;
            if(movement.sqrMagnitude > 0.1f)
                chc.transform.LookAt(chc.transform.position + movement);

            chc.Move(movement);
        }
        else if (isDashing == false)
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

                int _id = Player.skinId;
                for(int i = 0; i < 10; i++)
                {
                    DrawColor.DrawOnSplatmap(hit, _id, player, _smult);
                }

            }

        }
    }

    private IEnumerator DashTimer(float distance)
    {


        Debug.Log(distance);
        isDashing = true;
        PlayerStun.CanShoot = false;
        playerBase.audioHandler.SetSFX("Whoosh");
        dashPosition = (this.transform.position) + (this.transform.forward * distance);
        ToggleTrails(true);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        ToggleTrails(false);
        if (PlayerStun.Stunned == false)
        {
            PlayerStun.CanShoot = true;
        }
    }

    private IEnumerator DashCooldown()
    {

        canDash = false;
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;

    }

    private void ToggleTrails(bool value)
    {

        foreach (GameObject t in trail)
        {

            t.gameObject.SetActive(value);

        }

    }

    public void UpdateFillBar()
    {
        if (fillBar != null)
        {
            float newValue = dashAmount;

            fillBar.fillAmount = newValue;
        }
    }

}