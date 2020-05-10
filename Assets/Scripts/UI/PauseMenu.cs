/* 
 * Created by:
 * Name: Alexander Watson
 * Sid: 1507490
 * Date Created: 05/10/2019
 * Last Modified: 10/12/2019
 * Modified by: Dominik Waldowski
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool GameIsPaused = false;
    private GameObject pauseMenu;
    //private Loading loading;

    private void Awake()
    {
        pauseMenu = GameObject.Find("PausePanel").gameObject;
        //loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (ManageGame.instance.IsTimingDown)
        {
            if (Input.GetButtonDown("Activate"))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(10, LoadSceneMode.Single);
    }
}
