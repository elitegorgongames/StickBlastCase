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

    

    private void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol týklama -> Çubuðu seç
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
                        _initialMousePosition = Input.mousePosition; // Mouse'un baþlangýç pozisyonunu kaydet
                        _initialStickPosition = _currentSelectedStick.transform.position; // Çubuðun baþlangýç pozisyonunu kaydet
                        _isDragging = true;
                    }
                }
            }
        }

        if (_isDragging && Input.GetMouseButton(0) && _currentSelectedStick != null) // Mouse basýlýyken -> Hareket ettir
        {
            MoveSelectedStick();
            GridManager.Instance.FindClosestCircleNodeToSelectedStick(_currentSelectedStick);
        }

        if (Input.GetMouseButtonUp(0) && _currentSelectedStick != null) // Mouse býrakýldýðýnda -> Çubuðu serbest býrak
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

        Vector3 mouseDelta = Input.mousePosition - _initialMousePosition; // Mouse'un hareket miktarýný hesapla

        // Mouse hareketini dünya koordinatlarýna çevir
        Vector3 worldDelta = _mainCamera.ScreenToWorldPoint(new Vector3(mouseDelta.x, mouseDelta.y, 10f))
                           - _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 10f));

        // Yeni pozisyon: X ve Y ekseninde hareket etsin, Z ekseni sabit kalsýn
        Vector3 newPosition = _initialStickPosition + new Vector3(worldDelta.x, worldDelta.y + _movementOffset, 0);
        _currentSelectedStick.transform.position = newPosition;
    }   
}
