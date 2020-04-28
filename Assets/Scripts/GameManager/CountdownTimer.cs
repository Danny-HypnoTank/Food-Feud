/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 29/09/2019
 */ 
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField]
    private Sprite[] countDownImages;
    [SerializeField]
    private Image countdownVisual;
    [SerializeField]
    private int countDown;
    [SerializeField]
    private float timer;
    [SerializeField]
    [Range(1,3)]
    private int rate;
    [SerializeField]
    private bool isCounting;

    SoundManager soundManager;

    private void Start()
    {
        isCounting = true;
        countDown = 0;
        countdownVisual.sprite = countDownImages[countDown];
        countdownVisual.gameObject.SetActive(false);
    }

    //handles countdown timer by changing between 4 sprites
    private void Update()
    {
        if (isCounting == true)
        {
            countdownVisual.sprite = countDownImages[countDown];
            timer += Time.deltaTime;
            if (timer >= 1.0f && timer <= 1.99f)
            {
                countdownVisual.gameObject.SetActive(true);
                countDown = 0;
                

            }
            else if (timer >= 2.0f && timer <= 2.99f)
            {
                countDown = 1;
            }
            else if (timer >= 3.0f && timer <= 3.99f)
            {
                countDown = 2;
            }
            else if (timer >= 4.0f && timer <= 5.0f)
            {
                countDown = 3;
            }
            else if (timer >= 5.1f)
            {
                countdownVisual.gameObject.SetActive(false);
                ManageGame.instance.StartTimer();
                isCounting = false;
                soundManager = SoundManager.Instance;
                soundManager.AudioSource.Play();
            }          
        }
    }
}
