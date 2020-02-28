/*
 * Created by:
 * Name: James Sturdgess
 * Sid: 1314371
 * Date Created: 04/10/2019
 * Last Modified 19/10/2019
 * Modified By: Dominik Waldowski, Danny Pym-Hember
 */
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    #region Variables and References
    [SerializeField]
    private Animator animator;
    public Player Player { get; set; }
    public Animator Animator { get => animator; set => animator = value; }
    public PlayerController pCon { get; private set; }
    public DrawColor DrawColor { get; private set; }
    private ObjectPooling bombPool;
    public DazeState Daze { get; private set; }
    public ObjectAudioHandler audioHandler { get; private set; }
    [SerializeField]
    private GameObject[] skins;
    [SerializeField]
    private int animationIDSet;
    [SerializeField]
    private int currentWeaponId = 0;
    private float resetAnimTimer;
    private ExpressionManager expression;
    #endregion

    private void OnEnable()
    {
        if (Player != null)
        {
            Player.Speed = Player.DefaultSpeed;
        }
    }

    public void SetExpressionManager(int idToPass)
    {
        expression = skins[idToPass].GetComponent<ExpressionManager>();
    }

    private void Start()
    {
        Daze = GetComponent<DazeState>();
        pCon = GetComponent<PlayerController>();
        audioHandler = GetComponent<ObjectAudioHandler>();
        bombPool = GameObject.Find("GameManager").GetComponent<ObjectPooling>();
        DrawColor = GameObject.Find("GameManager").GetComponent<DrawColor>();
    }

    private void Update()
    {
        checkInput();
        if (animator != null)
        {
            AnimationManagement();
        }
    }

    //Checking Input to then change animations and Expressions
    private void checkInput()
    {
        float moveInputH = Input.GetAxisRaw($"Horizontal{Player.playerNum}");
        float moveInputV = Input.GetAxisRaw($"Vertical{Player.playerNum}");
        float input = moveInputH + moveInputV;

        if (pCon.IsDashing == true) //Check to see if player is dashing, if they are set animation to dash anim and Set expresiion
        {
            animationIDSet = 3;
            expression.SetExpression(4);
        }
        else if (Daze.Stunned == true) //Check to see if player is stunned, if they are set animation to stun animation
        {
            animationIDSet = 2;
            expression.SetExpression(1);
        }
        /*else if (isWeaponSwitch == true) //Pickup Animations
        {
            if (currentWeaponId == 1 || currentWeaponId == 3)
            {
                //ricochet and bomb anim id = 11
                animationIDSet = 15;
                resetAnimTimer = 0;
                expression.SetExpression(1);
            }
            else if (currentWeaponId == 2)
            {
                //twin spray anim id = 21
                animationIDSet = 25;
                resetAnimTimer = 0;
                expression.SetExpression(2);
            }
            else if (currentWeaponId == 0)
            {
                //default anim id = 1
                animationIDSet = 1;
                resetAnimTimer = 0;
                expression.SetExpression(0);
            }
        }*/
        else if (input != 0) //Check to see if player is moving
        {

            expression.SetExpression(0);
            if (currentWeaponId == 1) //If they are holding Bomb Launcher whilst walking
            {
                //bomb walk anim id = 5
                animationIDSet = 5;
                resetAnimTimer = 0;
            }
            else if (currentWeaponId == 2) //if they are holding Twin Spray whilst walking
            {
                //twin spray walk anim id = 6
                animationIDSet = 6;
                resetAnimTimer = 0;
            }
            else if (currentWeaponId == 0 || currentWeaponId == 3) //if they are holding Default weapon or Ricochet Weapon
            {
                //default and ricochet walk anim id = 1
                animationIDSet = 1;
                resetAnimTimer = 0;
            }
        }
        else if (input == 0) // if there is no movement input then run the idle animation
        {
            resetAnimTimer += Time.deltaTime;
            if (resetAnimTimer > 0.1f)
            {
                animationIDSet = 0;
                expression.SetExpression(0);
            }
        }
    }

    private void AnimationManagement()
    {
        if (ManageGame.instance.IsTimingDown == true)
        {
            switch (animationIDSet)
            {
                case 0: //Idle Animation
                    animator.SetInteger("AnimController", 0);
                    break;
                case 1: //Default and Ricochet Walking Animations
                    animator.SetInteger("AnimController", 1);
                    break;
                case 2: //Stun Animation
                    animator.SetInteger("AnimController", 2);
                    break;
                case 3: //Dash Animation
                    animator.SetInteger("AnimController", 3);
                    break;
                case 4: //Deploy God Power Up Animation
                    animator.SetInteger("AnimController", 4);
                    break;
                case 5: //Grenade Launcher Walk Animation
                    animator.SetInteger("AnimController", 5);
                    break;
                case 6: //Twin Spray Walk Animation
                    animator.SetInteger("AnimController", 6);
                    break;
            }
        }
        else
        {
            animator.SetInteger("AnimController", 0); // Else be in idle animation
        }

    }

    public void SetSkin(int skinId)
    {
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].gameObject.SetActive(false);
        }
        skins[skinId].gameObject.SetActive(true);
        if (skins[skinId].GetComponent<Animator>() != null)
        {
            animator = skins[skinId].GetComponent<Animator>();
            animator.enabled = true;
            animator.SetInteger("AnimController", 0);
        }
    }

    private void OnDisable()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }
    }
}