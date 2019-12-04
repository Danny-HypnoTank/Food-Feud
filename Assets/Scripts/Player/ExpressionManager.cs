using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer faceObj;

    [SerializeField]
    private Sprite[] listOfExpressions; //order of sprites normal/worried/happy/sad/angry

    private void OnEnable()
    {
        faceObj.sprite = listOfExpressions[0];
    }

    public void SetExpression(int passedExpression)
    {
        faceObj.sprite = listOfExpressions[passedExpression];
    }

}
