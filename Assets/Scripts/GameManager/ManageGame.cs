/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 15/10/2019
 * Modified By: Antoni Gudejko, Dominik Waldowski
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour
{
    private DeliveryPointsManager deliveryManager;
    [SerializeField]
    private Material[] coloursForPlatforms;
    private Loading loading;
    public delegate void GameWin();
    public static event GameWin OnGameWin;
    public static ManageGame instance;                            //instance of game manager
    [SerializeField]
    private Transform[] playerSpawnPositions;                   //stores all spawn positions for players
    [SerializeField]
    private Player[] players;                                   //stores all player data
    [Header("Player model")]
    [SerializeField]
    private GameObject emptyPlayer;
    [Header("Clock")]
    [SerializeField]
    private TextMesh timeRemaining;                                 //text that displays remaining time
    private float reverseTime = 0;                             //actual timer 
    [SerializeField]
    private List<GameObject> mapEdgesForGodPower = new List<GameObject>();
    private bool isTimingDown;
    [SerializeField]
    private GameObject godPowerUp;
    private List<GameObject> playerObjects = new List<GameObject>();
    private LevelManager layoutManager;
    public GridManager gridManager { get; private set; }
    public DrawColor drawColor { get; private set; }
    [SerializeField]
    private ObjectPooling objectPooling;
    [SerializeField]
    private Transform clockHand, pointA, pointB;
    [SerializeField]
    private float journeyLength;
    public bool IsTimingDown { get => isTimingDown; set => isTimingDown = value; }
    public Transform[] PlayerSpawnPositions { get => playerSpawnPositions; set => playerSpawnPositions = value; }
    public Player[] Players { get => players; set => players = value; }
    public List<GameObject> PlayerObjects { get => playerObjects; set => playerObjects = value; }
    public List<GameObject> MapEdgesForGodPower { get => mapEdgesForGodPower; set => mapEdgesForGodPower = value; }
    public GameObject GodPowerUp { get => godPowerUp; set => godPowerUp = value; }
    private float timeLimit = 60;

    //Creates instance of game manager
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }

        clockHand.eulerAngles = v3Rot;
    }
    //handles display and counting of round timer
    [SerializeField]
    Vector3 v3Rot;
    [SerializeField]
    Vector3 v3Dest;

    float speed = 3.8f; //2.5f

    private void Update()
    {
        if (isTimingDown == true)
        {
            v3Rot = Vector3.MoveTowards(v3Rot, v3Dest, speed * Time.deltaTime);
            v3Rot.y = 90;
            v3Rot.z = 0;
            //Debug.Log(reverseTime);
            reverseTime += Time.deltaTime;
            clockHand.transform.eulerAngles = v3Rot;
          //  var lookDir = pointB.position - clockHand.position;
           // lookDir.y = 90; 
          //  lookDir.z = 0;
            //clockHand.rotation = Quaternion.LookRotation(lookDir);
           // Quaternion rot = Quaternion.LookRotation(lookDir);
           // clockHand.rotation = Quaternion.Slerp(clockHand.rotation, rot, 1 * Time.deltaTime);
            //  string minutes = ((int)reverseTime / 60).ToString("00");
            //string seconds = ((int)reverseTime % 60).ToString("00");
            // timeRemaining.text = string.Format("{00:00}:{01:00}", minutes, seconds);
            if (reverseTime >= timeLimit)
            {
                reverseTime = timeLimit;
                if(OnGameWin != null)
                    OnGameWin();
                objectPooling.DisableAll();
                // loading.SetID(2);
                // loading.InitializeLoading();
                gridManager.CalculateFinalScore();
                SceneManager.LoadScene("EndRoundScene");
            }
            if (reverseTime % gridManager.TimeToCheck < 1) //Modulus operator to check if the value of reverseTime goes into TimeToCheck with a remainder that is less than 1, i.e. 60.23416 % 30 = 0.23416, 70.81674 % 30 = 10.81674 etc. -James
                gridManager.UpdateUI();
        }
    }


    private void Start()
    {
        loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
        layoutManager = GetComponent<LevelManager>();
        gridManager = GetComponent<GridManager>();
        drawColor = GetComponent<DrawColor>();
        layoutManager.LayoutGeneration();
        deliveryManager = GameObject.Find("DeliveryPointManager").GetComponent<DeliveryPointsManager>();
        for (int i = 0; i < layoutManager.SpawnPoints.Length; i++)
        {
            PlayerSpawnPositions[i] = layoutManager.SpawnPoints[i].transform;
        }
        drawColor._Terrain = layoutManager.PaintableObjects;
        godPowerUp.SetActive(false);
        //grabs time from main menu scene and checks if its in bounds if its not it sets it to 60 (temporary measure for when we test it straight from game scene)
        // reverseTime = PlayerPrefs.GetFloat("RoundDuration");
        //  if(reverseTime < 29 || reverseTime > 91)
        //  {
        //    reverseTime = 60;
        //  }
        reverseTime = 0;
        isTimingDown = false;
        // timeRemaining.gameObject.SetActive(false);
        PlacePlayers();

        SetUpSecondarySpawners();

        gridManager.Initialisation(PlayerObjects.Count);
        gridManager.PopulateGridList();
        gridManager.UpdateUI();
    }

    private void SetUpSecondarySpawners()
    {
        SecondaryObjectiveSpawner spawner = GameObject.Find("SecondaryObjSpawner").GetComponent<SecondaryObjectiveSpawner>();
        if (deliveryManager == null)
        {
            deliveryManager = FindObjectOfType<DeliveryPointsManager>();

        }
        deliveryManager.SetDeliveryPoints();
        for (int i = 0; i < PlayerObjects.Count; i++)
        {
            PlayerObjects[i].GetComponent<SecondaryObjCollector>().SetSpawner(spawner);
            deliveryManager.DeliveryPoints[i].gameObject.SetActive(true);
            deliveryManager.DeliveryPoints[i].GetComponent<DeliveryPoint>().Owner = playerObjects[i];
            deliveryManager.DeliveryPoints[i].GetComponent<Renderer>().material = coloursForPlatforms[playerObjects[i].GetComponent<PlayerBase>().Player.skinId];
        }
    }

    private void PlacePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isActivated == true)
            {
                if (players[i].isLocked == true)
                {
                    GameObject newPlayer = Instantiate(emptyPlayer) as GameObject;
                    newPlayer.name = "Player: " + (i + 1);
                    newPlayer.SetActive(false);
                    newPlayer.transform.position = playerSpawnPositions[i].transform.position;
                    newPlayer.transform.rotation = PlayerSpawnPositions[i].transform.rotation;
                    newPlayer.SetActive(true);
                    newPlayer.GetComponent<PlayerBase>().SetSkin(players[i].skinId);
                    players[i].playerScore = 0;
                    newPlayer.GetComponent<PlayerBase>().SetExpressionManager(players[i].skinId);
                    newPlayer.GetComponent<PlayerController>().Player = players[i];
                    PlayerBase playerBase = newPlayer.GetComponent<PlayerBase>();
                    playerBase.Player = players[i];
                    players[i].Speed = players[i].DefaultSpeed;
                    playerObjects.Add(newPlayer);
                }
            }
        }
    }

    //updates scores of the player
    /*public void UpdateScore(int playerNum, int scoreReceived)
    {
        players[playerNum - 1].playerScore += scoreReceived;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isActivated == true)
            {
                if (players[i].isLocked == true)
                {
                    playerBoards[i].Find("RightSide").Find("ScoreTxt").GetComponent<Text>().text = "Score: " + players[i].playerScore.ToString("0");
                }
            }
        }
    }*/

    //updates respawn timers of the player
    /*public void UpdateRespawnTimer(int playerNum, int respawnTime)
    {
        if(respawnTime > 0)
        {
            playerBoards[playerNum - 1].Find("Foreground").Find("RespawnTime").gameObject.SetActive(true);
        }
        playerBoards[playerNum - 1].Find("Foreground").Find("RespawnTime").GetComponent<Text>().text = "Respawn: " + respawnTime;
        if(respawnTime <= 0)
        {
            playerBoards[playerNum - 1].Find("Foreground").Find("RespawnTime").gameObject.SetActive(false);
        }
    }*/

    //Activates round timer
    public void StartTimer()
    {
      //  timeRemaining.gameObject.SetActive(true);
        isTimingDown = true;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameTheme();
        }
    }
}
