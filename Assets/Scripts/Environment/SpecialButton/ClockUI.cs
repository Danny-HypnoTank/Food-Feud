using UnityEngine;

public class ClockUI : MonoBehaviour, ISpecialUI
{

    [Header("Bar Properties")]
    [SerializeField]
    private GameObject handle;
    [SerializeField]
    private Vector3 handleEndRot;
    [SerializeField]
    private float lerpTime;

    private float timer;
    private float activationTime;
    private Vector3 handleStartRot;

    private void Awake()
    {
        //If the handle is set, change the initial value
        if (handle != null)
        {
            handleStartRot = handle.transform.eulerAngles;
        }
    }

    public void GetActivationTime(float time)
    {
        //Set the activation time to the time parameter
        activationTime = time;
    }

    public void UpdateUI(float time)
    {
        //If the handle is set, run the logic for updating the UI
        if (handle != null)
        {
            //Timer for lerping
            if (timer < activationTime)
                timer += Time.deltaTime;

            //If the parameter time is less than the activation time, lerp the handle towards the end rotation
            if (time < activationTime)
            {
                handle.transform.eulerAngles = Vector3.Lerp(handleStartRot, handleEndRot, timer / lerpTime);
            }
        }
    }
}
