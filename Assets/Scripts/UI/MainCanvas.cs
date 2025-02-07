using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{

    public GameObject failPanelObject;

    public List<GameObject> failedLettersList;
    public List<Vector3> failedLettersTargetPointList;
    public float moveLetterTime;
    public float moveDelayTime;
    public Transform failLettersInitialPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.FailEvent += EnableFailPanel;
        EventManager.Instance.RestartEvent += DisableFailPanel;

        DisableFailPanel();
    }

    private void OnDestroy()
    {
        EventManager.Instance.FailEvent -= EnableFailPanel;
        EventManager.Instance.RestartEvent -= DisableFailPanel;
    }

    private void EnableFailPanel()
    {
        failPanelObject.SetActive(true);

        MoveFailLetters();
    }

    private void DisableFailPanel()
    {
        failPanelObject.SetActive(false);

        foreach (var letter in failedLettersList)
        {
            letter.transform.position = failLettersInitialPoint.position;
        }
    }

    public void MoveFailLetters() 
    {
        StartCoroutine(MoveFailLettersWithDelay());
    }

    IEnumerator MoveFailLettersWithDelay()
    {
        for (int i = 0; i < failedLettersList.Count; i++)
        {
            GameObject letter = failedLettersList[i];
            letter.transform.DOMove(failedLettersTargetPointList[i], moveLetterTime);

            yield return new WaitForSeconds(moveDelayTime);
        }
    }

    public void RestartButton()
    {
        EventManager.Instance.OnRestartEvent();
    }
}
