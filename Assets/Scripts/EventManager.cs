using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager Instance;

    public event Action FailEvent;
    public event Action DiamondMovementCompleted;
    public event Action RestartEvent;

    private void Awake()
    {
        Instance = this;
    }


    public void OnFailEvent()
    {
        FailEvent?.Invoke();
    }

    public void OnDiamondMovementCompleted()
    {
        DiamondMovementCompleted?.Invoke();
    }

    public void OnRestartEvent()
    {
        RestartEvent?.Invoke();
        InputController.Instance.gameIsOn = true;
    }
}
