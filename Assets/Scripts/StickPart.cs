using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StickPart : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Material dissolveMaterial;

    public bool collideWithCompletedObject;
    public bool isDissolving;


    Vector3 _initialScale;
    Transform _transform;


    private void Awake()
    {
        _transform = transform;
        _initialScale = _transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SettleMovement()
    {       
        _transform.DOScale(_initialScale * PolishSettings.Instance.stickSettleScaleMultiplier, PolishSettings.Instance.stickSettleScaleTime).SetEase(PolishSettings.Instance.stickSettleScaleAC)
            .OnComplete(BackToInitialScale);
    }

    private void BackToInitialScale()
    {
        _transform.DOScale(_initialScale, PolishSettings.Instance.stickSettleScaleTime).SetEase(PolishSettings.Instance.stickSettleScaleAC);
    }

    IEnumerator DissolveWithDelay()
    {
        isDissolving = true;
        yield return new WaitForSeconds(.1f);
        if (collideWithCompletedObject)
        {
            isDissolving = false;
            yield break;
        }
      
        Debug.Log("dissolve stick part");
        var materialToDissolve = Instantiate(PolishSettings.Instance.dissolveMaterial);
        spriteRenderer.material = materialToDissolve;
        float dissolveValue = 1;
        float randomOffset = Random.Range(0, 50f);
        materialToDissolve.SetFloat("_Offset", randomOffset);

        DOTween.To(() => dissolveValue, x => dissolveValue = x, 0, .5f)
        .OnUpdate(() =>
        {
            materialToDissolve.SetFloat("_Fade", dissolveValue);
        }).OnComplete(() =>
        {
            ConnectionStickManager.Instance.CheckCompletedCircleNodes();
            DestroyObject();           
        });
        yield return new WaitForSeconds(.1f);
        ConnectionStickManager.Instance.UpdateConnectionStickHighlightStates();
       
    }
    public void Dissolve()
    {
        StartCoroutine(DissolveWithDelay());
        return;        
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CompletedCircleNodeObjectImage completedCircleNodeObjectImage))
        {
            collideWithCompletedObject = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CompletedCircleNodeObjectImage completedCircleNodeObjectImage))
        {
            collideWithCompletedObject = false;
        }
    }
}
