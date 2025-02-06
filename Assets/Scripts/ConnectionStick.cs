using UnityEngine;

public class ConnectionStick : MonoBehaviour
{

    public bool isOccupied;

    public Color hightlightColor;
    public Color initialColor;
    public SpriteRenderer spriteRenderer;

    public GameObject highlightObject;
    public GameObject defaultStickObject;

    public Transform transformToSendRay;
    public LayerMask stickPartLayer;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;

        initialColor = spriteRenderer.color;
    }

    private void Update()
    {
        //float maxRange = 5;
        //RaycastHit hit;

        //if (Physics.Raycast(transformToSendRay.position, (-Vector3.forward), out hit, maxRange))
        //{
        //    if (hit.transform.gameObject.TryGetComponent(out Stick stick))
        //    {
        //        Debug.DrawRay(transformToSendRay.position, -Vector3.forward, Color.blue, 1f);
        //        stick.gameObject.AddComponent<Rigidbody>();
        //    }
        //}
    }

    public void SendRayToFindStick()
    {
        float maxRange = 5f;
        RaycastHit hit;

        if (Physics.Raycast(transformToSendRay.position, -Vector3.forward, out hit, maxRange, stickPartLayer))
        {
            Debug.DrawRay(transformToSendRay.position, -Vector3.forward * maxRange, Color.blue, 10f);

            if (hit.transform.gameObject.TryGetComponent(out StickPart stickPart))
            {
                stickPart.Dissolve();
                isOccupied = false;
            }
        }
    }

    public Transform GetTransform()
    {
        return _transform;
    }

    public void SetHighlightColor()
    {
        spriteRenderer.color = hightlightColor;
        highlightObject.SetActive(true);
        defaultStickObject.SetActive(false);
    }

    public void SetInitialColor()
    {
        spriteRenderer.color = initialColor;
        highlightObject.SetActive(false);
        defaultStickObject.SetActive(true);
    }
}
