using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private CircleNode _circleObject;
    [SerializeField] private ConnectionStick _connectionStick; 
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private float _spacing = 1.5f;

    public List<CircleNode> circleNodeList;
    public CircleNode currentClosestCircleNodeToSelectedStick;
    public float minDistanceToPlaceGrid;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
        GenerateBars();
        CenterCamera();
    }

    void GenerateGrid()
    {
        for (int row = 0; row < _rowCount; row++)
        {
            for (int col = 0; col < _columnCount; col++)
            {
                Vector3 spawnPosition = new Vector3(col * _spacing, row * _spacing, 0);
                var circleNode = Instantiate(_circleObject, spawnPosition, Quaternion.identity, transform);
                circleNode.coordinate = new Vector2Int(row, col);
                circleNode.SetOffset();
                circleNodeList.Add(circleNode);
            }
        }
    }

    void GenerateBars()
    {
        for (int i = 0; i < circleNodeList.Count; i++)
        { 
            var circleNodePos = circleNodeList[i].GetTransform().position;
            circleNodePos.z = 0;

            if (circleNodeList[i].coordinate.x == _rowCount - 1 && circleNodeList[i].coordinate.y == _columnCount - 1)
            {
                continue;          
            }
            else if (circleNodeList[i].coordinate.y == _columnCount - 1)
            {
                var upLookingStick = Instantiate(_connectionStick, circleNodePos, Quaternion.identity, transform);
                upLookingStick.GetTransform().localScale = Vector3.one;

                circleNodeList[i].upConnectionStick = upLookingStick;
            }
            else if (circleNodeList[i].coordinate.x == _rowCount - 1)
            {
                var rightLookingStick = Instantiate(_connectionStick, circleNodePos, Quaternion.Euler(new Vector3(0, 0, -90)), transform);
                rightLookingStick.GetTransform().localScale = Vector3.one;

                circleNodeList[i].rightConnectionStick = rightLookingStick;
            }
            else
            {      
                var upLookingStick = Instantiate(_connectionStick, circleNodePos, Quaternion.identity, transform);
                var rightLookingStick = Instantiate(_connectionStick, circleNodePos, Quaternion.Euler(new Vector3(0, 0, -90)), transform);

                upLookingStick.GetTransform().localScale = Vector3.one;
                rightLookingStick.GetTransform().localScale = Vector3.one;

                circleNodeList[i].upConnectionStick = upLookingStick;
                circleNodeList[i].rightConnectionStick = rightLookingStick;
            }
        }
    }

    void AssignConnectionSticksToCircleNodes()
    {

    }

    public void FindClosestCircleNodeToSelectedStick(Stick stick)
    {
        var closestDistance = minDistanceToPlaceGrid;

        for (int i = 0; i < circleNodeList.Count; i++)
        {
            if (Vector3.Distance(circleNodeList[i].GetTransform().position,stick.GetCalculationTransformStartPoint().position)<closestDistance)
            {
                closestDistance = Vector3.Distance(circleNodeList[i].GetTransform().position, stick.GetCalculationTransformStartPoint().position);
                currentClosestCircleNodeToSelectedStick = circleNodeList[i];
            }
        }

        for (int i = 0; i < circleNodeList.Count; i++)
        {
            if (circleNodeList[i].upConnectionStick != null)
            {
                circleNodeList[i].upConnectionStick.SetInitialColor();
            }
            if (circleNodeList[i].rightConnectionStick != null)
            {
                circleNodeList[i].rightConnectionStick.SetInitialColor();
            }     
        }

        if (currentClosestCircleNodeToSelectedStick!=null)
        {
            if (stick.stickType == StickType.Vertical)
            {
                if (currentClosestCircleNodeToSelectedStick.upConnectionStick!=null)
                {
                    currentClosestCircleNodeToSelectedStick.upConnectionStick.SetHighlightColor();            
                }                
            }
            if (stick.stickType == StickType.Horizontal)
            {
                if (currentClosestCircleNodeToSelectedStick.rightConnectionStick != null)
                {
                    currentClosestCircleNodeToSelectedStick.rightConnectionStick.SetHighlightColor();
                }
            }
            if (stick.stickType == StickType.LType)
            {
                if (currentClosestCircleNodeToSelectedStick.rightConnectionStick != null && currentClosestCircleNodeToSelectedStick.upConnectionStick != null)
                {
                    currentClosestCircleNodeToSelectedStick.rightConnectionStick.SetHighlightColor();
                    currentClosestCircleNodeToSelectedStick.upConnectionStick.SetHighlightColor();
                }
            }
            if (stick.stickType == StickType.UType)
            {
                if (currentClosestCircleNodeToSelectedStick.rightConnectionStick != null && currentClosestCircleNodeToSelectedStick.upConnectionStick != null)
                {
                    var rightNeighbor = GetRightNeighborOfCircleNode(currentClosestCircleNodeToSelectedStick);
                    if (rightNeighbor!=null)
                    {
                        rightNeighbor.upConnectionStick.SetHighlightColor();
                        currentClosestCircleNodeToSelectedStick.rightConnectionStick.SetHighlightColor();
                        currentClosestCircleNodeToSelectedStick.upConnectionStick.SetHighlightColor();
                    }
                }
            }
        }
    }

    private CircleNode GetRightNeighborOfCircleNode(CircleNode circleNode)
    {
        var rightNeighborCircleCoordinates = new Vector2Int(circleNode.coordinate.x, circleNode.coordinate.y+1);
        CircleNode rightNeighbor = null;
        if (rightNeighborCircleCoordinates.x < _rowCount)
        {           
            foreach (var cNode in circleNodeList)
            {
                if (cNode.coordinate.x == rightNeighborCircleCoordinates.x && cNode.coordinate.y == rightNeighborCircleCoordinates.y)
                {
                    rightNeighbor = cNode;
                    break;
                }
            }
        }

        return rightNeighbor;
    }

    void CenterCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float gridWidth = (_columnCount - 1) * _spacing;
            float gridHeight = (_rowCount - 1) * _spacing;
            Vector3 gridCenter = new Vector3(gridWidth / 2, gridHeight / 2, -10);
            mainCamera.transform.position = gridCenter;
        }
    }
}
