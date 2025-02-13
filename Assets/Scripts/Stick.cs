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

    public bool isPickable;
    public bool firstPick;

    public List<StickPart> stickPartsList;
    public List<SpriteRenderer> spriteRenderers;
    

    private void Awake()
    {
        _transform = transform;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.RestartEvent += RestartEvent;

        foreach (var sRenderer in spriteRenderers)
        {
            sRenderer.sortingOrder = 4;
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.RestartEvent -= RestartEvent;
    }

    public void FirstPickMovement(Vector3 target)
    {
        firstPick = true;

        _transform.DOMove(target, PolishSettings.Instance.stickFirstHoldOffsetMovementTime).OnComplete(() =>
        {
            firstPick = false;
        });

        SoundManager.Instance.PlayPickStickAudioClip();
    }

    public void MoveToTarget(Vector3 target)
    {
        var distance = Vector3.Distance(_transform.position, target);
        var moveSpeed = PolishSettings.Instance.moveToStartPointSpeed;
        var moveTime = PolishSettings.Instance.moveToStartPointTime;

        var pos = _transform.position; 

        if (stickType==StickType.Vertical)
        {
            pos.y = -2.9f;
         
            target.y = -2.9f;
        }
        else
        {
            pos.y = target.y;
        }
        _transform.position = pos;
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
            StickPartSettleMovement();
            GridManager.Instance.CheckIfAnyCircleNodeIsCompleted();
            ConnectionStickManager.Instance.UpdateConnectionStickOccupiedStates();          
            StickSpawner.Instance.DecreaseCurrentStickCount(this);
            StickSpawner.Instance.FailCheck();
            ChangeOrderInLayer();
            SoundManager.Instance.PlaySettleStickAudioClip();
        });
    }

    private void StickPartSettleMovement()
    {
        foreach (var stickPart in stickPartsList)
        {
            stickPart.SettleMovement();
        }
    }

    public void BackToStartPoint()
    {
        isPickable = false;
        var moveTime = PolishSettings.Instance.stickPlacementTime;
        var moveAC = PolishSettings.Instance.stickPlacementAC;
        _transform.DOMove(startPoint, moveTime).SetEase(moveAC).OnComplete(()=>
        {
            isPickable = true;
        });

        SoundManager.Instance.PlayStickBackToInitialPointClip();
    }

    private void ChangeOrderInLayer()
    {
        foreach (var sRenderer in spriteRenderers)
        {
            sRenderer.sortingOrder = 2;
        }
    }

    public void SetStartPoint()
    {
        startPoint = _transform.position;
        isPickable = true;
    }

    public Transform GetCalculationTransformStartPoint()
    {
        return calculationTransformStartPoint;
    }

    private void RestartEvent()
    {
        Destroy(gameObject);
    }
}


public enum StickType
{
    Vertical,
    Horizontal,
    LType,
    UType,
    MirroredLType,
    DownwardsUType,
    DoubleLengthVertical,
    DoubleLengthHorizontal,     
    DownwardsLType,
    DoubleLengthLType 
};
