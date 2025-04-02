using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    public static UIInputManager Instance; 
    [SerializeField] private InputActionReference _click;
    [SerializeField] private InputActionReference _rightClick;
   
    private InputActionMap UIActionMap;

    
    [HideInInspector] public  InputAction Click;
    [HideInInspector] public  InputAction RightClick;
    
    
    void Awake()
    {  
       if(Instance != null)
        {
            Destroy(Instance);
        } 
        Instance = this;

        _playerInput = GetComponentInParent<PlayerInput>();
        UIActionMap = _playerInput.actions.FindActionMap("UI");

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
        UIActionMap.Disable();
        Click.Disable();
        RightClick.Disable();
        
    }

    private void EnableCameraInputs()
    {
        UIActionMap.Enable();

        Click = _click.action;
        RightClick = _rightClick.action;
       
        Click.Enable();
        RightClick.Enable();
    
    }
}
