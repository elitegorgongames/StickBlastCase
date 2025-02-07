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

    public int currentStickCount;


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
            Stick stickToSpawn = null;
            bool fitFound = false;

            // Uyan bir stick bulunana kadar döngüye gir
            while (!fitFound)
            {
                int stickOrder = Random.Range(0, stickPrefabsList.Count);
                stickToSpawn = Instantiate(stickPrefabsList[stickOrder], initialStickSpawnPoint.position, Quaternion.identity);

                var circleNodesList = GridManager.Instance.GetAllCircleNodes();
                foreach (var cNode in circleNodesList)
                {
                    if (GridManager.Instance.IsStickFitIntoTheCircleNode(stickToSpawn, cNode))
                    {
                        Debug.Log("Stick fits for circle node: " + cNode.coordinate);
                        fitFound = true;
                        break;
                    }
                }

                // Eðer uygun bir node bulunamadýysa, spawn edilen stick'i yok et ve yeniden dene
                if (!fitFound)
                {
                    Destroy(stickToSpawn.gameObject);
                    yield return null; // Bir sonraki frame'de tekrar dene (alternatif olarak küçük bir gecikme de ekleyebilirsin)
                }
            }

            // Uyan stick bulundu, hedef konuma taþýyoruz
            stickToSpawn.MoveToTarget(stickSpawnPointsList[i].position);
            yield return new WaitForSeconds(delayTime);
        }

        currentStickCount = spawnStickCount;
    }

    public void DecreaseCurrentStickCount()
    {
        currentStickCount--;

        if (currentStickCount==0)
        {
            StartCoroutine(SpawnSticksWithDelay());
        }
    }
}
