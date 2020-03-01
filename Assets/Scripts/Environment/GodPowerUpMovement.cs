using UnityEngine;

public class GodPowerUpMovement : MonoBehaviour
{

    private float startTime;
    private float distance;
    private Transform target, startPos;
    private Vector3 spawnPos;
    private GameObject[] nodes;
    private bool isRepeating;
    public int previousNode { get; set; }

    [Header("Properties")]
    [SerializeField]
    private float speed = 25;

    [Header("Bounce Properties")]
    [SerializeField]
    private bool usebounce;
    [SerializeField]
    private LayerMask bounceSurface;
    private void Awake()
    {

        nodes = GameObject.FindGameObjectsWithTag("NukeSpawn");
        isRepeating = false;
        spawnPos = transform.position;
    }

    private void OnEnable()
    {

        if (!usebounce)
            ChooseRandomNode();
        else
        {
            transform.position = spawnPos;
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

    }

    private void Bouncing()
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, Time.deltaTime * speed + 0.1f, bounceSurface))
        {
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
            float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
        }


    }

    public void ChooseRandomNode()
    {

        if (!isRepeating)
        {

            InvokeRepeating(nameof(ChooseRandomNode), 10, 10);
            isRepeating = true;

        }

        startTime = Time.time;
        startPos = transform;

        int node = Random.Range(0, nodes.Length);
        target = nodes[node].transform;

        distance = Vector3.Distance(startPos.position, target.position);

    }

    private void FixedUpdate()
    {
        if (usebounce)
            Bouncing();
        else
        {
            if (transform.position != target.position)
            {
                float duration = (Time.time - startTime) * 2.5f;
                float fraction = duration / distance;
                transform.position = Vector3.Lerp(startPos.position, target.position, fraction);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            Camera.main.GetComponent<CameraShaker>().ShakeCamera();
            ManageGame.instance.GodPowerUp.SetActive(false);
            player.Splat(2);
        }
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
