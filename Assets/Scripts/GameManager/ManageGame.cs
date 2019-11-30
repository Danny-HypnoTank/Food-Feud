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
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ManageGame : MonoBehaviour
{
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
    private float reverseTime = 60;                             //actual timer 
    [SerializeField]
    private List<GameObject> mapEdgesForGodPower = new List<GameObject>();
    private bool isTimingDown;
    [SerializeField]
    private GameObject godPowerUp;
    private List<GameObject> playerObjects = new List<GameObject>();
    private LevelManager layoutManager;
    private DrawColor drawColor;

    public bool IsTimingDown { get => isTimingDown; set => isTimingDown = value; }
    public Transform[] PlayerSpawnPositions { get => playerSpawnPositions; set => playerSpawnPositions = value; }
    public Player[] Players { get => players; set => players = value; }
    public List<GameObject> PlayerObjects { get => playerObjects; set => playerObjects = value; }
    public List<GameObject> MapEdgesForGodPower { get => mapEdgesForGodPower; set => mapEdgesForGodPower = value; }
    public GameObject GodPowerUp { get => godPowerUp; set => godPowerUp = value; }

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
    }

    private void Start()
    {
        layoutManager = GetComponent<LevelManager>();
        drawColor = GetComponent<DrawColor>();
        layoutManager.LayoutGeneration();
        for (int i = 0; i < layoutManager.SpawnPoints.Length; i++)
        {
            PlayerSpawnPositions[i] = layoutManager.SpawnPoints[i].transform;
        }
        drawColor._Terrain = layoutManager.PaintableObjects;
        godPowerUp.SetActive(false);
        //grabs time from main menu scene and checks if its in bounds if its not it sets it to 60 (temporary measure for when we test it straight from game scene)
        reverseTime = PlayerPrefs.GetFloat("RoundDuration");
        if(reverseTime < 29 || reverseTime > 91)
        {
            reverseTime = 60;
        }
        isTimingDown = false;
        timeRemaining.gameObject.SetActive(false);
        PlacePlayers();
    }

    //Prepares players and their UI to initiate the game
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
                    
                    newPlayer.GetComponent<PlayerController>().Player = players[i];
                    PlayerBase playerBase = newPlayer.GetComponent<PlayerBase>();
                    playerBase.Player = players[i];
                    for (int t = 0; t < playerBase.Weapons.Length; t++)
                    {
                        playerBase.Weapons[t].GetComponent<Shooting>().Player = players[i];
                        playerBase.Weapons[t].GetComponent<Shooting>().PlayerBase = playerBase; 
                    }
                    players[i].Speed = players[i].DefaultSpeed;
                    playerObjects.Add(newPlayer);
                }
            }
        }
    }

    //handles display and counting of round timer
    private void Update()
    {
        if (isTimingDown == true)
        {
            reverseTime -= Time.deltaTime;
            string minutes = ((int)reverseTime / 60).ToString("00");
            string seconds = ((int)reverseTime % 60).ToString("00");
            timeRemaining.text = string.Format("{00:00}:{01:00}", minutes, seconds);
            if (reverseTime <= 0)
            {
                reverseTime = 0;
                if(OnGameWin != null)
                    OnGameWin();
                SceneManager.LoadScene("EndRoundScene");
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
        timeRemaining.gameObject.SetActive(true);
        isTimingDown = true;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameTheme();
        }
    }
}
