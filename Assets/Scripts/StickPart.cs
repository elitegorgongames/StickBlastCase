using DG.Tweening;
using UnityEngine;

public class StickPart : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Material dissolveMaterial;

    public bool collideWithCompletedObject;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void Dissolve()
    {
        if (collideWithCompletedObject)
        {
            return;
        }

        var materialToDissolve = Instantiate(PolishSettings.Instance.dissolveMaterial);
        spriteRenderer.material = materialToDissolve;
        float dissolveValue = 1;
        float randomOffset = Random.Range(0, 50f);
        materialToDissolve.SetFloat("_Offset", randomOffset);

        DOTween.To(() => dissolveValue, x => dissolveValue = x, 0, .5f)
        .OnUpdate(() =>
        {
            materialToDissolve.SetFloat("_Fade", dissolveValue);
        }).OnComplete(DestroyObject);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (TryGetComponent(out CompletedCircleNodeObjectImage completedCircleNodeObjectImage))
    //    {
    //        collideWithCompletedObject = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (TryGetComponent(out CompletedCircleNodeObjectImage completedCircleNodeObjectImage))
    //    {
    //        collideWithCompletedObject = false;
    //    }
    //}
}
