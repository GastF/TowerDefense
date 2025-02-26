using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputManager : MonoBehaviour
{   private PlayerInput _playerInput;
    [SerializeField] private InputActionReference _movementInput;
    [SerializeField] private InputActionReference _rotateLeftInput;
    [SerializeField] private InputActionReference _rotateRightInput;
    [SerializeField] private InputActionReference _zoomInput;
    private InputActionMap CameraActionMap;
    public static InputAction Move;
    public static InputAction RotateLeft;
    public static InputAction RotateRight;
    public static InputAction Zoom;
    
    void Awake()
    {
       EnableCameraInputs();
    }
    void OnDisable()
    {
        DisableCameraInputs();
    }

    private void DisableCameraInputs()
    {
        CameraActionMap.Disable();
        Move.Disable();
        RotateLeft.Disable();
        RotateRight.Disable();
        Zoom.Disable();
    }

    private void EnableCameraInputs()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        CameraActionMap = _playerInput.actions.FindActionMap("Camera Movement");
        
        Move = _movementInput.action;
        Zoom = _zoomInput.action;
        RotateLeft = _rotateLeftInput.action;
        RotateRight = _rotateRightInput.action;
        
        CameraActionMap.Enable();
        Move.Enable();
        RotateLeft.Enable();
        RotateRight.Enable();
        Zoom.Enable();
    }
}
