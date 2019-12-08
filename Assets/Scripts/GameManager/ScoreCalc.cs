using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Class for calculating the area of the level covered by each player
/// </summary>
public class ScoreCalc : MonoBehaviour
{

    #region Public Methods

    private void Awake()
    {

        //Singleton instantiation
        if (Instance != null)
            Instance = this;

    }

    private void Start()
    {

        //Make Array size the same as terrain list
        int size = ManageGame.instance.drawColor._Terrain.Count;
        terrain = new float[size];

        //Iterate through terrain Array and set values to the terrain list values ignoring element 0 (floor)
        for (int i = 0; i < terrain.Length; i++)
        {

            terrain[i] = ManageGame.instance.drawColor._Terrain[i].GetComponent<PaintSizeMultiplier>().multiplier;

        }

        //Initialise arrays
        Scores = new int[4];
        Circles = new Circle[buffersize];

    }

    /// <summary>
    /// Method for displaying the player's scores
    /// </summary>
    private void DisplayScores()
    {

        //Loop through all elements of ScoreText and set the text accordingly
        for (int i = 0; i < 4; i++)
        {

            ScoreText[i].text = $"Player {i} Score: {Scores[i]}";

        }

    }

    

    #endregion


    #region To revisit next Trimester

    #region Structs

    /// <summary>
    /// Struct to define a circle via the colour, centre point and size
    /// </summary>
    public struct Circle
    {

        public UInt16 PlayerColour { get; set; } //The ID of the colour
        public Vector2 CentrePoint { get; set; } //The location of the centre point
        public float Size { get; set; } //The size of the circle
        public int TerrainID { get; set; } //ID of the terrain
        public float Radius { get; set; } //Radius of the circle
        public float ExposedArea { get; set; } //Total area exposed

    }

    #endregion

    #region Fields

    private int nextIndex = 0; //The next index of the Cicles array
    private float[] terrain; //Lookup table for the terrain
    private readonly UInt16 buffersize = UInt16.MaxValue; //Buffer size for the terrain array

    #endregion

    #region Properties

    /// <summary>
    /// Object to store the instance of the class
    /// </summary>
    public static ScoreCalc Instance { get; private set; }

    /// <summary>
    /// Array of circles
    /// </summary>
    public Circle[] Circles { get; private set; }

    /// <summary>
    /// Array for the player scores
    /// </summary>
    public int[] Scores { get; private set; }

    /// <summary>
    /// The Text UI objects to display the player's scores
    /// </summary>
    [SerializeField]
    public Text[] ScoreText { get; private set; }

    #endregion

    #region Private Methods

    /// <summary>
    /// Method for adding circles to the array
    /// </summary>
    /// <param name="hit">The raycast hit info from the raycast that paints</param>
    /// <param name="playerColour">The ID of the colour</param>
    /// <param name="num">The index of the terrain hit</param>
    public void CircleLogic(RaycastHit hit, UInt16 playerColour, int num)
    {

        //Add the circle to the array
        Circles[nextIndex] = new Circle { CentrePoint = hit.point, PlayerColour = playerColour, TerrainID = num };

        //If the terrain isn't the ground, get the size from the lookup table, else size is 1
        if (num > 0)
            Circles[nextIndex].Size = terrain[num];
        else
            Circles[nextIndex].Size = 1;

        //If we hit the max size of the Circles array loop back to the start
        if (nextIndex <= buffersize)
            nextIndex++;
        else
            nextIndex = 0;

    }

    /// <summary>
    /// Method for calculating the radius of a circle
    /// </summary>
    /// <param name="c">The circle to calculate the area of</param>
    /// <returns>The radius of the given circle</returns>
    private float CalculateCircleRadius(Circle c)
    {

        float result;

        //Calculate the radius of the circle with the formula r = Sqrt(Size/π)
        float radSq = c.Size / (float)Math.PI;
        result = (float)Math.Sqrt(radSq);

        return result;

    }

    /// <summary>
    /// Method to calculate the distance between two circles
    /// </summary>
    /// <param name="one">The first circle to compare</param>
    /// <param name="two">The second circle to compare</param>
    private float CalculateCircleDistance(Circle one, Circle two)
    {

        float result;

        one.Radius = CalculateCircleRadius(one); //The bottom circle
        two.Radius = CalculateCircleRadius(two); //The top circle

        result = Vector2.Distance(one.CentrePoint, two.CentrePoint);

        return result;

    }


    private float CalculatOverlap(float distance, float rad1, float rad2)
    {

        float overlapArea;

        overlapArea = (Mathf.Pow(-distance, 2)) * (-distance + (2 * rad1)) * (distance + (2 * rad1));

        overlapArea = Mathf.Sqrt(overlapArea) / distance;

        return overlapArea;

    }

    private void CalculateExposedArea(Circle one, Circle two)
    {

        float distance = CalculateCircleDistance(one, two);

        float overlapArea = CalculatOverlap(distance, one.Radius, two.Radius);

    }

    #endregion

    #endregion

}