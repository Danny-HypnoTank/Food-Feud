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
    private Renderer faceRenderer;

    [SerializeField]
    private Texture[] listOfExpressions; //order of sprites normal/worried/happy/sad/angry

    private void OnEnable()
    {
        faceRenderer.material.mainTexture = listOfExpressions[0];
    }

    public void SetExpression(int passedExpression)
    {
        faceRenderer.material.mainTexture = listOfExpressions[passedExpression];
    }

}

