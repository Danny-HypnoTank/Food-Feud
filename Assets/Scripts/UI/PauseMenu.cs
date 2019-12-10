/* 
 * Created by:
 * Name: Alexander Watson
 * Sid: 1507490
 * Date Created: 05/10/2019
 * Last Modified: 10/12/2019
 * Modified by: Dominik Waldowski
 */
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool GameIsPaused = false;
    private GameObject pauseMenu;
    private Loading loading;

    private void Awake()
    {
        pauseMenu = GameObject.Find("PausePanel").gameObject;
        loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
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

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        ManageGame.instance.IsTimingDown = true;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        ManageGame.instance.IsTimingDown = false;
    }

    public void QuitGame()
    {
        loading.SetID(0);
        loading.InitializeLoading();
    }
}
