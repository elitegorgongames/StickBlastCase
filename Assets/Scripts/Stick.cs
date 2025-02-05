using UnityEngine;
using DG.Tweening;


public class Stick : MonoBehaviour
{

    public bool isPicked;
    public bool isPlaced;
    public StickType stickType; 
 
    public Transform calculationTransformStartPoint;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
    }



    public void PlaceToGrid(Vector3 referenceCircleNode)
    {
        var offset = referenceCircleNode - calculationTransformStartPoint.position;

        var targetPos = _transform.position + offset;
        var moveTime = PolishSettings.Instance.stickPlacementTime;
        var moveAC = PolishSettings.Instance.stickPlacementAC;

        _transform.DOMove(targetPos, moveTime).SetEase(moveAC);

        Debug.Log("stick movement"+ offset+" offset"+ targetPos);
    }


    public Transform GetCalculationTransformStartPoint()
    {
        return calculationTransformStartPoint;
    }

}


public enum StickType
{
    Vertical,
    DoubleLengthVertical,
    DoubleLengthHorizontal,
    Horizontal,
    LType,    
    DownwardsLType,
    MirroredLType,
    DoubleLengthLType,
    UType,
    DownwardsUType
};
