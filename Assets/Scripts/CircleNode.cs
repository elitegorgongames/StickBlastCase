using UnityEngine;
using System;

[Serializable]
public class CircleNode : MonoBehaviour
{
    public Vector2Int coordinate;

    public ConnectionStick rightConnectionStick;    
    public ConnectionStick upConnectionStick;

    public SpriteRenderer spriteRenderer;
    public Color initialColor;
    public Color hightligthColor;
    public bool isHighlited;
    public bool isOccupied;

    public float offsetZ;

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

    public void SetOffset()
    {
        var pos = _transform.position;
        pos.z = offsetZ;
        _transform.position = pos;
    }

    public void SetHighlightColor()
    {
        spriteRenderer.color = hightligthColor;
    }

    public void SetInitialColor()
    {
        spriteRenderer.color = initialColor;
    }
}
