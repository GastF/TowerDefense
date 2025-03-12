using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int InitialResources = 10;
    public int InitialHP = 10;

    [HideInInspector] public int Stone;
    [HideInInspector] public int Wood;
    [HideInInspector] public int Gold;
    
    
    [HideInInspector] public int HP;

    
    [HideInInspector] public int Waves;
    public int CurrentWave = 0;

    private void  Awake()
    {   
        if(Instance != null) return;
        Instance = this;
    }
    void OnEnable()
    {
        TowersManager.TowerPurchased += ManageResourcesOnUsage;
        TowerInfo.OnTowerSold += ManageResourcesOnSell;
        UIManager.OnUseResources += ManageResourcesOnUsage;
    }
    void OnDisable()
    {
        TowersManager.TowerPurchased -= ManageResourcesOnUsage;
        TowerInfo.OnTowerSold -= ManageResourcesOnSell;
        UIManager.OnUseResources -= ManageResourcesOnUsage;
    }
    void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        Stone = InitialResources;
        Wood =  InitialResources;
        Gold =  InitialResources;
        HP =    InitialHP;

        Waves =  10;

        UIManager.Instance.TopDrawer.UpdateHP();
        UIManager.Instance.TopDrawer.UpdateResources();
    }
    public void AddResource(int amount,Farm.FarmType type)
    {
        switch (type)
        {
            case Farm.FarmType.StoneFarm:
                Stone += amount;
                break;
            case Farm.FarmType.WoodFarm:
                Wood += amount;
                break;
            case Farm.FarmType.GoldFarm:
                Gold += amount;
                break;
        }
        UIManager.Instance.TopDrawer.UpdateResources();
    }
    
   
    private void ManageResourcesOnUsage(int[] cost)
    {
        Stone -=  cost[0];
        Wood  -=  cost[1];
        Gold  -=  cost[2];

        UIManager.Instance.TopDrawer.UpdateResources();
    }
    private void ManageResourcesOnSell(Tower tower)
    {
        Stone+= tower.SellingPrice[0];
        Wood += tower.SellingPrice[1];
        Gold += tower.SellingPrice[2];
        
        UIManager.Instance.TopDrawer.UpdateResources();
    }
}
