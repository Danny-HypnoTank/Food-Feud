/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 21/11/2019
 * Last Modified: 21/11/2019
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
    [SerializeField]
    private Transform loadingPage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        loadingPage.gameObject.SetActive(false);
        mainMenuScript.gameObject.SetActive(true);
        optionsScript.gameObject.SetActive(false);
        characterSelectScript.gameObject.SetActive(false);
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
        Debug.Log("Triggered");
        mainMenuScript.gameObject.SetActive(true);
        optionsScript.gameObject.SetActive(false);
    }

    public void CharacterSelectionToMainMenu()
    {

        mainMenuScript.gameObject.SetActive(true);
        characterSelectScript.gameObject.SetActive(false);
    }

}
