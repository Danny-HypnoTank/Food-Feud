using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{

    public int[] Scores { get; private set; }
    public float[] Percentages { get; private set; }
    private int[] bonusPoints;
    private float totalScore;
    private int playerCount;
    private List<GameObject> gridObjects;

    [SerializeField]
    private Text scoreText;

    [SerializeField] private int _timeToCheck;
    public int TimeToCheck { get { return _timeToCheck; } }

    public void Initialisation(int count)
    {

        playerCount = count+1;

        Scores = new int[playerCount];
        Percentages = new float[playerCount];
        bonusPoints = new int[count];
        for (int i = 0; i < playerCount; i++)
        {

            Scores[i] = 0;
            Percentages[i] = 0;
            if(i < count)
            {

                bonusPoints[i] = 0;

            }

        }

    }

    public void Reset()
    {

        foreach(GameObject grid in gridObjects)
        {

            ScoreSquare square = grid.GetComponent<ScoreSquare>();

            square.SetValue(-1);

        }

        for(int i = 0; i < playerCount; i++)
        {

            Scores[i] = 0;
            Percentages[i] = 0;

        }

    }

    public void PopulateGridList()
    {

        gridObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("ScoreGrid"));

    }

    public void UnloadGridList()
    {

        gridObjects.Clear();

    }

    private void CalcScores()
    {

        for (int i = 0; i < playerCount; i++)
        {

            Scores[i] = 0;

            foreach (GameObject grid in gridObjects)
            {

                ScoreSquare square = grid.GetComponent<ScoreSquare>();

                if (i > 0)
                {
                    if (square.Value == ManageGame.instance.Players[i-1].skinId)
                        Scores[i]++;
                }
                else
                    if (square.Value == i-1)
                        Scores[i]++;

            }
        }

        totalScore = 0;

        foreach (float s in Scores)
        {

            totalScore += s;

        }

    }

    public void CalcPercentages()
    {

        CalcScores();

        for(int i = 0; i < playerCount; i++)
        {

            if (Scores[i] > 0)
            {
                float percentage = Scores[i] / totalScore;
                //percentage /= 100;
                Percentages[i] = percentage;
            }
            else
                Percentages[i] = 0;
        }

    }

    public void UpdateUI()
    {
        CalcPercentages();

        string scoreString = string.Empty;

        for (int i = 0; i < playerCount; i++)
        {

            if (i == 0)
                scoreString += $"{Percentages[i]:P2}";
            else
                scoreString += $"\t\t\t{Percentages[i]:P2}";

        }

        scoreText.text = scoreString;

    }
    public void CalculateFinalScore()
    {

        for (int i = 1; i < playerCount; i++)
        {
            Scores[i] += bonusPoints[i - 1];
        }

        foreach (float s in Scores)
        {

            totalScore += s;

        }

        for (int i = 1; i < playerCount; i++)
        {
            if (Scores[i] > 0)
            {
                float percentage = Scores[i] / totalScore;
                //percentage *= 100;
                Percentages[i] = percentage;
                ManageGame.instance.Players[i-1].scorePercentage = percentage;
            }
            else
                Percentages[i] = 0;
        }

    }

    public void AddBonusPoints(int playerID, int value)
    {

        bonusPoints[playerID] += value;

    }

}
