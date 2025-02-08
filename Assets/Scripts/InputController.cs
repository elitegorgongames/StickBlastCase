using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;


    [SerializeField] private Stick _currentSelectedStick;
    [SerializeField] private float _movementOffset = 1.0f; // Çubuðun yukarý kayma miktarý


    private Camera _mainCamera;
    private Vector3 _initialMousePosition;
    private Vector3 _initialStickPosition;
    private bool _isDragging = false;


    public bool gameIsOn;


    private void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main;
        gameIsOn = true;
    }

    void Update()
    {
        if (!gameIsOn)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.TryGetComponent(out Stick stick))
                {
                    if (stick.isPlaced)
                    {
                        return;
                    }
                    if (!stick.isPickable)
                    {
                        return;
                    }

                    if (!stick.isPicked)
                    {
                        stick.isPicked = true;
                        _currentSelectedStick = stick;
                        _initialMousePosition = Input.mousePosition; 
                        _initialStickPosition = _currentSelectedStick.transform.position; 
                        _isDragging = true;

                        Vector3 mouseDelta = Input.mousePosition - _initialMousePosition;

                        Vector3 worldDelta = _mainCamera.ScreenToWorldPoint(new Vector3(mouseDelta.x, mouseDelta.y, 10f))
                                           - _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 10f));
                        Vector3 newPosition = _initialStickPosition + new Vector3(worldDelta.x, worldDelta.y + _movementOffset, 0);
                        _currentSelectedStick.FirstPickMovement(newPosition);
                    }
                }
            }
        }

        if (_isDragging && Input.GetMouseButton(0) && _currentSelectedStick != null) 
        {
            MoveSelectedStick();
            GridManager.Instance.FindClosestCircleNodeToSelectedStick(_currentSelectedStick);
        }

        if (Input.GetMouseButtonUp(0) && _currentSelectedStick != null) 
        {
            GridManager.Instance.SetConnectionSticksOccupied(out CircleNode referenceCircleNode, out List<CircleNode> highlightedCircleNodeList);
            if (referenceCircleNode!=null)
            {
                _currentSelectedStick.PlaceToGrid(referenceCircleNode.GetTransform().position);
                foreach (var cNode in highlightedCircleNodeList)
                {
                    cNode.isOccupied = true;
                }
            }
            else
            {
                _currentSelectedStick.BackToStartPoint();
            }

            _currentSelectedStick.isPicked = false;
            _currentSelectedStick = null;
            _isDragging = false;
            GridManager.Instance.enableToPlace = false;      
        }
    }

    void MoveSelectedStick()
    {
        if (_currentSelectedStick.firstPick)
        {
            return;
        }

        if (_currentSelectedStick == null) return;

        Vector3 mouseDelta = Input.mousePosition - _initialMousePosition; 
       
        Vector3 worldDelta = _mainCamera.ScreenToWorldPoint(new Vector3(mouseDelta.x, mouseDelta.y, 10f))
                           - _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 10f));
       
        Vector3 newPosition = _initialStickPosition + new Vector3(worldDelta.x, worldDelta.y + _movementOffset, 0);
        _currentSelectedStick.transform.position = newPosition;
    }   

    public Stick GetCurrentSelectedStick()
    {
        return _currentSelectedStick;
    }
}
