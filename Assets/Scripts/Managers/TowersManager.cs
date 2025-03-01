using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private Camera _mainCamera;
    

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
            //*Tower SETUP
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
        mousePos.z = 10f;
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);
        _currentTower.transform.position = Physics.Raycast(ray, out RaycastHit hit) ? hit.point : _mainCamera.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            _currentTower = null; 
            TowerPlaced?.Invoke();
        }
        else if (Input.GetMouseButtonDown(1))
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
    }

}
