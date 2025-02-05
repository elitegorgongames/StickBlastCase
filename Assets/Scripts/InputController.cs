using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;


    [SerializeField] private Stick _currentSelectedStick;
    [SerializeField] private float _movementOffset = 1.0f; // �ubu�un yukar� kayma miktar�


    private Camera _mainCamera;
    private Vector3 _initialMousePosition;
    private Vector3 _initialStickPosition;
    private bool _isDragging = false;

    

    private void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol t�klama -> �ubu�u se�
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.TryGetComponent(out Stick stick))
                {
                    if (!stick.isPicked)
                    {
                        stick.isPicked = true;
                        _currentSelectedStick = stick;
                        _initialMousePosition = Input.mousePosition; // Mouse'un ba�lang�� pozisyonunu kaydet
                        _initialStickPosition = _currentSelectedStick.transform.position; // �ubu�un ba�lang�� pozisyonunu kaydet
                        _isDragging = true;
                    }
                }
            }
        }

        if (_isDragging && Input.GetMouseButton(0) && _currentSelectedStick != null) // Mouse bas�l�yken -> Hareket ettir
        {
            MoveSelectedStick();
            GridManager.Instance.FindClosestCircleNodeToSelectedStick(_currentSelectedStick);
        }

        if (Input.GetMouseButtonUp(0) && _currentSelectedStick != null) // Mouse b�rak�ld���nda -> �ubu�u serbest b�rak
        {
            GridManager.Instance.SetConnectionSticksOccupied();
            _currentSelectedStick.isPicked = false;
            _currentSelectedStick = null;
            _isDragging = false;
            GridManager.Instance.enableToPlace = false;
        }
    }

    void MoveSelectedStick()
    {
        if (_currentSelectedStick == null) return;

        Vector3 mouseDelta = Input.mousePosition - _initialMousePosition; // Mouse'un hareket miktar�n� hesapla

        // Mouse hareketini d�nya koordinatlar�na �evir
        Vector3 worldDelta = _mainCamera.ScreenToWorldPoint(new Vector3(mouseDelta.x, mouseDelta.y, 10f))
                           - _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 10f));

        // Yeni pozisyon: X ve Y ekseninde hareket etsin, Z ekseni sabit kals�n
        Vector3 newPosition = _initialStickPosition + new Vector3(worldDelta.x, worldDelta.y + _movementOffset, 0);
        _currentSelectedStick.transform.position = newPosition;
    }   
}
