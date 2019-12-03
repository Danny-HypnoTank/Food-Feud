using UnityEngine;

public class GodPowerUpMovement : MonoBehaviour
{
    private float bombSpeed = 1;                                 //time until bomb disappears automatically                         //how far the bomb will travel
    private float bombTopHeight = 8;
    private float startTime;                                        //Currently time when bomb activates
    private float journeyLength;                                   //total length of journey travelled by bomb
    //private Vector3 startPos;
    private float distance;
    private Transform target, startPos;
    private GameObject[] nodes;
    private bool isRepeating;
    public int previousNode { get; set; }

    private void Awake()
    {

        nodes = GameObject.FindGameObjectsWithTag("NukeSpawn");
        isRepeating = false;

    }

    private void OnEnable()
    {

        ChooseRandomNode();

    }

    public void ChooseRandomNode()
    {

        if (!isRepeating)
        {

            InvokeRepeating(nameof(ChooseRandomNode), 5, 5);
            isRepeating = true;

        }

        startTime = Time.time;
        startPos = transform;

        int node = Random.Range(0, nodes.Length);
        target = nodes[node].transform;

        distance = Vector3.Distance(startPos.position, target.position);

    }

    private void Update()
    {

        if (transform.position != target.position)
        {
            float duration = (Time.time - startTime) * 5;
            float fraction = duration / distance;
            transform.position = Vector3.Lerp(startPos.position, target.position, fraction);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            if (other.gameObject.GetComponent<PlayerBase>().CurrentPowerUp == null)
            {
                other.GetComponent<PlayerBase>().SetPowerUp(GetComponent<Power>().PowerHeld);
                gameObject.SetActive(false);
            }
    }

    private void OnDisable()
    {

        Debug.Log("Disabled");

    }

}

/*public void FindClosestNode()
{
    startPos = this.transform.position;
    GameObject[] allEdges = ManageGame.instance.MapEdgesForGodPower.ToArray();
    float nearestEdge = Mathf.Infinity;
    Transform target = null;
    for (int i = 0; i < allEdges.Length; i++)
    {
        float distance = Vector3.Distance(startPos, allEdges[i].transform.position);
        if (distance < nearestEdge)
        {
            nearestEdge = distance;
            target = allEdges[i].transform;
        }
    }
    endPos = target.transform.position;
    targetAppear = target.transform;

    SetParabola();
}

public void FindFurthestNode()
{
    startPos = this.transform.position;
    GameObject[] allEdges = ManageGame.instance.MapEdgesForGodPower.ToArray();
    float furthestEdge = 0;
    Transform target = null;
    for (int i = 0; i < allEdges.Length; i++)
    {
        float distance = Vector3.Distance(startPos, allEdges[i].transform.position);
        Debug.Log(allEdges[i].transform.name + " / " + distance + "/ " + startPos);
        if (distance > furthestEdge)
        {
            furthestEdge = distance;
            target = allEdges[i].transform;
        }
    }
    endPos = target.transform.position;

    targetAppear = target.transform;
    SetParabola();
}

private void SetParabola()
{
    startTime = 0;
    startTime = Time.time;
    journeyLength = Vector3.Distance(startPos, endPos);
    startedFlying = true;
}

private void Update()
{
    if (startedFlying == true)
    {
        float distCovered = (Time.time - startTime) * bombSpeed;
        journeyLength = Vector3.Distance(startPos, endPos);
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = MathParabola.Parabola(startPos, endPos, bombTopHeight, fractionOfJourney * bombSpeed);
        if (fractionOfJourney >= 1)
        {
            canCollide = true;
            startedFlying = false;
            this.transform.position = endPos;
            targetAppear.GetComponent<EdgePowerUpGodPower>().EnablePowerGodPower();
            this.gameObject.SetActive(false);
        }
    }
}*/
