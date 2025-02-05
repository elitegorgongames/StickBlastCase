using UnityEngine;

public class ConnectionStick : MonoBehaviour
{

    public bool isOccupied;

    public Color hightlightColor;
    public Color initialColor;
    public SpriteRenderer spriteRenderer;

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
    }

    public void SetInitialColor()
    {
        spriteRenderer.color = initialColor;
    }
}
