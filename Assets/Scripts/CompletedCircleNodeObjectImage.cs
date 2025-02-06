using DG.Tweening;
using UnityEngine;

public class CompletedCircleNodeObjectImage : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Material dissolveMaterial;
    public Vector3 initialTargetScale;
    public float scaleTime;
    public AnimationCurve scaleAC;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScaleUp();
    }



    private void ScaleUp()
    {
        _transform.DOScale(initialTargetScale, scaleTime).SetEase(scaleAC);
    }

    public void Dissolve()
    {  
        var materialToDissolve = Instantiate(dissolveMaterial);
        spriteRenderer.material = materialToDissolve;
        float dissolveValue = 1;
        float randomOffset = Random.Range(0, 50f);
        materialToDissolve.SetFloat("_Offset", randomOffset);
    
        DOTween.To(() => dissolveValue, x => dissolveValue = x, 0, .5f)
        .OnUpdate(() =>
        {
            materialToDissolve.SetFloat("_Fade", dissolveValue);    
        })/*.OnComplete(DestroyObject)*/;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
