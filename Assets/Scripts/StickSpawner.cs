using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickSpawner : MonoBehaviour
{
    public static StickSpawner Instance;

    public List<Stick> stickPrefabsList;
    public float delayTime;
    public int spawnStickCount;
    public List<Transform> stickSpawnPointsList;
    public Transform initialStickSpawnPoint;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnSticksWithDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(SpawnSticksWithDelay());
        }
    }

    IEnumerator SpawnSticksWithDelay()
    {
        for (int i = 0; i < spawnStickCount; i++)
        {
            var stickOrder = Random.Range(0, stickPrefabsList.Count);

            var stickToSpawn = Instantiate(stickPrefabsList[stickOrder], initialStickSpawnPoint.position,Quaternion.identity);

            var circleNodesList = GridManager.Instance.GetAllCircleNodes();
            foreach (var cNode in circleNodesList)
            {
                if (GridManager.Instance.IsStickFitIntoTheCircleNode(stickToSpawn, cNode))
                {
                    Debug.Log("stick is fit for cNode " + cNode.coordinate);
                    break;
                }
            }
            stickToSpawn.MoveToTarget(stickSpawnPointsList[i].position);

            yield return new WaitForSeconds(delayTime);
        }       
    }
}
