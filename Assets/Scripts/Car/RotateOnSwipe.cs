using UnityEngine;
using UnityEngine.EventSystems;

public class RotateOnSwipe : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 0.15f;
    [SerializeField] private float smoothTime = 0.08f;
    private UIController _uiController;

    private float _targetYRotation;
    private float _currentVelocity;

    private Vector2 _lastTouchPosition;
    private bool _isDragging;

    void Start()
    {
        _targetYRotation = transform.eulerAngles.y;
        _uiController = FindObjectOfType<UIController>();
    }

    void Update()
    {
        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);

        // Prevent UI touches
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (_uiController.isUIInteracting) return;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                _isDragging = true;
                _lastTouchPosition = touch.position;
                break;

            case TouchPhase.Moved:
                if (!_isDragging)
                    return;

                float deltaX = touch.position.x - _lastTouchPosition.x;
                _targetYRotation += -deltaX * rotationSpeed;
                _lastTouchPosition = touch.position;
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                _isDragging = false;
                break;
        }
        
        if (!_isDragging)
            _targetYRotation = transform.eulerAngles.y;

        float smoothY = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            _targetYRotation,
            ref _currentVelocity,
            smoothTime
        );

        transform.rotation = Quaternion.Euler(0f, smoothY, 0f);
    }
}