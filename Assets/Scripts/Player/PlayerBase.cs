/*
 * Created by:
 * Name: James Sturdgess
 * Sid: 1314371
 * Date Created: 04/10/2019
 * Last Modified 19/10/2019
 * Modified By: Dominik Waldowski, Danny Pym-Hember
 */
using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
#region Variables and References
    [SerializeField]
    private Animator animator;
    private Player player;
    public Player Player { get => player; set => player = value; }
    public Powerup CurrentPowerUp { get => currentPowerUp; set => currentPowerUp = value; }
    public GameObject[] Weapons { get => weapons; set => weapons = value; }
    public bool GodModeKnockback { get => godModeKnockback; set => godModeKnockback = value; }
    public Animator Animator { get => animator; set => animator = value; }

    public PlayerController pCon {get; private set;}

    private DrawColor drawColor;
    public DrawColor DrawColor { get { return drawColor; } private set { drawColor = value; } }

    private ObjectPooling bombPool;
    [SerializeField]
    private Powerup currentPowerUp;
    public DazeState Daze { get; private set; }
    public ObjectAudioHandler audioHandler { get; private set; }
    private Shooting shooting;

    [SerializeField]
    private GameObject[] weapons;
    [SerializeField]
    private GameObject[] skins;
    [SerializeField]
    private Transform[] weaponOffsetPos, weaponOffsetTwinGun;
    [Header("Offset")]
    [SerializeField]
    private Transform gunHolder, twinGunLeft, twinGunRight;
    private Vector3 currentPos;
    private Vector3 previousPos;

    private bool godModeKnockback;
    private float godPowerTimer;
    private float godPowerLimit = 10;
    [SerializeField]
    private int animationIDSet;
    [SerializeField]
    private int currentWeaponId = 0;
    private float resetAnimTimer;
    private bool isWeaponSwitch = false;
    private float weaponPoolTimer = 1.0f;
    private ExpressionManager expression;
    #endregion

    private void OnEnable()
    {
        ResetGodMode();
        if (player != null)
        {
            player.Speed = player.DefaultSpeed;
        }
    }

    public void SetExpressionManager(int idToPass)
    {
        expression = skins[idToPass].GetComponent<ExpressionManager>();
    }

    public void RemoveGodPower()
    {
        Debug.Log("GodMode removed!");
        /*Vector3 abovePlayer = this.transform.position + (Vector3.up * 2);
        ManageGame.instance.GodPowerUp.transform.position = abovePlayer;*/
        ManageGame.instance.GodPowerUp.SetActive(true);
        ManageGame.instance.GodPowerUp.GetComponent<GodPowerUpMovement>().ChooseRandomNode();
    }

    public void LoseGodPower()
    {
        //ManageGame.instance.GodPowerUp.transform.position = this.transform.position;
        ManageGame.instance.GodPowerUp.SetActive(true);
        ManageGame.instance.GodPowerUp.GetComponent<GodPowerUpMovement>().ChooseRandomNode();
    }

    private void Start()
    {
        Daze = this.gameObject.GetComponent<DazeState>();
        bombPool = GameObject.Find("GameManager").GetComponent<ObjectPooling>();
        shooting = this.gameObject.GetComponent<Shooting>();
        drawColor = GameObject.Find("GameManager").GetComponent<DrawColor>();
        pCon = GetComponent<PlayerController>();
        audioHandler = GetComponent<ObjectAudioHandler>();
    }

    private void Update()
    {
        OffsetFixWeapon();
        checkInput();
        if (animator != null)
        {
            AnimationManagement();
        }
    }

    //Setting Weapon Position to position of arm for offset
    private void OffsetFixWeapon()
    {
        if (currentWeaponId == 2)
        {
            twinGunLeft.transform.position = weaponOffsetTwinGun[player.skinId].transform.position;
            twinGunRight.transform.position = weaponOffsetPos[player.skinId].transform.position;
        }
        else
        {

            weapons[currentWeaponId].transform.position = weaponOffsetPos[player.skinId].transform.position;
        }
    }

    //Checking Input to then change animations and Expressions
    private void checkInput()
    {
        float moveInputH = Input.GetAxisRaw("Horizontal" + player.playerNum);
        float moveInputV = Input.GetAxisRaw("Vertical" + player.playerNum);
        float input = moveInputH + moveInputV;

        if (this.gameObject.GetComponent<PlayerController>().IsDashing == true) //Check to see if player is dashing, if they are set animation to dash anim and Set expresiion
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
            if (currentWeaponId == 1 ) //If they are holding Bomb Launcher whilst walking
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
            if (CurrentPowerUp != null)
                if (CurrentPowerUp.powerUpPower == Powerup.powerUps.godpowerup)
                    animationIDSet = 4;
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

    public void DefaultWeaponSet()
    {
        currentWeaponId = 0;
        weapons[currentWeaponId].transform.position = weaponOffsetPos[0].transform.position;
        weapons[0].SetActive(true);
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

    public void SetPowerUp(Powerup powerGained)
    {
        CurrentPowerUp = powerGained;
        if (CurrentPowerUp.powerUpPower != Powerup.powerUps.godpowerup)
            audioHandler.SetSFX("Pickup");
        else
            audioHandler.SetSFX("Boom");
        UsePower();
    }

    private void UsePower()
    {
        if (currentPowerUp.powerUpPower == Powerup.powerUps.powerups)
        {
            if (currentPowerUp.powerUpId == 0)
            {
                ResetOtherPlayerAmmo();
            }
            else if (currentPowerUp.powerUpId == 1)
            {
                ResetOtherPlayerWeapons();
            }
            else if (currentPowerUp.powerUpId == 2)
            {
                MassFreeze();
            }
        }
        else if (currentPowerUp.powerUpPower == Powerup.powerUps.weapons)
        {
            ResetWeapon();
            StopCoroutine("PullOutWeapon");
            StartCoroutine("PullOutWeapon");
        }
        else if (currentPowerUp.powerUpPower == Powerup.powerUps.godpowerup)
        {
            pCon.Splat();
            animationIDSet = 4;
            resetAnimTimer = 0;
            //StartTimer();

        }
        currentPowerUp = null;
        //ManageGame.instance.PowerIconUpdate(player.playerNum, 0);
    }

#region Coroutines
    private IEnumerator PullInWeapon()
    {

        yield return null;
    }

    private IEnumerator PullOutWeapon()
    {
        isWeaponSwitch = true;
        currentWeaponId = currentPowerUp.weaponID;
        if (currentWeaponId == 2)
        {
            twinGunLeft.transform.position = weaponOffsetTwinGun[player.skinId].transform.position;
            twinGunRight.transform.position = weaponOffsetPos[player.skinId].transform.position;
        }
        else
        {
            weapons[currentPowerUp.weaponID].transform.position = weaponOffsetPos[player.skinId].transform.position;
        }
        weapons[currentPowerUp.weaponID].SetActive(true);
        yield return new WaitForSeconds(weaponPoolTimer);
        isWeaponSwitch = false;
        yield return null;
    }
    #endregion

#region Reset/Power Up Methods
    private void ResetOtherPlayerWeapons()
    {
        for (int i = 0; i < ManageGame.instance.PlayerObjects.Count; i++)
        {
            if (ManageGame.instance.PlayerObjects[i].GetComponent<PlayerBase>().Player.playerNum != player.playerNum)
            {
                ManageGame.instance.PlayerObjects[i].GetComponent<PlayerBase>().ResetWeapon();
                ManageGame.instance.PlayerObjects[i].GetComponent<PlayerBase>().Weapons[0].SetActive(true);
            }
        }
    }

    private void ResetOtherPlayerAmmo()
    {
        for (int i = 0; i < ManageGame.instance.PlayerObjects.Count; i++)
        {
            if (ManageGame.instance.PlayerObjects[i].GetComponent<PlayerBase>().Player.playerNum != player.playerNum)
            {
                ManageGame.instance.PlayerObjects[i].GetComponent<Shooting>().Ammo = 0;
            }
        }
    }

    private void MassFreeze()
    {
        for (int i = 0; i < ManageGame.instance.PlayerObjects.Count; i++)
        {
            if (ManageGame.instance.PlayerObjects[i].GetComponent<PlayerBase>().Player.playerNum != player.playerNum)
            {
                //ManageGame.instance.PlayerObjects[i].GetComponent<DazeState>().StartCoroutine(ManageGame.instance.PlayerObjects[i].GetComponent<DazeState>().Stun(ManageGame.instance.PlayerObjects[i].GetComponent<PlayerBase>().Player));
            }
        }
    }

    public void ResetWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
    }

    public void ResetGodMode()
    {
        godPowerTimer = 0;
    }
#endregion
}


#region Bomb Launch Code
/*GameObject bomb = bombPool.GetPooledObject();
bomb.transform.rotation = shooting.GunMuzzle.transform.rotation;
bomb.transform.position = shooting.GunMuzzle.transform.position;
bomb.GetComponent<Bomb>().SetParent(this);
bomb.GetComponent<Bomb>().decal = currentPowerUp.powerupObject;
bomb.SetActive(true);*/
#endregion

//Code for speed up power up
//StartCoroutine(daze.SpeedUp(player, 15));
#region old Code
/*
 * characterControl = GetComponent<PlayerController>();
        shooting = GetComponent<Shooting>();
        if (useOldSystem == false)
        {
            characterControl.enabled = false;
            shooting.enabled = false;
        }
        OriginalSpeed = MoveSpeed;

        StateMachine = new StateMachine<PlayerBase>(this);
        StateMachine.ChangeState(IdleState.Instance);
          [SerializeField]
    private bool useOldSystem;
    if (useOldSystem == false)
        {
            StateMachine.Update();
        }
        private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "SideBoundry")
            StateMachine.ChangeState(FallState.Instance);

    }
    public void SetPowerItem(GameObject go) { TempPowerItem = go; }

    public void SetPowerUp(Powerups.Powers power) { CurrentPowerup = power; }

    public void SetSpeed(float s) { MoveSpeed = s; }

    public void SetAxis(bool a) { AxisInUse = a; }

    public float MoveSpeed
    {
        get { return _moveSpeed; }
        private set { _moveSpeed = value; }
    }

    public float DashSpeed
    {
        get { return _dashSpeed; }
        private set { _dashSpeed = value; }
    }

    public float Range
    {

        get { return _range; }
        private set { _range = value; }
    }
    public Transform GunMuzzle
    {

        get { return _gunMuzzle; }
        private set { _gunMuzzle = value; }
    }
    public StateMachine<PlayerBase> StateMachine { get; private set; }
    public GameObject TempPowerItem
    {
        get { return _tempPowerItem; }
        private set { _tempPowerItem = value; }
    }
    public CharacterController Chc { get; private set; }

    public ObjectPooling OPool { get; private set; }

    public Powerups.Powers CurrentPowerup { get; private set; }

    public Rigidbody Rigid { get; private set; }

    public bool AxisInUse { get; private set; }

    public float OriginalSpeed { get; private set; }
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _dashSpeed = 10f;
    [SerializeField]
    private float _range = 5f;
    [SerializeField]
    private float _gravity = 0.2f;
    [SerializeField]
    private Transform _gunMuzzle;
    [SerializeField]
    private GameObject _tempPowerItem;
    private Player player;
   // private PlayerController characterControl;
   // private Shooting shooting;
 */
#endregion
