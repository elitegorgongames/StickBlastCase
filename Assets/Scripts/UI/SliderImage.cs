using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SliderImage : MonoBehaviour
{

    public float targetPoint;
    public float currentPoint;
    public Slider slider;
    public TextMeshProUGUI currentPointText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.DiamondMovementCompleted += DiamondMovementCompleted;
        EventManager.Instance.RestartEvent += RestartEvent;
        SetSliderValue();
        slider.maxValue = targetPoint;
    }

    private void OnDestroy()
    {
        EventManager.Instance.DiamondMovementCompleted -= DiamondMovementCompleted;
        EventManager.Instance.RestartEvent -= RestartEvent;
    }


    public void SetSliderValue()
    {
        slider.value =(currentPoint / targetPoint)*targetPoint;

        if (currentPoint>=targetPoint)
        {
            EventManager.Instance.OnSuccessEvent();
        }

        SetCurrentPointText();
    }

    private void DiamondMovementCompleted()
    {
        currentPoint++;
        SetSliderValue();
    }

    private void RestartEvent()
    {
        currentPoint =0;
        SetSliderValue();
    }

    private void SetCurrentPointText()
    {
        currentPointText.text = currentPoint.ToString();
    }
}
