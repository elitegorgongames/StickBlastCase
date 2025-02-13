using DG.Tweening;
using UnityEngine;

public class CompletedCircleNodeObjectImage : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Material dissolveMaterial;
    public Vector3 initialTargetScale;
    public float scaleTime;
    public AnimationCurve scaleAC;

    public CircleNode belongedCircleNode;

    public Diamond diamondPrefab;
    public Material diagonalShineMaterial;

    private bool _closeShine;
    public float shineTimer;

    BoxCollider _collider;

    Transform _transform;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _transform = transform;

        diagonalShineMaterial = Instantiate(spriteRenderer.material);
        spriteRenderer.material = diagonalShineMaterial;
        spriteRenderer.material.SetFloat("_StartTime", Time.time-.2f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.RestartEvent += DestroyObject;
        ScaleUp();

        ConnectionStickManager.Instance.completedObjectsList.Add(this);
        SoundManager.Instance.PlayShineAudioClip();


    }

    private void OnDestroy()
    {
        EventManager.Instance.RestartEvent -= DestroyObject;
        ConnectionStickManager.Instance.completedObjectsList.Remove(this);
    }

    private void Update()
    {
        if (_closeShine)
        {
            return;
        }

        shineTimer -= Time.deltaTime;
        if (shineTimer <= 0)
        {
            _closeShine = true;
            spriteRenderer.material.SetFloat("_WaveSpeed", 0);
        }
    }

    private void ScaleUp()
    {
        _transform.DOScale(initialTargetScale, scaleTime).SetEase(scaleAC);
    }

    public void Dissolve()
    {
        //Debug.Log("dissolve stick part");
        _collider.size *= .1f;
        //_collider.enabled = false;
        var materialToDissolve = Instantiate(dissolveMaterial);
        spriteRenderer.material = materialToDissolve;
        float dissolveValue = 1;
        float randomOffset = Random.Range(0, 50f);
        materialToDissolve.SetFloat("_Offset", randomOffset);
    
        DOTween.To(() => dissolveValue, x => dissolveValue = x, 0, .5f)
        .OnUpdate(() =>
        {
            materialToDissolve.SetFloat("_Fade", dissolveValue);    
        }).OnComplete(DestroyObject);

        var diamond = Instantiate(diamondPrefab,PolishSettings.Instance.diamondTargetTransform);
        diamond.transform.position = Camera.main.WorldToScreenPoint(_transform.position);

        SoundManager.Instance.PlayDissolveAudioClip();
    }

    public void SetDiagonalShaderMaterial()
    {

    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
