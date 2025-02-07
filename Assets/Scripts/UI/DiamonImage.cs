using UnityEngine;
using DG.Tweening;
using TMPro;

public class DiamonImage : MonoBehaviour
{

    public float scaleTime;
    public float scaleMultiplier;
    public bool scaleState;
    public Vector3 initialScale;
    public TextMeshProUGUI diamondAmount;
    public int currentDiamondAmount;

    Transform _transform;

    public object DoTween { get; private set; }

    private void Awake()
    {
        _transform = transform;
        initialScale = _transform.localScale;
    }

    private void Start()
    {
        EventManager.Instance.DiamondMovementCompleted += Scale;
    }

    private void OnDestroy()
    {
        EventManager.Instance.DiamondMovementCompleted -= Scale;
    }


    public void Scale()
    {
        currentDiamondAmount++;
        DiamondAmountChanged();

        if (scaleState)
        {
            DOTween.Kill(_transform);
        }

        _transform.DOScale(initialScale * scaleMultiplier, scaleTime).OnComplete(ScaleBack);
        scaleState = true;
    }

    public void ScaleBack()
    {
        _transform.DOScale(initialScale, scaleTime);
    }

    private void DiamondAmountChanged()
    {
        diamondAmount.text = currentDiamondAmount.ToString();
    }
}
