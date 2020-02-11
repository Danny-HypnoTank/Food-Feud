/* 
 * Created by:
 * Name: Chethan Prasad
 * Sid: 1831604
 * Date Created: 01/10/2019
 * Last Modified: 
 * Modified By: 
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    public void MainMenu()
    {
        //if (Input.GetAxis("Horizontal") < -0.3f)
        //{
        SceneManager.LoadScene("NewMainMenu", LoadSceneMode.Single);
        //}
    }

    public void RematchButton()
    {
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
