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
    private DazeState daze;
    private Shooting shooting;
    private StunBehavior stunB;

    [SerializeField]
    private GameObject[] weapons;
    [SerializeField]
    private GameObject[] skins;
    [SerializeField]
    private Transform[] weaponOffsetPos;
    [Header("Offset")]
    [SerializeField]
    private Transform gunHolder;
    private Vector3 currentPos;
    private Vector3 previousPos;

    private bool godModeKnockback;
    private float godPowerTimer;
    private float godPowerLimit = 10;
    private int animationIDSet;

    private void OnEnable()
    {
        ResetGodMode();
        if (player != null)
        {
            player.Speed = player.DefaultSpeed;
        }
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
        stunB = GetComponent<StunBehavior>();
        daze = this.gameObject.GetComponent<DazeState>();
        bombPool = GameObject.Find("GameManager").GetComponent<ObjectPooling>();
        shooting = this.gameObject.GetComponent<Shooting>();
        drawColor = GameObject.Find("GameManager").GetComponent<DrawColor>();
        pCon = GetComponent<PlayerController>();
        //Reseting Weapons and Characteristics
        //ResetGodMode();
        ResetWeapon();
        DefaultWeaponSet();
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

    private void OffsetFixWeapon()
    {
        gunHolder.transform.position = weaponOffsetPos[player.skinId].transform.position;
    }

    private void checkInput()
    {
        float moveInputH = Input.GetAxisRaw("Horizontal" + player.playerNum);
        float moveInputV = Input.GetAxisRaw("Vertical" + player.playerNum);
        float input = moveInputH + moveInputV;
        if (this.gameObject.GetComponent<PlayerController>().IsDashing == true)
        {
            animationIDSet = 3;
        }
        else if (daze.Stunned == true)
        {
            animationIDSet = 2;
        }
        else
        {
            if (input == 0)
            {
                animationIDSet = 0;
            }
            else
            {
                animationIDSet = 1;
            }
        }
    }

    
    private void AnimationManagement()
    {
        switch (animationIDSet)
        {
            case 0:               
                animator.SetInteger("AnimController", 0);
                break;
            case 1:
                animator.SetInteger("AnimController", 1);
                break;
            case 2:
                animator.SetInteger("AnimController", 2);
                break;
            case 3:
                animator.SetInteger("AnimController", 3);
                break;
        }

    }

    public void DefaultWeaponSet()
    {
        gunHolder.transform.position = weaponOffsetPos[0].transform.position;
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
        currentPowerUp = powerGained;
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
            gunHolder.transform.position = weaponOffsetPos[player.skinId].transform.position;
            weapons[currentPowerUp.weaponID].SetActive(true);
        }
        else if (currentPowerUp.powerUpPower == Powerup.powerUps.godpowerup)
        {

            pCon.Splat(25);
            //StartTimer();

        }
        currentPowerUp = null;
        //ManageGame.instance.PowerIconUpdate(player.playerNum, 0);
    }

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
                ManageGame.instance.PlayerObjects[i].GetComponent<DazeState>().StartCoroutine(ManageGame.instance.PlayerObjects[i].GetComponent<DazeState>().Stun(ManageGame.instance.PlayerObjects[i].GetComponent<PlayerBase>().Player));
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

    /*private void StartTimer()
    {
        godPowerTimer += Time.deltaTime;
        if (godPowerTimer >= godPowerLimit)
        {
            ResetGodMode();
            RemoveGodPower();
        }
    }*/

    public void ResetGodMode()
    {
        godPowerTimer = 0;
    }
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
