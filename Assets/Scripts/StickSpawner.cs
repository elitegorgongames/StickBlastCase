using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StickSpawner : MonoBehaviour
{
    public static StickSpawner Instance;

    public List<Stick> stickPrefabsList;
    public float delayTime;
    public int spawnStickCount;
    public List<Transform> stickSpawnPointsList;
    public Transform initialStickSpawnPoint;

    public List<Stick> currentStickList;

    public int currentStickCount;

    public Volume postProcessVolume;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.RestartEvent += RestartEvent;
        StartCoroutine(SpawnSticksWithDelay());
    }

    private void OnDestroy()
    {
        EventManager.Instance.RestartEvent -= RestartEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(SpawnSticksWithDelay());
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Camera.main.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = !Camera.main.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing;
        }
    }

 

    IEnumerator SpawnSticksWithDelay()
    {
        for (int i = 0; i < spawnStickCount; i++)
        {
            Stick stickToSpawn = null;
            bool fitFound = false;

      
            while (!fitFound)
            {
                int stickOrder = Random.Range(0, stickPrefabsList.Count);
                stickToSpawn = Instantiate(stickPrefabsList[stickOrder], initialStickSpawnPoint.position, Quaternion.identity);
                currentStickList.Add(stickToSpawn);

                var circleNodesList = GridManager.Instance.GetAllCircleNodes();
                foreach (var cNode in circleNodesList)
                {
                    if (GridManager.Instance.IsStickFitIntoTheCircleNode(stickToSpawn, cNode))
                    {
                        //Debug.Log("Stick fits for circle node: " + cNode.coordinate);
                        fitFound = true;
                        break;
                    }
                }

                
                if (!fitFound)
                {
                    Destroy(stickToSpawn.gameObject);
                    currentStickList.Remove(stickToSpawn);
                    yield return null;
                }
            }


            SoundManager.Instance.PlaySpawnStickClip();
            stickToSpawn.MoveToTarget(stickSpawnPointsList[i].position);
            yield return new WaitForSeconds(delayTime);
        }

        currentStickCount = spawnStickCount;
    }

    public void DecreaseCurrentStickCount(Stick stickToRemove)
    {
        currentStickCount--;
        bool fitFound = false;
        currentStickList.Remove(stickToRemove);
        if (currentStickCount > 0)
        {
            var circleNodesList = GridManager.Instance.GetAllCircleNodes();
            foreach (var stick in currentStickList)
            {
                foreach (var cNode in circleNodesList)
                {
                    if (GridManager.Instance.IsStickFitIntoTheCircleNode(stick, cNode))
                    {
                        //Debug.Log("Stick fits for circle node: " + cNode.coordinate);
                        fitFound = true;
                        break;
                    }
                }
            }
            if (!fitFound)
            {
                //EventManager.Instance.OnFailEvent();
            }
        }


        if (currentStickCount==0)
        {
            StartCoroutine(SpawnSticksWithDelay());
        }
    }
    
    public void FailCheck()
    {
        StartCoroutine(FailCheckWithDelay());
    }

    IEnumerator FailCheckWithDelay()
    {
        bool fitFound = false;
        var circleNodesList = GridManager.Instance.GetAllCircleNodes();
        if (currentStickList.Count>0)
        {
            foreach (var stick in currentStickList)
            {
                foreach (var cNode in circleNodesList)
                {
                    if (GridManager.Instance.IsStickFitIntoTheCircleNode(stick, cNode))
                    {
                        fitFound = true;
                        break;
                    }
                }
            }
            if (!fitFound)
            {
                InputController.Instance.gameIsOn = false;
                yield return new WaitForSeconds(.5f);
                currentStickList.Clear();
                EventManager.Instance.OnFailEvent();
            }
        }
  
    }

    private void RestartEvent()
    {
        StartCoroutine(SpawnSticksWithDelay());
    }
}
