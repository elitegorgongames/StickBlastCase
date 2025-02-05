using System.Collections.Generic;
using System.Linq;
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
    public List<ConnectionStick> connectionSticksToPlaceList;
    public List<CircleNode> highlitghedCicrleNodesList;
    public float minDistanceToPlaceGrid;
    public bool enableToPlace;


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
        int orderCounter = 0;
        for (int row = 0; row < _rowCount; row++)
        {
            for (int col = 0; col < _columnCount; col++)
            {
                Vector3 spawnPosition = new Vector3(col * _spacing, row * _spacing, 0);
                var circleNode = Instantiate(_circleObject, spawnPosition, Quaternion.identity, transform);
                circleNode.coordinate = new Vector2Int(row, col);
                circleNode.SetOffset();
                circleNode.circleNodeOrder = orderCounter;
                circleNodeList.Add(circleNode);
                orderCounter++;
            }
        }
    }

    void GenerateBars()
    {
        for (int i = 0; i < circleNodeList.Count; i++)
        { 
            var circleNodePos = circleNodeList[i].GetTransform().position;
            circleNodePos.z = -0.1f;

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

    public void FindClosestCircleNodeToSelectedStick(Stick stick)
    {
        enableToPlace = false;
        var closestDistance = minDistanceToPlaceGrid;
        currentClosestCircleNodeToSelectedStick = null;
        connectionSticksToPlaceList.Clear();
        highlitghedCicrleNodesList.Clear();

        var currentPickedStick = InputController.Instance.GetCurrentSelectedStick();

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
        if (currentClosestCircleNodeToSelectedStick==null)
        {
            CloseHighlightOfAllConnectionSticksAndCircleNodes();
            return;
        }

        if (!IsStickFitIntoTheCircleNode(currentPickedStick, currentClosestCircleNodeToSelectedStick))
        {
            return;
        }

        if (currentClosestCircleNodeToSelectedStick!=null)
        {
            if (stick.stickType == StickType.Vertical)
            {
                if (currentClosestCircleNodeToSelectedStick.upConnectionStick!=null)
                {         
                    currentClosestCircleNodeToSelectedStick.SetHighlightColor();
                    var upNeighborCircleNode = FindUpNeighborOfCircleNode(currentClosestCircleNodeToSelectedStick);
                    upNeighborCircleNode.SetHighlightColor();

                    highlitghedCicrleNodesList.Add(currentClosestCircleNodeToSelectedStick);
                    highlitghedCicrleNodesList.Add(upNeighborCircleNode);

                    currentClosestCircleNodeToSelectedStick.upConnectionStick.SetHighlightColor();

                    connectionSticksToPlaceList.Add(currentClosestCircleNodeToSelectedStick.upConnectionStick);

                    enableToPlace = true;              
                }                
            }
            if (stick.stickType == StickType.Horizontal)
            {
                if (currentClosestCircleNodeToSelectedStick.rightConnectionStick != null)
                {
                    currentClosestCircleNodeToSelectedStick.SetHighlightColor();
                    var rightNeighborCircleNode = FindRightNeighborOfCircleNode(currentClosestCircleNodeToSelectedStick);
                    rightNeighborCircleNode.SetHighlightColor();

                    highlitghedCicrleNodesList.Add(currentClosestCircleNodeToSelectedStick);
                    highlitghedCicrleNodesList.Add(rightNeighborCircleNode);

                    currentClosestCircleNodeToSelectedStick.rightConnectionStick.SetHighlightColor();

                    connectionSticksToPlaceList.Add(currentClosestCircleNodeToSelectedStick.rightConnectionStick);

                    enableToPlace = true;
                }
            }
            if (stick.stickType == StickType.LType)
            {
                if (currentClosestCircleNodeToSelectedStick.rightConnectionStick != null && currentClosestCircleNodeToSelectedStick.upConnectionStick != null)
                {
                    currentClosestCircleNodeToSelectedStick.SetHighlightColor();
                    var upNeighborCircleNode = FindUpNeighborOfCircleNode(currentClosestCircleNodeToSelectedStick);
                    upNeighborCircleNode.SetHighlightColor();
                    var rightNeighborCircleNode = FindRightNeighborOfCircleNode(currentClosestCircleNodeToSelectedStick);
                    rightNeighborCircleNode.SetHighlightColor();

                    highlitghedCicrleNodesList.Add(currentClosestCircleNodeToSelectedStick);
                    highlitghedCicrleNodesList.Add(upNeighborCircleNode);
                    highlitghedCicrleNodesList.Add(rightNeighborCircleNode);

                    currentClosestCircleNodeToSelectedStick.rightConnectionStick.SetHighlightColor();
                    currentClosestCircleNodeToSelectedStick.upConnectionStick.SetHighlightColor();

                    connectionSticksToPlaceList.Add(currentClosestCircleNodeToSelectedStick.rightConnectionStick);
                    connectionSticksToPlaceList.Add(currentClosestCircleNodeToSelectedStick.upConnectionStick);

                    enableToPlace = true;
                }
            }
            if (stick.stickType == StickType.UType)
            {
                if (currentClosestCircleNodeToSelectedStick.rightConnectionStick != null && currentClosestCircleNodeToSelectedStick.upConnectionStick != null)
                {
                    var rightNeighborCircleNode = FindRightNeighborOfCircleNode(currentClosestCircleNodeToSelectedStick);
                    var upNeighborCircleNode = FindUpNeighborOfCircleNode(currentClosestCircleNodeToSelectedStick);
                    var rightUpNeighborCircleNode = FindRightUpCircleNode(currentClosestCircleNodeToSelectedStick);

                    if (rightNeighborCircleNode != null && upNeighborCircleNode!=null)
                    {
                        currentClosestCircleNodeToSelectedStick.SetHighlightColor();
                        rightNeighborCircleNode.SetHighlightColor();
                        upNeighborCircleNode.SetHighlightColor();
                        rightUpNeighborCircleNode.SetHighlightColor();

                        highlitghedCicrleNodesList.Add(currentClosestCircleNodeToSelectedStick);
                        highlitghedCicrleNodesList.Add(upNeighborCircleNode);
                        highlitghedCicrleNodesList.Add(rightNeighborCircleNode);
                        highlitghedCicrleNodesList.Add(rightUpNeighborCircleNode);


                        rightNeighborCircleNode.upConnectionStick.SetHighlightColor();
                        currentClosestCircleNodeToSelectedStick.rightConnectionStick.SetHighlightColor();
                        currentClosestCircleNodeToSelectedStick.upConnectionStick.SetHighlightColor();

                        connectionSticksToPlaceList.Add(rightNeighborCircleNode.upConnectionStick);
                        connectionSticksToPlaceList.Add(currentClosestCircleNodeToSelectedStick.rightConnectionStick);
                        connectionSticksToPlaceList.Add(currentClosestCircleNodeToSelectedStick.upConnectionStick);

                        enableToPlace = true;
                    }
                }
            }
        }
    }

    private bool IsStickFitIntoTheCircleNode(Stick stick, CircleNode closestCircleNode)
    {
        var stickType = stick.stickType;

        if (stickType == StickType.Vertical)
        {
            if (closestCircleNode.upConnectionStick==null)
            {
                return false;
            }
            if (closestCircleNode.upConnectionStick.isOccupied)
            {
                return false;
            }
        }
        if (stickType == StickType.Horizontal)
        {
            if (closestCircleNode.rightConnectionStick == null)
            {
                return false;
            }
            if (closestCircleNode.rightConnectionStick.isOccupied)
            {
                return false;
            }
        }
        if (stickType == StickType.LType)
        {
            if (closestCircleNode.rightConnectionStick == null || closestCircleNode.upConnectionStick == null)
            {
                return false;
            }
            if (closestCircleNode.rightConnectionStick.isOccupied || closestCircleNode.upConnectionStick.isOccupied)
            {
                return false;
            }
        }
        if (stickType == StickType.UType)
        {
            var rightNeighborOfClosestCircleNode = FindRightNeighborOfCircleNode(closestCircleNode);
            if (closestCircleNode.rightConnectionStick == null || closestCircleNode.upConnectionStick == null)
            {
                return false;
            }
            if (closestCircleNode.rightConnectionStick.isOccupied || closestCircleNode.upConnectionStick.isOccupied || rightNeighborOfClosestCircleNode.upConnectionStick.isOccupied)
            {
                return false;
            }
        }

        return true;   
    }

    private void CloseHighlightOfAllConnectionSticksAndCircleNodes()
    {
        foreach (var cNode in circleNodeList)
        {
            if (cNode.isOccupied)
            {
                continue;
            }

            cNode.SetInitialColor();
            if (cNode.rightConnectionStick!=null)
            {
                cNode.rightConnectionStick.SetInitialColor();
            }
            if (cNode.upConnectionStick!=null)
            {
                cNode.upConnectionStick.SetInitialColor();
            }
        }
    }


    public void SetConnectionSticksOccupied(out CircleNode referenceCircleNode, out List<CircleNode> highlightedCircleNodeList)
    { 
        var currentSelectedStick = InputController.Instance.GetCurrentSelectedStick();
        referenceCircleNode = null;
        highlightedCircleNodeList = new List<CircleNode>();

        if (currentClosestCircleNodeToSelectedStick == null)
        {
            return;
        }

        if (currentSelectedStick.stickType == StickType.Vertical)
        {
            if (currentClosestCircleNodeToSelectedStick.upConnectionStick == null)
            {
                return;
            }
            if (currentClosestCircleNodeToSelectedStick.upConnectionStick.isOccupied)
            {                
                return;
            }
        }
        if (currentSelectedStick.stickType == StickType.Horizontal)
        {
            if (currentClosestCircleNodeToSelectedStick.rightConnectionStick==null)
            {
                return;
            }
            if (currentClosestCircleNodeToSelectedStick.rightConnectionStick.isOccupied)
            {
                return;
            }
        }
        if (currentSelectedStick.stickType == StickType.LType)
        {
            if (currentClosestCircleNodeToSelectedStick.upConnectionStick==null || currentClosestCircleNodeToSelectedStick.rightConnectionStick==null)
            {
                return;
            }
            if (currentClosestCircleNodeToSelectedStick.rightConnectionStick.isOccupied || currentClosestCircleNodeToSelectedStick.upConnectionStick.isOccupied)
            {
                return;
            }
        }
        if (currentSelectedStick.stickType == StickType.UType)
        {
            if (currentClosestCircleNodeToSelectedStick.upConnectionStick == null || currentClosestCircleNodeToSelectedStick.rightConnectionStick == null)
            {
                return;
            }
            var rightNeighborCircleNode = FindRightNeighborOfCircleNode(currentClosestCircleNodeToSelectedStick);
            if (currentClosestCircleNodeToSelectedStick.rightConnectionStick.isOccupied || 
                currentClosestCircleNodeToSelectedStick.upConnectionStick.isOccupied ||
                rightNeighborCircleNode.upConnectionStick.isOccupied)
            {
                return;
            }
        }


        if (connectionSticksToPlaceList.Count==0)
        {
            referenceCircleNode = null;
            return;
        }

        for (int i = 0; i < connectionSticksToPlaceList.Count; i++)
        {
            connectionSticksToPlaceList[i].isOccupied = true;
        }
        for (int i = 0; i < highlitghedCicrleNodesList.Count; i++)
        {
            highlightedCircleNodeList.Add(highlitghedCicrleNodesList[i]);
        }

        referenceCircleNode = currentClosestCircleNodeToSelectedStick;
    }

    private CircleNode FindRightNeighborOfCircleNode(CircleNode circleNode)
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

    private CircleNode FindUpNeighborOfCircleNode(CircleNode circleNode)
    {
        var upNeighborCircleNodeCoordinates = new Vector2Int(circleNode.coordinate.x+1, circleNode.coordinate.y);
        CircleNode upNeighbor = null;
        if (upNeighborCircleNodeCoordinates.y < _columnCount)
        {
            foreach (var cNode in circleNodeList)
            {
                if (cNode.coordinate.x == upNeighborCircleNodeCoordinates.x && cNode.coordinate.y == upNeighborCircleNodeCoordinates.y)
                {
                    upNeighbor = cNode;
                    break;
                }
            }
        }

        return upNeighbor;
    }

    private CircleNode FindRightUpCircleNode(CircleNode circleNode)
    {
        var rightUpNeighborCircleNodeCoordinates = new Vector2Int(circleNode.coordinate.x + 1, circleNode.coordinate.y+1);
        CircleNode rightUpNeighbor = null;
        if (rightUpNeighborCircleNodeCoordinates.y < _columnCount && rightUpNeighborCircleNodeCoordinates.x<_rowCount)
        {
            foreach (var cNode in circleNodeList)
            {
                if (cNode.coordinate.x == rightUpNeighborCircleNodeCoordinates.x && cNode.coordinate.y == rightUpNeighborCircleNodeCoordinates.y)
                {
                    rightUpNeighbor = cNode;
                    break;
                }
            }
        }

        return rightUpNeighbor;
    }

    public void CheckIfAnyCircleNodeIsCompleted()
    {
        var completedCircleNodeOrdersList = new List<int>();

        for (int i = 0; i < circleNodeList.Count; i++)
        {
            bool circleNodesAreOccupied = false;
            bool sticksAreOccupied = false;
            var circleOrder = i;
            if (circleNodeList[i].isOccupied)
            {
                var rightNeighborCircleNode = FindRightNeighborOfCircleNode(circleNodeList[i]);
                var upNeighborCircleNode = FindUpNeighborOfCircleNode(circleNodeList[i]);
                var rightUpNeighborCircleNode = FindRightUpCircleNode(circleNodeList[i]);
            

                if (rightNeighborCircleNode==null || upNeighborCircleNode==null || rightUpNeighborCircleNode==null)
                {
                    continue;
                }
                if (rightNeighborCircleNode.isOccupied && upNeighborCircleNode.isOccupied && rightUpNeighborCircleNode.isOccupied)
                {                    
                    circleNodesAreOccupied = true;                 
                }

                var rightConnectionStick = circleNodeList[i].rightConnectionStick;
                var upConnectionStick = circleNodeList[i].upConnectionStick;
                var rightRightConnectionStick = rightNeighborCircleNode.upConnectionStick;
                var upUpConnectionStick = upNeighborCircleNode.rightConnectionStick;

                if (rightConnectionStick.isOccupied && upConnectionStick.isOccupied && rightRightConnectionStick.isOccupied && upUpConnectionStick.isOccupied)
                {
                    sticksAreOccupied = true;             
                }
            }

            if (circleNodesAreOccupied && sticksAreOccupied)
            {
                Debug.Log("this circle node is completed " + circleOrder);
                completedCircleNodeOrdersList.Add(circleOrder);
                
            }
        }



        foreach (var item in completedCircleNodeOrdersList)
        {
            Debug.Log("completed list items " + item);
        }
        var rowListsToCheck = new List<List<int>>();
        var columnListsToCheck = new List<List<int>>();

        for (int i = 0; i < _rowCount; i++)
        {
            var rowNumbers = new List<int>();
            for (int j = 0; j < _columnCount - 1; j++)
            {
                rowNumbers.Add(i * _columnCount + j);
            }
            rowListsToCheck.Add(rowNumbers);
        }


        for (int j = 0; j < _columnCount; j++)
        {
            var colNumbers = new List<int>();
            for (int i = 0; i < _rowCount - 1; i++)
            {
                colNumbers.Add(i * _columnCount + j);
            }
            columnListsToCheck.Add(colNumbers);
        }

        for (int i = 0; i < rowListsToCheck.Count; i++)
        {
            var allOfList1IsInList2 = rowListsToCheck[i].Intersect(completedCircleNodeOrdersList).Count() == rowListsToCheck[i].Count();
            if (allOfList1IsInList2)
            {
                Debug.Log("contains here row");

                foreach (var order in rowListsToCheck[i])
                {
                    var cNode = GetCircleNodeByOrder(order);
                    Destroy(cNode.rightConnectionStick.gameObject);
                    Destroy(cNode.upConnectionStick.gameObject);
                }
            }
        }

        for (int i = 0; i < columnListsToCheck.Count; i++)
        {
            var allOfList1IsInList2 = columnListsToCheck[i].Intersect(completedCircleNodeOrdersList).Count() == columnListsToCheck[i].Count();
            if (allOfList1IsInList2)
            {
                Debug.Log("contains here column");
            }
        }  
    }
  
    public List<List<int>> CheckIfAnyLineIsCompleted(List<int> completedCircleOrderList)
    {
        var rowListsToCheck = new List<List<int>>();
        var columnListsToCheck = new List<List<int>>();
        var matchedLists = new List<List<int>>(); 

    
        for (int i = 0; i < _rowCount; i++)
        {
            var rowNumbers = new List<int>();
            for (int j = 0; j < _columnCount - 1; j++)
            {
                rowNumbers.Add(i * _columnCount + j);
            }
            rowListsToCheck.Add(rowNumbers);
        }

  
        for (int j = 0; j < _columnCount; j++)
        {
            var colNumbers = new List<int>();
            for (int i = 0; i < _rowCount - 1; i++)
            {
                colNumbers.Add(i * _columnCount + j);
            }
            columnListsToCheck.Add(colNumbers);
        }

        matchedLists.AddRange(rowListsToCheck.Where(row => row.OrderBy(x => x).SequenceEqual(completedCircleOrderList.OrderBy(x => x))));
        matchedLists.AddRange(columnListsToCheck.Where(col => col.OrderBy(x => x).SequenceEqual(completedCircleOrderList.OrderBy(x => x))));

        if (matchedLists.Count>0)
        {
            Debug.Log("list is here dude");
        }
        return matchedLists; 
    }



    private CircleNode GetCircleNodeByOrder(int order)
    {
        foreach (var cNode in circleNodeList)
        {
            if (cNode.circleNodeOrder == order)
            {
                return cNode;
            }
        }

        Debug.LogWarning("no circle node with this order");
        return null;
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
