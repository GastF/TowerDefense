using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputManager : MonoBehaviour
{   private PlayerInput _playerInput;
    public static CameraInputManager Instance; 
    [SerializeField] private InputActionReference _movement;
    [SerializeField] private InputActionReference _rotate;
    [SerializeField] private InputActionReference _zoom;
    private InputActionMap CameraActionMap;

    
    [HideInInspector] public  InputAction Move;
    [HideInInspector] public  InputAction Rotate;
    [HideInInspector] public  InputAction Zoom;
    
    void Awake()
    {  
        if(Instance != null)
        {
            Destroy(Instance);
        } 
        Instance = this;

        _playerInput = GetComponentInParent<PlayerInput>();
        CameraActionMap = _playerInput.actions.FindActionMap("Camera");

        EnableCameraInputs();
    }
    void OnDisable()
    {
        DisableCameraInputs();
        if(Instance != null)
        Destroy(Instance);
    }

    private void DisableCameraInputs()
    {
        CameraActionMap.Disable();
        Move.Disable();
        Rotate.Disable();
        Zoom.Disable();
    }

    private void EnableCameraInputs()
    {
        CameraActionMap.Enable();

        Move = _movement.action;
        Zoom = _zoom.action;
        Rotate = _rotate.action;
       
        Move.Enable();
        Rotate.Enable();
        Zoom.Enable();
    }
}
