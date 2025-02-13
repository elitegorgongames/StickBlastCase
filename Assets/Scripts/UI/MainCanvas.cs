using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{

    public GameObject failPanelObject;
    public GameObject successPanelObject;

    public List<GameObject> failedLettersList;
    public List<Vector3> failedLettersTargetPointList;
    public float moveLetterTime;
    public float moveDelayTime;
    public Transform failLettersInitialPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.FailEvent += EnableFailPanel;
        EventManager.Instance.SuccessEvent += EnableSuccessPanel;
        EventManager.Instance.RestartEvent += DisableFailPanel;
        EventManager.Instance.RestartEvent += DisableSuccessPanel;

        SetFailedLettersPoints();
        DisableFailPanel();     
    }

    private void OnDestroy()
    {
        EventManager.Instance.FailEvent -= EnableFailPanel;
        EventManager.Instance.SuccessEvent -= EnableSuccessPanel;
        EventManager.Instance.RestartEvent -= DisableFailPanel;
        EventManager.Instance.RestartEvent -= DisableSuccessPanel;
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

    private void EnableSuccessPanel()
    {
        successPanelObject.SetActive(true);
        SoundManager.Instance.PlaySuccessAudioClip();
    }

    private void DisableSuccessPanel()
    {
        successPanelObject.SetActive(false);
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

    private void SetFailedLettersPoints()
    {
        for (int i = 0; i < failedLettersList.Count; i++)
        {
            failedLettersTargetPointList.Add(failedLettersList[i].transform.position);
        }
    }

    public void RestartButton()
    {
        SoundManager.Instance.PlayUIAudioClip();
        EventManager.Instance.OnRestartEvent();
    }
}
