/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 15/10/2019
 */
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "ScriptableObjects/Player", order = 0)]
public class Player : ScriptableObject
{
    public int playerNum;       //Preset value for each player
    public bool isActivated;       //checks if player is going to participate in the game in character selection panel
    public bool isLocked;       //used for character selection to see if player locked in or not
    public int playerScore;     //Used for storing score for each individual player
    public float scorePercentage; //Percentage of total score
    public int skinId;          //Temporary will be replaced later based on how we choose to handle skins
    public string[] namesOfSkins; //Temporary variable for testing
    public Color[] SkinColours;
    [Header("In Game Values")]
    private float defaultSpeed = 5;
    public bool hasWon = false;
    private float speed = 5;

    public float Speed { get => speed; set => speed = value; }
    public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = value; }
}
