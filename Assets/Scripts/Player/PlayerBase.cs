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
    private int animationID;
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
            animationID = 3;
            expression.SetExpression(4);
        }
        else if (Daze.Stunned == true) //Check to see if player is stunned, if they are set animation to stun animation
        {
            animationID = 2;
            expression.SetExpression(1);
        }
        else if (Daze.Frozen == true)
        {
            animationID = 4;
            expression.SetExpression(1);
        }
        else if (input != 0 & Daze.Stunned == false & Daze.Frozen == false & pCon.IsDashing == false) //Check to see if player is moving
        {
            animationID = 1;
            expression.SetExpression(0);
        }
        else if (input == 0) // if there is no movement input then run the idle animation
        {
            resetAnimTimer += Time.deltaTime;
            if (resetAnimTimer > 0.1f)
            {
                animationID = 0;
                expression.SetExpression(0);
            }
        }
    }

    private void AnimationManagement()
    {
        if (ManageGame.instance.IsTimingDown == true)
        {
            animator.SetInteger("AnimController", animationID);
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