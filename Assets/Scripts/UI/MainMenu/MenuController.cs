/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 21/11/2019
 * Modified by: Alex Watson
 * Last Modified: 08/03/2020
 */
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public static MenuController instance;
    [SerializeField]
    private Transform mainMenuScript;
    [SerializeField]
    private Transform optionsScript;
    [SerializeField]
    private Transform characterSelectScript;
    /*[SerializeField]
    private Transform loadingPage;*/
    [SerializeField]
    private Transform levelSelectScript;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //loadingPage.gameObject.SetActive(false);
        mainMenuScript.gameObject.SetActive(true);
        optionsScript.gameObject.SetActive(false);
        characterSelectScript.gameObject.SetActive(false);
        levelSelectScript.gameObject.SetActive(false);
    }

    public void MainMenuToOptionsTransition()
    {
        mainMenuScript.gameObject.SetActive(false);
        optionsScript.gameObject.SetActive(true);
    }
    
    public void MainMenuToCharacterSelect()
    {
        mainMenuScript.gameObject.SetActive(false);
        characterSelectScript.gameObject.SetActive(true);
    }
    public void OptionsMenuToMainMenu()
    {
        mainMenuScript.gameObject.SetActive(true);
        optionsScript.gameObject.SetActive(false);
    }

    public void CharacterSelectionToMainMenu()
    {

        mainMenuScript.gameObject.SetActive(true);
        characterSelectScript.gameObject.SetActive(false);
    }

    public void CharacterSelectionToLevelSelect()
    {
        levelSelectScript.gameObject.SetActive(true);
        characterSelectScript.gameObject.SetActive(false);
    }

    public void LevelSelectToCharacterSelection()
    {
        levelSelectScript.gameObject.SetActive(false);
        characterSelectScript.gameObject.SetActive(true);
    }

}
