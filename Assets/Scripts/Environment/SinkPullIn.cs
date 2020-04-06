using UnityEngine;

public class SinkPullIn : MonoBehaviour, ICanBeActivated
{
    [Header("Timer Settings")]
    [SerializeField]
    private float duration;
    [SerializeField]
    private float slowDownStart;
    [SerializeField]
    private float pullInStart;

    [Header("Object Settings")]
    [SerializeField]
    private GameObject waterPlane;
    [SerializeField]
    private GameObject tapWater;

    [Header("Speed Settings")]
    [SerializeField]
    private float baseWaterSpeed;
    [SerializeField]
    private float pullInSpeed;

    [Header("Modifiers")]
    [SerializeField, Range(0, 1)]
    private float speedDownModifier;

    private float elapsedTime;
    private float waterSpeed;
    private bool isPulling;
    private bool hasSlowed;
    private int playerCount;
    private bool isActive;

    private void Awake()
    {
        isActive = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive)
        {
            Timer();

            if (isPulling)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    Vector3 forceDirection = transform.position - ManageGame.instance.allPlayerControllers[i].transform.position;
                    ManageGame.instance.allPlayerControllers[i].chc.Move(forceDirection * pullInSpeed);
                }
            }
            waterPlane.transform.Translate(Vector3.up * waterSpeed * Time.deltaTime);
        }
    }

    private void Timer()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= slowDownStart && !hasSlowed)
            {
                hasSlowed = true;
                ModifySpeed(speedDownModifier);
            }

            if (elapsedTime >= pullInStart && !isPulling)
            {
                tapWater.SetActive(false);
                isPulling = true;
                waterSpeed = -baseWaterSpeed;
            }

            CheckTimerEnd();
        }
    }

    private void CheckTimerEnd()
    {
        if (elapsedTime >= duration)
        {
            ModifySpeed();

            isActive = false;
        }
    }

    private void ResetValues()
    {
        elapsedTime = 0;
        waterSpeed = baseWaterSpeed;
        isPulling = false;
        hasSlowed = false;
    }

    private void ModifySpeed(float multiplier = 1)
    {
        for (int i = 0; i < playerCount; i++)
        {
            PlayerController currentPlayer = ManageGame.instance.allPlayerControllers[i];
            currentPlayer.SetProperty<float>(nameof(currentPlayer.MoveSpeedModifier), currentPlayer.BaseSpeed * multiplier);
        }
    }

    public void Activate()
    {
        playerCount = ManageGame.instance.allPlayerControllers.Count;
        isActive = true;
        tapWater.SetActive(true);
        ResetValues();
    }
}
