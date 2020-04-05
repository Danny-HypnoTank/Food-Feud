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
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{

    [Header("Player Attributes")]
    [SerializeField]
    private float _moveSpeedModifier = 5;
    public float MoveSpeedModifier { get { return _moveSpeedModifier; } private set { _moveSpeedModifier = value; } }

    [SerializeField]
    private float weaponSplashMultiplier = 1;

    private readonly float gravity = 300.0f;

    public float MoveSpeed { get; private set; }
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
    private bool canDash;
    private Vector3 dashPosition;
    public bool IsDashing { get; private set; }

    [Header("Graphical Objects")]
    [SerializeField]
    private GameObject[] trail;
    [SerializeField]
    private ParticleSystem[] impactParticles;
    [SerializeField]
    private ParticleSystem _smokeParticles;
    [SerializeField]
    private Image fillBar;
    [SerializeField]
    private Sprite[] fillBarSprites;
    [SerializeField]
    private Image fillBarBg;
    [SerializeField]
    private Sprite[] fillBarBgSprites;
    [SerializeField]
    private GameObject scoreText;
    [SerializeField]
    private GameObject scoreTextInstance;
    int scoreAdditive = 0;
    [SerializeField]
    private GameObject _sImmunityObj;
    [SerializeField]
    private GameObject _iceCube;
    [SerializeField]
    private SpriteRenderer buffSprite;
    [SerializeField]
    private SpriteRenderer debuffSprite;

    [Header("Layer Masks")]
    [SerializeField]
    private LayerMask scoreLayer;

    //Fields
    private Vector3 moveInput;
    private ObjectAudioHandler audioHandler;
    private ExplodingPotato potato;

    //Auto Properties
    public float BaseSpeed { get; private set; }
    public float DashAmount { get; private set; }
    public DrawColor DrawColor { get; private set; }
    public PlayerBase PlayerBase { get; private set; }
    public DazeState PlayerStun { get; private set; }
    public BuffDebuff CurrentPowerup { get; private set; }
    public StunImmunity StunImmunityPowerup { get; private set; }
    public CharacterController chc { get; private set; }
    
    //Full properties
    private Vector3 _moveVelocity;
    public Vector3 MoveVelocity { get { return _moveVelocity; } private set { _moveVelocity = value; } }
    public GameObject IceCube { get { return _iceCube; } }
    public GameObject SImunnityObj { get { return _sImmunityObj; } }
    public ParticleSystem SmokeParticles { get { return _smokeParticles; } }
    public SpriteRenderer BuffSprite { get { return buffSprite; } }
    public SpriteRenderer DeBuffSprite { get { return debuffSprite; } }

    //MedalEvents
    public delegate int DashDelegate(int id);
    public event DashDelegate dashEvent;

    public delegate int StunOtherDelegate(int id);
    public event StunOtherDelegate stunOtherEvent;

    public delegate int CollectDelegate(int id);
    public event CollectDelegate collectEvent;

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
        DashAmount = 0;
        BaseSpeed = MoveSpeedModifier;
        MoveSpeed = Player.Speed;
        var smokeMain = SmokeParticles.main;
        smokeMain.startColor = Player.SkinColours[Player.skinId];

        fillBarBg.sprite = fillBarBgSprites[Player.skinId];
        fillBar.sprite = fillBarSprites[Player.skinId];

        for (int i = 0; i < impactParticles.Length; i++)
        {
            var impactMain = impactParticles[i].main;
            impactMain.startColor = Player.SkinColours[Player.skinId];
        }

        trail.ToggleGameObjects(false);
        UpdateFillBar();
        Splat();
    }

    private void Update()
    {

        if (ManageGame.instance.IsTimingDown == true)
        {
            if (PlayerStun.CanMove)
            {
                GetMovementInput();
                RunBuffDebuffLogic();
                Dash();
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
                    if (otherPlayer.PlayerStun.CanMove)
                    {
                        StunOtherCounter(Player.playerNum);

                        Player.StunCount++;
                        StartCoroutine(otherPlayer.PlayerStun.Stun(DashAmount,this));
                    }

                    Splat(DashAmount);
                    impactParticles[0].Play();

                    if (potato != null)
                        potato.OnHit(otherPlayer);
                }
            }
        }
        else if (other.CompareTag("ObjectCollider"))
        {
            if (IsDashing)
            {
                Splat(DashAmount);
                impactParticles[0].Play();
            }
        }
    }

    private void FixedUpdate()
    {
        _moveVelocity.y -= gravity * Time.fixedDeltaTime;

        if (IsDashing)
        {
            Vector3 direction = dashPosition - this.transform.position;
            Vector3 movement = direction.normalized * dashPower * Time.fixedDeltaTime;

            if (movement.sqrMagnitude > 0.1f)
                chc.transform.LookAt(chc.transform.position + movement);

            chc.Move(movement);
        }
        else if (!IsDashing)
        {
            if (PlayerStun.CanMove)
            {
                chc.Move(_moveVelocity * Time.fixedDeltaTime);

                if (moveInput.sqrMagnitude > 0.1f)
                    transform.rotation = Quaternion.LookRotation(moveInput);
            }
        }
    }

    private void GetMovementInput()
    {
        moveInput = new Vector3(Input.GetAxisRaw($"Horizontal{Player.playerNum}"), 0f, Input.GetAxisRaw($"Vertical{Player.playerNum}"));
        _moveVelocity = moveInput * (MoveSpeed + (_moveSpeedModifier));
    }

    private void RunBuffDebuffLogic()
    {
        if (CurrentPowerup != null)
            CurrentPowerup.OnUpdate(Time.deltaTime);
    }

    private void Dash()
    {
        if (Input.GetButton($"Dash{Player.playerNum}") && !IsDashing && PlayerStun.CanMove && canDash)
        {
            if (DashAmount < 1)
                DashAmount += Time.deltaTime;
            else
                DashAmount = 1;
            Player.DashCount++;

            

            UpdateFillBar();
        }

        if (Input.GetButtonUp($"Dash{Player.playerNum}") && !IsDashing && PlayerStun.CanMove && canDash)
        {
            float distanceToDash = 0;

            DashCounter(Player.playerNum);

            distanceToDash.CalculateFromPercentage(dashDistanceMin, dashDistanceMax, DashAmount);
            dashPower.CalculateFromPercentage(dashPowerMin, dashPowerMax, DashAmount);

            StartCoroutine(DashTimer(distanceToDash));
            StartCoroutine(DashCooldown());
        }

        if (!Input.GetButton($"Dash{Player.playerNum}"))
        {
            if (DashAmount > 0)
                DashAmount -= Time.deltaTime;
            else
                DashAmount = 0;

            UpdateFillBar();
        }
    }

    public void Splat(float multiplier = 1)
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (scoreTextInstance != null)
        {
            if (scoreTextInstance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("End") == true)
            {
                scoreAdditive = 0;
            }
        }

        if (Physics.Raycast(transform.position + Vector3.up, -transform.up, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("PaintableEnvironment"))
            {
                //audioHandler.SetSFX("Splat");

                PaintSizeMultiplier paintMultiplier = hit.collider.GetComponent<PaintSizeMultiplier>();
                if (multiplier < 0.5f)
                {
                    multiplier = 0.5f;
                }

                float _smult;
                if (paintMultiplier)
                    _smult = paintMultiplier.multiplier * multiplier;
                else
                    _smult = 1f * multiplier;

                int _id = Player.playerNum;
                for (int i = 0; i < 10; i++)
                {
                    DrawColor.DrawOnSplatmap(hit, _id, Player, _smult);
                }
            }
        }

        if (Physics.Raycast(ray, out hit, scoreLayer))
        {
            if (hit.collider.CompareTag("ScoreGrid"))
            {
                Collider[] squaresHit = Physics.OverlapSphere(hit.collider.gameObject.transform.position, 6 * multiplier);

                for (int i = 0; i < squaresHit.Length; i++)
                {
                    ScoreSquare square = squaresHit[i].GetComponent<ScoreSquare>();

                    if (square)
                    {
                        if (square.Value != Player.skinId)
                        {
                            square.SetValue(Player.skinId);
                            if (scoreTextInstance == null)
                            {
                                scoreTextInstance = Instantiate(scoreText, transform);
                                AddScore();
                            }
                            else
                            {
                                AddScore();
                            }
                        }

                    }

                }

            }
        }
    }

    void AddScore()
    {
        scoreAdditive++;
        scoreTextInstance.GetComponent<TextMeshPro>().text = "+" + scoreAdditive;
        scoreTextInstance.GetComponent<Animator>().SetTrigger("GoBack");
    }

    public void PickUpPowerUp(BuffDebuff powerup)
    {
        Player.PowerUpsCount++;

        CollectCounter(Player.playerNum);

        if (powerup is StunImmunity)
        {
            StunImmunityPowerup = (StunImmunity)powerup;
            StunImmunityPowerup.Start(this);
        }
        else
        {
            CurrentPowerup = powerup;
            if(CurrentPowerup != null)
                CurrentPowerup.Start(this);
        }
    }

    private IEnumerator DashTimer(float distance)
    {
        IsDashing = true;
        dashPosition = (this.transform.position) + (this.transform.forward * distance); //Unsure if even necessary

        PlayerBase.audioHandler.SetSFX("Whoosh");
        trail.ToggleGameObjects(true);

        yield return new WaitForSeconds(dashDuration);

        IsDashing = false;

        trail.ToggleGameObjects(false);
    }

    private IEnumerator DashCooldown()
    {
        canDash = false;

        float cooldown = 0;
        cooldown.CalculateFromPercentage(dashCooldownMin, dashCooldownMax, DashAmount);

        yield return new WaitForSeconds(cooldown);

        canDash = true;
    }

    private void UpdateFillBar()
    {
        if (fillBar != null)
            fillBar.fillAmount = DashAmount;
    }

    void DashCounter(int playerID)
    {
        Debug.Log(playerID);
        if (dashEvent != null)
        {
            dashEvent(playerID);

            if (dashEvent.GetInvocationList() != null)
            {
                Debug.Log(dashEvent.GetInvocationList());
            }
            else
            {
                Debug.Log("No Invocators");
            }
        }
    }

    void StunOtherCounter(int playerID)
    {
        Debug.Log(playerID);
        if (stunOtherEvent != null)
        {
            stunOtherEvent(playerID);

            if (stunOtherEvent.GetInvocationList() != null)
            {
                Debug.Log(stunOtherEvent.GetInvocationList());
            }
            else
            {
                Debug.Log("No Invocators");
            }
        }
    }

    void CollectCounter(int playerID)
    {
        Debug.Log(playerID);
        if (collectEvent != null)
        {
            collectEvent(playerID);

            if (collectEvent.GetInvocationList() != null)
            {
                Debug.Log(collectEvent.GetInvocationList());
            }
            else
            {
                Debug.Log("No Invocators");
            }
        }
    }
}