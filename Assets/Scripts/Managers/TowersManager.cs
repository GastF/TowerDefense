using System;
using UnityEngine;


public  class TowersManager : MonoBehaviour
{   
    //*Available Towers
    [Header("Available Towers Setup")]
    [SerializeField] private SO_Tower[] Towers;
    public static event Action<SO_Tower> TowerLoaded;
    public static event Action TowerPlaced;
    public static event Action PlacingCanceled;
    

     //* Tower Placement
    private GameObject _currentTower;
    private Collider _currentTowerCollider;
    private Camera _mainCamera;
    
    private UIInputManager input;

    void Awake()
    {
        input = UIInputManager.Instance;
    }
    void OnEnable()
    {   
        _mainCamera = Camera.main;
        UIManager.TowerSelected += OnTowerSelected;
        LoadTowers();
    }
    void OnDisable()
    {
        UIManager.TowerSelected -= OnTowerSelected;
    }
    private void LoadTowers()
    {
        foreach (SO_Tower towerData in Towers)
        {
    
            TowerLoaded?.Invoke(towerData);
        }
    }

    private void Update()
    {
        ManagePlacement();
    }

    private void ManagePlacement()
    {
        if (_currentTower == null) return;

        
        Vector3 mousePos = Input.mousePosition;
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.CompareTag("Tower"))
            {
                _currentTower.transform.position =  hit.point;
                return;
            }
            _currentTower.transform.position =  hit.point;
        
        }
        if (input.Click.WasPressedThisFrame())
        {
            _currentTowerCollider.enabled = true;
            _currentTower = null; 
            TowerPlaced?.Invoke();
        }
        else if (input.RightClick.WasPressedThisFrame())
        {
            
            Destroy(_currentTower);
            _currentTower = null;
            PlacingCanceled?.Invoke();
        }
        
    }
    private void OnTowerSelected(SO_Tower  tower)
    {
        if (_currentTower != null) Destroy(_currentTower);
        _currentTower = Instantiate(tower.TowerPrefab,this.transform);
        _currentTower.GetComponent<Tower>().towerData = tower;
        _currentTowerCollider = _currentTower.GetComponent<Collider>();
        _currentTowerCollider.enabled = false;
    }

}
