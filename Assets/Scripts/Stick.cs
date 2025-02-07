using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Stick : MonoBehaviour
{

    public bool isPicked;
    public bool isPlaced;
    public StickType stickType;
    public Vector3 startPoint;
 
    public Transform calculationTransformStartPoint;
    private Transform _transform;

    

    private void Awake()
    {
        _transform = transform;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetStartPoint();
    }


    public void MoveToTarget(Vector3 target)
    {
        var distance = Vector3.Distance(_transform.position, target);
        var moveSpeed = PolishSettings.Instance.moveToStartPointSpeed;
        var moveTime = distance/moveSpeed;

        _transform.DOMove(target, moveTime).OnComplete(SetStartPoint);
    }

    public void PlaceToGrid(Vector3 referenceCircleNode)
    {
        var offset = referenceCircleNode - calculationTransformStartPoint.position;

        var targetPos = _transform.position + offset;
        var moveTime = PolishSettings.Instance.stickPlacementTime;
        var moveAC = PolishSettings.Instance.stickPlacementAC;

        isPlaced = true;

        _transform.DOMove(targetPos, moveTime).SetEase(moveAC).OnComplete(()=> 
        {
            GridManager.Instance.CheckIfAnyCircleNodeIsCompleted();
            ConnectionStickManager.Instance.UpdateConnectionStickOccupiedStates();
            StickSpawner.Instance.DecreaseCurrentStickCount();
        });
    }

    public void BackToStartPoint()
    {
        var moveTime = PolishSettings.Instance.stickPlacementTime;
        var moveAC = PolishSettings.Instance.stickPlacementAC;
        _transform.DOMove(startPoint, moveTime).SetEase(moveAC);
    }


    public void SetStartPoint()
    {
        startPoint = _transform.position;
    }

    public Transform GetCalculationTransformStartPoint()
    {
        return calculationTransformStartPoint;
    }
}


public enum StickType
{
    Vertical,
    Horizontal,
    LType,
    UType,
    DoubleLengthVertical,
    DoubleLengthHorizontal,     
    DownwardsLType,
    MirroredLType,
    DoubleLengthLType,
    DownwardsUType
};
