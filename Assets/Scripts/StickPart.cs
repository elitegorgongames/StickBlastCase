using DG.Tweening;
using UnityEngine;

public class StickPart : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Material dissolveMaterial;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void Dissolve()
    {
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
}
