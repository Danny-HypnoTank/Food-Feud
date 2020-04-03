using UnityEngine;

[CreateAssetMenu(fileName = "NewSpecialUI", menuName = "SpecialUI/Dial UI")]
public class SpecialButtonDialUI : SpecialUILogic
{
    [Header("Dial Properties")]
    [SerializeField]
    private Vector3 handleEndRot;

    private float timer;
    private float activationTime;
    private Vector3 handleStartRot;
    private GameObject handle;

    public override void Initialisation(float time)
    {
        //Find the handle in the scene
        handle = GameObject.FindGameObjectWithTag("Handle");

        //If the handle is found, change the initial value
        if (handle != null)
            handleStartRot = handle.transform.eulerAngles;

        //Set the activation time to the time parameter
        activationTime = time;
    }

    public override void UpdateUI(float time)
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
                handle.transform.eulerAngles = Vector3.Lerp(handleStartRot, handleEndRot, timer / activationTime);
            }
        }
    }
}
