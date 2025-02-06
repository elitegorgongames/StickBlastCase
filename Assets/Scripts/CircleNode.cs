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
    public int circleNodeOrder;

    public CompletedCircleNodeObjectImage completedCircleNodeObjectImagePrefab;
    public CompletedCircleNodeObjectImage completedCircleNodeObject;
    public Transform completedCircleNodeObjectPrefabSpawnTransform;

    public bool isCompleted;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
        initialColor = spriteRenderer.color;
    }

    public void SpawnCompletedObject()
    {
        var completedObject = Instantiate(completedCircleNodeObjectImagePrefab, _transform);
        completedObject.transform.position = completedCircleNodeObjectPrefabSpawnTransform.position;  
        completedCircleNodeObject = completedObject;
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
