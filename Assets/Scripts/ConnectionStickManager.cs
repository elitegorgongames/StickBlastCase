using System.Collections.Generic;
using UnityEngine;

public class ConnectionStickManager : MonoBehaviour
{

    public static ConnectionStickManager Instance;

    public List<ConnectionStick> connectionStickList;

    public List<CompletedCircleNodeObjectImage> completedObjectsList;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateConnectionStickOccupiedStates();
        }       
    }

    public void UpdateConnectionStickHighlightStates()
    {
        foreach (var connectionStick in connectionStickList)
        {
            connectionStick.CloseCircleNodeHighligth();
        }

        foreach (var connectionStick in connectionStickList)
        {
            connectionStick.SetCircleNodeHighlithStates();
        }
    }

    public void UpdateConnectionStickOccupiedStates()
    {
        foreach (var cStick in connectionStickList)
        {
            cStick.SetOccupiedState();
        }

        CheckCompletedObjects();
    }

    public void CheckCompletedObjects()
    {
        foreach (var cObject in completedObjectsList)
        {
            if (cObject.belongedCircleNode!=null)
            {
                cObject.belongedCircleNode.completedCircleNodeObject = cObject;
                cObject.belongedCircleNode.isCompleted = true;
                cObject.belongedCircleNode.SetHighlightColor();

                var cNode = cObject.belongedCircleNode;
                var r = GridManager.Instance.FindRightNeighborOfCircleNode(cNode);
                var u = GridManager.Instance.FindUpNeighborOfCircleNode(cNode);
                var up = GridManager.Instance.FindRightUpCircleNode(cNode);

                r.SetHighlightColor();
                u.SetHighlightColor();
                up.SetHighlightColor();
            }
        }
    }

    public void AddToConnectionStickList(ConnectionStick connectionStick)
    {
        connectionStickList.Add(connectionStick);
    }
}
