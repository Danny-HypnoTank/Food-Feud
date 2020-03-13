using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int _timeToCheck; //How frequently we want things to be ran

    [Header("Bar Graphics")]
    [SerializeField]
    private RectTransform[] fillBars; //The bars for the score bar

    private float totalScore; //Contains the combined total of all player's scores
    private int playerCount; //The amount of players (+1)
    private bool updating; //Is something being updated
    private float originalBarWidth; //The value of the original width of the score bar
    private List<ScoreSquare> gridObjects; //List to store the grid objects

    public int[] Scores { get; private set; } //Array to store the scores for the players + the unpainted areas
    public float[] Percentages { get; private set; } //Array to store the percentages for the players + unpainted areas
    public int TimeToCheck { get { return _timeToCheck; } } //Property to grab the value of _timeToCheck

    [SerializeField]
    private Text scoreText;

    //Initialisation method
    public void Initialisation(int count)
    {
        //Instantiate grid
        gridObjects = new List<ScoreSquare>();
        //Set default values
        playerCount = count + 1;
        originalBarWidth = fillBars[0].rect.width;

        //Initialise arrays
        Scores = new int[playerCount];
        Percentages = new float[playerCount];
        //Set default values in the arrays
        for (int i = 0; i < playerCount; i++)
        {
            Scores[i] = 0;
            Percentages[i] = 0;
        }

        //Updating defaults to false
        updating = false;

        for (int i = 1; i < playerCount; i++)
        {
            Image currentImage = fillBars[i].GetComponent<Image>();
            int id = ManageGame.instance.Players[i - 1].skinId;
            currentImage.color = ManageGame.instance.Players[i - 1].SkinColours[id];
            Color colour = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 1);
            currentImage.color = colour;
        }
    }

    //Reset method
    public void Reset()
    {
        //Reset the values of the score squares
        for (int i = 0; i < gridObjects.Count; i++)
        {
            gridObjects[i].SetValue(-1);
        }

        //Reset the scores and percentages
        for (int i = 0; i < playerCount; i++)
        {
            Scores[i] = 0;
            Percentages[i] = 0;
        }
    }

    //Method to populate the list
    public void PopulateGridList()
    {
        //Temporary array of score grid objects
        GameObject[] tempGrid = GameObject.FindGameObjectsWithTag("ScoreGrid");
        //Populate list with the score square component from each grid object
        for (int i = 0; i < tempGrid.Length; i++)
        {
            gridObjects.Add(tempGrid[i].GetComponent<ScoreSquare>());
        }
    }

    //Method to unload the grid
    public void UnloadGridList()
    {
        //Set the list to null
        gridObjects = null;
    }

    //Method for calculating scores
    private void CalcScores()
    {
        for (int i = 0; i < playerCount; i++)
        {
            //Reset score
            Scores[i] = 0;

            //Loop through the list of score squares
            for (int j = 0; j < gridObjects.Count; j++)
            {
                //If the index is > 0
                if (i > 0)
                {
                    //If the value of the score square is the player's skinId - 1 increment their score
                    if (gridObjects[j].Value == ManageGame.instance.Players[i - 1].skinId)
                        Scores[i]++;
                }
                else
                    if (gridObjects[j].Value == i - 1)
                    Scores[i]++;
            }
        }

        //Reset the total score
        totalScore = 0;

        //Add each score to total score
        for (int i = 0; i < Scores.Length; i++)
        {
            totalScore += Scores[i];
        }
    }

    //Method for calculating percentages
    public void CalcPercentages()
    {
        //Calculate the scores
        CalcScores();

        for (int i = 0; i < playerCount; i++)
        {
            //If the score is greater than 0 calculate the percentage, else set the percentage to 0
            if (Scores[i] > 0)
            {
                float percentage = Scores[i] / totalScore;
                Percentages[i] = percentage;
            }
            else
                Percentages[i] = 0;
        }
    }

    //Method for updating the UI
    public void UpdateUI()
    {
        //Check if the UI is updating
        if (!updating)
        {
            //Start the cooldown coroutine
            StartCoroutine(UpdateCooldown());
            //Calculate the percentages
            CalcPercentages();

            for (int i = 0; i < playerCount; i++)
            {
                //Initialise a value for the new width
                float newWidth = originalBarWidth;
                newWidth *= Percentages[i];
                float extra = 12 * (playerCount);
                extra *= Percentages[i];
                if (newWidth < originalBarWidth && newWidth > extra)
                    newWidth += extra;

                //Set the width of the bar
                fillBars[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

                //If the bar isn't the unpainted percentage
                if (i > 0)
                {
                    //Get a reference to the previous bar in the array
                    RectTransform prevBar = fillBars[i - 1];
                    //Calculate the new X value for the position
                    float newX = prevBar.localPosition.x + prevBar.rect.width;
                    newX -= 15;
                    //Create a new Vector2 for the new position
                    Vector2 newPosition = new Vector2(newX, prevBar.localPosition.y);
                    //Set the position of the bar
                    fillBars[i].localPosition = newPosition;
                }
            }

            #region maybe keep idk
            //Instantiate string builder
            StringBuilder scoreString = new StringBuilder();

            //Loop through the percentage array and append to the string builder
            for (int i = 0; i < playerCount; i++)
            {
                if (i == 0)
                    scoreString.Append($"{Percentages[i]:P2}");
                else
                    scoreString.Append($"\t\t\t{Percentages[i]:P2}");
            }

            //Set the string value of the text
            scoreText.text = scoreString.ToString();
            #endregion
        }
    }

    //Method for calculating the final scores
    public void CalculateFinalScore()
    {
        //Calculate the percentages
        CalcPercentages();

        //Set the player's percentages to the calculated percentages
        for (int i = 1; i < playerCount; i++)
        {
            ManageGame.instance.Players[i - 1].scorePercentage = Percentages[i];
        }
    }

    //Coroutine so methods can be made to only run once per second
    private IEnumerator UpdateCooldown()
    {
        updating = true;
        yield return new WaitForSeconds(1);
        updating = false;
    }

}