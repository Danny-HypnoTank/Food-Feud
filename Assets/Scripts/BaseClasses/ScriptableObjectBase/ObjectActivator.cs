using UnityEngine;

[CreateAssetMenu(fileName = "NewButtonLogic", menuName = "Special Button/Object Activator")]
public class ObjectActivator : SpecialButtonLogic
{
    [Header("Object Properties")]
    [SerializeField]
    private string objectTag;

    private ICanBeActivated objectToActivate;

    public override void Initialisation()
    {
        objectToActivate = GameObject.FindGameObjectWithTag(objectTag).GetComponent<ICanBeActivated>();
        Debug.Log(objectToActivate);
    }
    public override void DoAction()
    {
        objectToActivate.Activate();
    }
}
