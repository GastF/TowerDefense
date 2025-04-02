using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public  class TowersManager : MonoBehaviour
{   
    //*Available Towers
    public static TowersManager Instance;
    [Header("Available Towers Setup")]
    [SerializeField] private SO_Tower[] Towers;
    public static event Action<SO_Tower> TowerLoaded;
    public static event Action <int[]>TowerPurchased;
    public static event Action PlacingCanceled;
    [SerializeField] private Material _availableMat;
    [SerializeField] private Material _unavailableMat;
    private Dictionary<string,Material> OriginalMat = new();
     //* Tower Placement
    [SerializeField] private LayerMask _placeableArea;
    private GameObject _currentTower;
    private Collider _currentTowerCollider;
    private Camera _mainCamera;
    private UIInputManager input;
    
    //* Bullet Pooling
    public Dictionary<SO_Tower.ProjectileType, ObjectPool<GameObject>> ProjectilePools = new ();

    void Awake()
    {
        input = UIInputManager.Instance;
       if(Instance != null)
        {
            Destroy(Instance);
        } 
        Instance = this;
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
        if(Instance != null)
        Destroy(Instance);
    }
    void Start()
    {
        SetPool();
    }

    private void SetPool()
    {
        foreach (SO_Tower towerData in Towers)
        {
            if (!ProjectilePools.ContainsKey(towerData.TypeOfProjectile))
            {
                ProjectilePools[towerData.TypeOfProjectile] = new ObjectPool<GameObject>(
                    () => {return Spawn(towerData.ProjectilePrefab);},
                    Projectile => { Projectile.gameObject.SetActive(true);},
                    Projectile => { Projectile.gameObject.SetActive(false);},
                    Projectile => {Destroy(Projectile.gameObject);},
                    false,100,150);
            }
        }
       
    }
    public void ReturnToPool(SO_Tower.ProjectileType projectileType, GameObject projectile)
    {
        
        ProjectilePools[projectileType].Release(projectile);
    }
    private GameObject Spawn(GameObject projectilePrefab)
    {
        var projectile = Instantiate( projectilePrefab);
        projectile.gameObject.SetActive(false);
        return projectile;
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
        
        var tower = _currentTower.GetComponent<Tower>();
        SetTowerStats(tower);
        tower.DisableRangeCollider();
        tower.ShowRange();
        
        Vector3 mousePos = Input.mousePosition;
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);
        
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _placeableArea))
        {
            
            AssignMat(_unavailableMat,tower.meshes);
            return;
        }
        _currentTower.transform.position = hit.point;
        // Verificar si hay otra torre en la misma posición antes de colocarla
        float checkRadius = 1.5f; // Ajusta según el tamaño de la torre
        Collider[] colliders = Physics.OverlapSphere(hit.point, checkRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Tower")) // Asegúrate de que las torres tienen este tag
            {
               // Debug.Log("No puedes colocar la torre aquí, ya hay una en esta posición.");
                AssignMat(_unavailableMat,tower.meshes);
                return;
            }
        }

        AssignMat(_availableMat,tower.meshes);
        if (input.Click.WasPressedThisFrame())
        {
           
            // Colocar la torre si no hay otra en la misma zona
            UIManager.Instance.ShowTowers();
            tower.HideRange();
            tower.EnableRangeCollider();
            TowerPurchased?.Invoke(tower.Data.ScriptableData.Cost);
            RestoreMat(tower.meshes);
            _currentTowerCollider.enabled = true;
            _currentTower = null;
        }
        else if (input.RightClick.WasPressedThisFrame())
        {   
            RestoreMat(tower.meshes);
            Destroy(_currentTower);
            _currentTower = null;
            PlacingCanceled?.Invoke();
        }
    }
    private void SetTowerStats(Tower tower)
    {
        tower.Stats.Damage = tower.Data.ScriptableData.Damage;
        tower.Stats.Range = tower.Data.ScriptableData.Range;
        tower.Stats.Speed = tower.Data.ScriptableData.RateOfFire;
    }
    private void OnTowerSelected(SO_Tower  tower)
    {
        if (_currentTower != null) Destroy(_currentTower);
        _currentTower = Instantiate(tower.TowerPrefab,this.transform);
        _currentTower.GetComponent<Tower>().Data.ScriptableData = tower;
        _currentTowerCollider = _currentTower.GetComponent<Collider>();
        _currentTowerCollider.enabled = false;
    }
    private void AssignMat(Material mat ,MeshRenderer[] meshes)
    {
        foreach(var mesh in meshes)
        {   
            if(!OriginalMat.ContainsKey(mesh.name))
                OriginalMat.Add(mesh.name,mesh.material);
            
            mesh.material = mat;
        }
    }
    private void RestoreMat(MeshRenderer[]meshes)
    {
        foreach(var mesh in meshes)
        {
            if(OriginalMat.ContainsKey(mesh.name))
            {
                mesh.material = OriginalMat[mesh.name];
            }
        }
    }
}
