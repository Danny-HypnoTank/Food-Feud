using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levelLayouts;
    [SerializeField]
    private GameObject map;
    private GameObject[] spawnPoints;
    private List<GameObject> paintableObjects = new List<GameObject>();

    private int layoutIteration = 0;

    public GameObject[] SpawnPoints { get => spawnPoints; set => spawnPoints = value; }
    public List<GameObject> PaintableObjects { get => paintableObjects; set => paintableObjects = value; }

    private void Start()
    {
        
    }

    public void LayoutGeneration()
    {
        layoutIteration = Random.Range(0, levelLayouts.Length);
        Instantiate(levelLayouts[layoutIteration],new Vector3 (0,0,0), Quaternion.identity, map.transform);
        SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        PaintableObjects.AddRange(GameObject.FindGameObjectsWithTag("PaintableEnvironment"));
        var index = PaintableObjects.FindIndex(x => x.name == "Floor");
        var item = PaintableObjects[index];
        //PaintableObjects[0] = item;
    }
}
