using UnityEngine;

public class ConnectionStick : MonoBehaviour
{

    public bool isOccupied;

    public Color hightlightColor;
    public Color initialColor;
    public SpriteRenderer spriteRenderer;

    public GameObject highlightObject;
    public GameObject defaultStickObject;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;

        initialColor = spriteRenderer.color;
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
