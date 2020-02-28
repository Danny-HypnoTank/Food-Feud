/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 10/12/2019
 * Modified by: Alex Watson
 * Last Modified: 10/02/2020
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumController : MonoBehaviour
{
    private Vector3 minScale;
    private Vector3 maxScale;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Transform playerHolder;
    [SerializeField]
    private Transform[] models;
    [SerializeField]
    private Animator animator;
    private float total;
    [SerializeField]
    private Transform podium;
    [SerializeField]
    private MeshRenderer podiumMesh;
    [SerializeField]
    private Material[] podiumAlternates;
    [SerializeField]
    private float speed = 2.0f;
    private float duration = 5.0f;

    public void AddPlayer(Player newPlayer)

    {
        player = newPlayer;

        for (int i = 0; i < models.Length; i++)
        {
            models[i].gameObject.SetActive(false);
        }

            models[player.skinId].gameObject.SetActive(true);
        podiumMesh.material = podiumAlternates[player.skinId];
            animator = models[player.skinId].GetComponent<Animator>();
            //models[player.skinId].GetComponent<ExpressionManager>().SetExpression(0);

            animator.enabled = true;
            animator.SetInteger("Pos", 0);

    }
    public void SetTotal(float _total)
    {
        total = _total;
        if (this.gameObject.activeInHierarchy == true)
        {
            PercentageCalc();
            Scale();
        }
    }


    //Chethan and PK code modified slightly by Dominik Waldowski
    private void PercentageCalc()
    {
            player.scorePercentage = (player.playerScore / total) * 100;
    }

    private IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time, GameObject podium)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            podium.transform.Translate(Vector3.up * (rate / 2) * Time.deltaTime);
            podium.transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        if(player.hasWon == true)
        {
            animator.SetInteger("Pos", 1);
            models[player.skinId].GetComponent<ExpressionManager>().SetExpression(1);
        }
        else if(player.hasWon == false)
        {
            animator.SetInteger("Pos", 2);
            models[player.skinId].GetComponent<ExpressionManager>().SetExpression(2);
        }
    }

    private void Scale()
    {
        minScale = transform.localScale;
        maxScale = new Vector3(1, (float)player.scorePercentage /100 , 1);
        minScale = new Vector3(1, 0, 1);
        StartCoroutine(RepeatLerp(minScale, maxScale, duration, podium.gameObject));
    }


}
