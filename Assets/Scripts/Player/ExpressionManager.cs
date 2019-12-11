/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 02/10/2019
 * Last Modified: 10/12/2019
 */
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
