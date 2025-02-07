using DG.Tweening;
using UnityEngine;

public class Diamond : MonoBehaviour
{


    private Vector3 _initialScale;


    Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _initialScale = _transform.localScale;
    }


    void Start()
    {
        MoveToTarget();
    }



    public void MoveToTarget()
    {
        var targetScale = PolishSettings.Instance.diamondTargetScale;
        var scaleTime = PolishSettings.Instance.diamondScaleTime;

        var diamondTargetObject = PolishSettings.Instance.diamondTargetTransform;
        var targetPosition = Camera.main.ScreenToWorldPoint(diamondTargetObject.position);
        var distance = Vector3.Distance(targetPosition, _transform.position);

        var baseMoveSpeed = PolishSettings.Instance.diamondMoveSpeed;
        var moveSpeed = Random.Range(baseMoveSpeed - PolishSettings.Instance.diamondMoveSpeedInterval, baseMoveSpeed + PolishSettings.Instance.diamondMoveSpeedInterval);
        var moveTime = distance / PolishSettings.Instance.diamondMoveSpeed;

        var sequence = DOTween.Sequence();

        sequence.Append(_transform.DOScale(targetScale, scaleTime)).Append(_transform.DOScale(_initialScale, scaleTime)).
            Append(_transform.DOMove(targetPosition, moveTime)).SetEase(PolishSettings.Instance.diamondMoveAC).OnComplete(() =>
            {
                EventManager.Instance.OnDiamondMovementCompleted();
                DestroyObject();
            });
        
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
