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

    public CircleNode rightCircleNode;
    public CircleNode leftCircleNode;
    public CircleNode upCircleNode;
    public CircleNode downCircleNode;

    public Stick verticalStick;
    public Stick horizontalStick;
    public Transform verticallStickTransform;
    public Transform horizontalStickTransform;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;

        initialColor = spriteRenderer.color;
    }

    private void Start()
    {
        ConnectionStickManager.Instance.AddToConnectionStickList(this);
    }

    public void SpawnVerticalStick()
    {
        var stick = Instantiate(verticalStick);
        stick.transform.position = verticallStickTransform.position;
    }

    public void SpawnHorizontalStick()
    {
        var stick = Instantiate(horizontalStick);
        stick.transform.position = horizontalStickTransform.position;
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

        SetCircleNodeHighlithStates();
    }

    public void SetOccupiedState()
    {
        float maxRange = 5f;
        RaycastHit hit;

        if (Physics.Raycast(transformToSendRay.position, -Vector3.forward, out hit, maxRange, stickPartLayer))
        { 
            if (hit.transform.gameObject.TryGetComponent(out StickPart stickPart))
            {
                if (stickPart.isDissolving)
                {
                    return;
                }
                else
                {
                    isOccupied = true;
                }
              
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

    public void SetCircleNodeHighlithStates()
    {
        if (isOccupied)
        {          
            if (rightCircleNode != null)
            {
                rightCircleNode.SetHighlightColor();
                rightCircleNode.isOccupied = true;
            }
            if (leftCircleNode != null)
            {
                leftCircleNode.SetHighlightColor();
                leftCircleNode.isOccupied = true;
            }
            if (upCircleNode != null)
            {
                upCircleNode.SetHighlightColor();
                upCircleNode.isOccupied = true;
            }
            if (downCircleNode != null)
            {
                downCircleNode.SetHighlightColor();
                downCircleNode.isOccupied = true;
            }
        }
    }

    public void CloseCircleNodeHighligth()
    {
        if (rightCircleNode != null)
        {
            rightCircleNode.SetInitialColor();
        }
        if (leftCircleNode != null)
        {
            leftCircleNode.SetInitialColor();
        }
        if (upCircleNode != null)
        {
            upCircleNode.SetInitialColor();
        }
        if (downCircleNode != null)
        {
            downCircleNode.SetInitialColor();
        }
    }
}
