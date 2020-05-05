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

    [Header("Objects")]
    [SerializeField]
    private GameObject multiplierArea;

    [Header("Debug")]
    [SerializeField]
    private bool usePercentages;

    private float totalScore; //Contains the combined total of all player's scores
    private int playerCount; //The amount of players (+1)
    private bool updating; //Is something being updated
    private float originalBarWidth; //The value of the original width of the score bar
    private List<ScoreSquare> gridObjects; //List to store the grid objects
    private StringBuilder scoreString;
    [SerializeField]
    private int[] scores;
    
    public int[] Scores { get { return scores; } private set { scores = value; } } //Array to store the scores for the players + the unpainted areas
    public float[] Percentages { get; private set; } //Array to store the percentages for the players + unpainted areas
    public int TimeToCheck { get { return _timeToCheck; } } //Property to grab the value of _timeToCheck

    [SerializeField]
    private Text scoreText;

    //Initialisation method
    public void Initialisation(int count)
    {
        //Set the scoreText to active or inactive based on if we're using the percentages debug
        scoreText.gameObject.SetActive(usePercentages);

        //Instantiate grid
        gridObjects = new List<ScoreSquare>();
        //Instantiate scorestring
        scoreString = new StringBuilder();
        //Set default values
        playerCount = count;
        originalBarWidth = fillBars[0].rect.width;

        //Initialise arrays
        Scores = new int[playerCount];
        Percentages = new float[playerCount];
        //Set default values in the arrays
        for (int i = 0; i < playerCount; i++)
        {
            Scores[i] = 1;
            Percentages[i] = 0f;
        }

        try
        {
            InitialiseBars();
        }
        catch
        {
            Debug.LogWarning("Failed To initialise bars line 65 Grid Manager Script");
        }

        //Updating defaults to false
        updating = false;

        UpdateUI();
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
            Scores[i] = 1;
            Percentages[i] = 0f;
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

    private void InitialiseBars()
    {
        //Get the Image component of the left-most bar element
        Image currentImage = fillBars[4].GetComponent<Image>();

        //Get the skinId of the player with the lowest playernum and set the bar colour to their skin colour
        int playerID = ManageGame.instance.allPlayerControllers[0].Player.skinId;
        //Set the colour to the skin colour
        currentImage.color = ManageGame.instance.allPlayerControllers[0].Player.ScoreColour[playerID];

        //Make sure the colour actually has 100% alpha and apply the colour to the bar image
        Color newColour = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 1);
        currentImage.color = newColour;

        for (int i = 0; i < playerCount + 1; i++)
        {
            //Set the bar for the current player to active
            fillBars[i].gameObject.SetActive(true);

            //Get the Image component of the current bar element
            currentImage = fillBars[i].GetComponent<Image>();

            if (i < playerCount)
            {
                //Get the player's skin ID
                playerID = ManageGame.instance.allPlayerControllers[i].Player.skinId;

                //Set the colour to the skin colour
                currentImage.color = ManageGame.instance.allPlayerControllers[i].Player.ScoreColour[playerID];

                //Make sure the colour actually has 100% alpha and apply the colour to the bar image
                newColour = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 1);
                currentImage.color = newColour;
            }

        }
        
    }

    //Method for calculating scores
    private void CalcScores()
    {
        for (int i = 0; i < playerCount; i++)
        {
            //Reset score
            Scores[i] = 1;

            //Loop through the list of score squares
            for (int j = 0; j < gridObjects.Count; j++)
            {
                //If the value of the score square is the player's skinId - 1 increment their score
                if (gridObjects[j].Value == ManageGame.instance.Players[i].skinId)
                    Scores[i] +=  gridObjects[j].Multiplier;
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

    public void UpdateElements()
    {
        //Check if the UI is updating
        if (!updating)
        {
            //Start the cooldown coroutine
            StartCoroutine(UpdateCooldown());
            //Call the UI Update method
            UpdateUI();
            //Call the Multiplier Area Toggle method
            ToggleMultiplierArea();
        }
    }

    //Method for updating the UI
    private void UpdateUI()
    {
        //Calculate the percentages
        CalcPercentages();
        //Calculate and set bar widths
        CalculateBarWidth();
        //Calculate the score text
        CalculateScoreText();
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

    private void CalculateBarWidth()
    {
        for (int i = 0; i < playerCount; i++)
        {
            //Initialise a value for the new width
            float newWidth = originalBarWidth;
            newWidth *= Percentages[i];
            //Calculate the extra width to account for the extra bar image
            if (i > 0)
                newWidth += 15;

            //Set the width of the bar
            fillBars[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

            //If the bar isn't player 1
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
    }

    private void CalculateScoreText()
    {
        //Check if we're using the percentages or not
        if (usePercentages)
        {
            scoreString.Clear();

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
        }
    }

    public void ToggleMultiplierArea()
    {
        //Returns the opposite value of whether the object is enabled or not (i.e. if enabled, it returns false)
        bool isEnabled = !multiplierArea.gameObject.activeSelf;

        //Check if isEnabled is false and if it is, call the method to change the scale and location of the multiplier else, reset the multipliers
        if (isEnabled)
        {
            SetMultiplierScale();
            SetMultiplierPosition();
        }
        else
        {
            for(int i = 0; i < gridObjects.Count; i++)
            {
                if(gridObjects[i].Multiplier > 1)
                {
                    gridObjects[i].SetMultiplier(1);
                }
            }
        }

        //Set the object to inactive or active
        multiplierArea.SetActive(isEnabled);
    }

    private void SetMultiplierScale()
    {
        int newSize = Random.Range(3, 6);

        //Scale the area of the multiplier zone by 3-5 score grid elements (4x4 squares)
        Vector3 newScale = new Vector3
        {
            x = newSize,
            y = 3,
            z = newSize
        };

        multiplierArea.transform.localScale = newScale;
    }

    private void SetMultiplierPosition()
    {
        //Get a random square
        int squareIndex = Random.Range(0, gridObjects.Count);
        Vector3 newPos = gridObjects[squareIndex].transform.position;

        //Move the multiplier area to the chosen square
        Vector3 newPosition = new Vector3
        {
            x = newPos.x,
            y = -1.8f,
            z = newPos.z
        };
        
        multiplierArea.transform.position = newPosition;
    }

    //Coroutine so methods can be made to only run once per second
    private IEnumerator UpdateCooldown()
    {
        updating = true;
        yield return new WaitForSeconds(1);
        updating = false;
    }

}