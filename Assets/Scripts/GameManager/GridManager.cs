using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _timeToCheck;
    public int TimeToCheck { get { return _timeToCheck; } }

    [Header("Bar Graphics")]
    [SerializeField]
    private RectTransform[] fillBars;

    private float totalScore;
    private int playerCount;
    private bool updating;
    private float originalBarWidth;
    private List<ScoreSquare> gridObjects;

    public int[] Scores { get; private set; }
    public float[] Percentages { get; private set; }

    [SerializeField]
    private Text scoreText;

    public void Initialisation(int count)
    {
        gridObjects = new List<ScoreSquare>();
        playerCount = count + 1;
        originalBarWidth = fillBars[0].rect.width;

        Scores = new int[playerCount];
        Percentages = new float[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            Scores[i] = 0;
            Percentages[i] = 0;
        }

        updating = false;
    }

    public void Reset()
    {
        foreach (ScoreSquare square in gridObjects)
        {
            square.SetValue(-1);
        }

        for (int i = 0; i < playerCount; i++)
        {
            Scores[i] = 0;
            Percentages[i] = 0;
        }
    }

    public void PopulateGridList()
    {
        GameObject[] tempGrid = GameObject.FindGameObjectsWithTag("ScoreGrid");
        foreach (GameObject go in tempGrid)
        {
            gridObjects.Add(go.GetComponent<ScoreSquare>());
        }
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

            foreach (ScoreSquare square in gridObjects)
            {
                if (i > 0)
                {
                    if (square.Value == ManageGame.instance.Players[i - 1].skinId)
                        Scores[i]++;
                }
                else
                    if (square.Value == i - 1)
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

        for (int i = 0; i < playerCount; i++)
        {
            if (Scores[i] > 0)
            {
                float percentage = Scores[i] / totalScore;
                Percentages[i] = percentage;
            }
            else
                Percentages[i] = 0;
        }
    }

    public void UpdateUI()
    {
        if (!updating)
        {
            StartCoroutine(updateCooldown());
            CalcPercentages();

            for (int i = 0; i < playerCount; i++)
            {
                float newWidth = originalBarWidth + 15;
                newWidth *= Percentages[i];

                fillBars[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

                if (i > 0)
                {
                    RectTransform prevBar = fillBars[i - 1];
                    float newX = prevBar.localPosition.x + prevBar.rect.width;
                    newX -= 15;
                    Vector2 newPosition = new Vector2(newX, prevBar.localPosition.y);
                    fillBars[i].localPosition = newPosition;
                }
            }

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
    }
    public void CalculateFinalScore()
    {
        foreach (float s in Scores)
        {
            totalScore += s;
        }

        for (int i = 1; i < playerCount; i++)
        {
            if (Scores[i] > 0)
            {
                float percentage = Scores[i] / totalScore;
                Percentages[i] = percentage;
                ManageGame.instance.Players[i - 1].scorePercentage = percentage;
            }
            else
                Percentages[i] = 0;
        }
    }

    private IEnumerator updateCooldown()
    {
        updating = true;
        yield return new WaitForSeconds(1);
        updating = false;
    }

}
