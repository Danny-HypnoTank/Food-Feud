/*
 * Created by:
 * Name: Danny Pym-Hember
 * Sid: 1513999
 * Date Created: 29/09/2019
 * Last Modified 06/10/2019
 * Modified By: Dominik Waldowski, Danny Pym-Hember
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables and References:
    private float gravity = 300.0f;                      //stores value for gravity
    [SerializeField]
    private float moveSpeedModifier = 5;

    private CharacterController chc;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Vector3 lookInput;

    private DefaultShooting dShooting;
    public DazeState pStunned { get; private set; }
    private PlayerBase playerBase;
    [SerializeField]
    private float dashDuration = 0.2f;          //if dash duration too small it causes animation glitch
    private float dashPower = 20;
    private float dashDistance = 5;
    private Vector3 dashPosition;
    [SerializeField]
    private GameObject[] trail;

    [SerializeField]
    private bool rotationLockOption;

    [SerializeField]
    private float weaponSplashMultiplier = 1;

    private Player player;                  //stores player data

    public Player Player { get => player; set => player = value; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }

    private bool isDashing;

    private Ray ray;
    private DrawColor drawColor;
    public DrawColor DrawColor { get { return drawColor; } private set { drawColor = value; } }

    public float MoveSpeedModifier { get => moveSpeedModifier; set => moveSpeedModifier = value; }

    private void Start()
    {
        foreach(GameObject t in trail)
        {

            t.gameObject.SetActive(false);

        }
        isDashing = false;
        chc = GetComponent<CharacterController>();
        dShooting = GetComponent<DefaultShooting>();
        pStunned = GetComponent<DazeState>();
        playerBase = GetComponent<PlayerBase>();
        drawColor = GameObject.Find("GameManager").GetComponent<DrawColor>();

        Splat(20);
    }

    private void Update()
    {
        //Player Movement with joysticks or keyboard
        if (ManageGame.instance.IsTimingDown == true)
        {

            if (!pStunned.Stunned)
            {

                moveInput = new Vector3(Input.GetAxisRaw("Horizontal" + player.playerNum), 0f, Input.GetAxisRaw("Vertical" + player.playerNum));
                moveVelocity = moveInput * (player.Speed + (moveSpeedModifier));


                if (Input.GetButtonDown("Dash" + player.playerNum) && isDashing == false && pStunned.Stunned == false)
                {

                    dashPosition = (this.transform.position) + (this.transform.forward * dashDistance);
                    isDashing = true;
                    pStunned.CanShoot = false;
                    foreach (GameObject t in trail)
                    {

                        t.gameObject.SetActive(true);

                    }
                    StartCoroutine(DashTimer());
                }
                else
                {
                    moveVelocity = moveInput * (player.Speed + (moveSpeedModifier));
                }

                //Player rotations with twin sticks
                //lookInput = new Vector3(Input.GetAxisRaw("RHorizontal" + player.playerNum), 0f, Input.GetAxisRaw("RVertical" + player.playerNum));

                //Player Rotation with only the left stick
                if (rotationLockOption == true)
                {
                    if (moveInput.sqrMagnitude > 0.1f)
                    {
                        transform.rotation = Quaternion.LookRotation(moveInput);
                    }
                }

                if (rotationLockOption == false)
                {
                    if (moveInput.sqrMagnitude > 0.1f)
                    {
                        if (!Input.GetButton("Shoot" + player.playerNum))
                        {
                            transform.rotation = Quaternion.LookRotation(moveInput);
                        }
                    }
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isDashing)
            {

                PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();

                if (!otherPlayer.IsDashing)
                {

                    if (!otherPlayer.pStunned.Stunned)
                        StartCoroutine(otherPlayer.pStunned.Stun(otherPlayer.Player));

                    Splat(20);

                }

            }
        }
    }

    private void FixedUpdate()
    {
        moveVelocity.y -= gravity * Time.deltaTime;
        if (isDashing == true)
        {
            // Dashing();
            Vector3 direction = dashPosition - this.transform.position;
            Vector3 movement = direction.normalized * dashPower * Time.deltaTime;
            if(movement.sqrMagnitude > 0.1f)
                chc.transform.LookAt(chc.transform.position + movement);

            chc.Move(movement);
        }
        else if (isDashing == false)
        {
            chc.Move(moveVelocity * Time.deltaTime);
        }
    }

    public void Splat(float size)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up, Color.green, 90);
        if (Physics.Raycast(transform.position + Vector3.up, -transform.up, out hit))
        {

            if (hit.collider.gameObject.tag == "PaintableEnvironment")
            {
                float _smult;

                if (hit.collider.GetComponent<PaintSizeMultiplier>())
                {
                    _smult = hit.collider.GetComponent<PaintSizeMultiplier>().multiplier * weaponSplashMultiplier;
                }
                else
                {
                    _smult = 1f * weaponSplashMultiplier;
                }

                int _id = Player.skinId;
                for(int i = 0; i < 10; i++)
                {
                    DrawColor.DrawOnSplatmap(hit, _id, player, _smult);
                }

            }

        }
    }

    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashDuration);
        foreach (GameObject t in trail)
        {

            t.gameObject.SetActive(false);

        }
        isDashing = false;
        if (pStunned.Stunned == false)
        {
            pStunned.CanShoot = true;
        }
    }
}