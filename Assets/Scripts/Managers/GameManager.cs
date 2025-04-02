using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour,ITargetable
{
    public static GameManager Instance;
    
    public int InitialResources = 10;
    public int InitialHP = 10;
    public Transform Castle;
    [HideInInspector] public int Stone;
    [HideInInspector] public int Wood;
    [HideInInspector] public int Gold;
    
    
    [SerializeField] private Slider _healthBar;
    [HideInInspector] public int HP;
   
    [HideInInspector] public int TotalWaves;
    [HideInInspector] public int CurrentWave = 0;
    [HideInInspector] public bool Defeated = false;

    private void  Awake()
    {   
        if(Instance != null)
        {
            Destroy(Instance);
        } 
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
        if(Instance != null)
        Destroy(Instance);
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
        _healthBar.maxValue = InitialHP;
        _healthBar.value = InitialHP;
        Defeated = false;


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
        Stone+= tower.Data.SellingPrice[0];
        Wood += tower.Data.SellingPrice[1];
        Gold += tower.Data.SellingPrice[2];
        
        UIManager.Instance.TopDrawer.UpdateResources();
    }
    public void DoubleTime()
    {
        Time.timeScale = 2;
    }
    public void NormalTime()
    {   
        AudioListener.pause = false;
        Time.timeScale = 1;
    }
    public void PauseTime()
    {   
        
        AudioListener.pause = true;
        Time.timeScale = 0;
    }
    
    public void Damaged(int dmg)
    {
        if(HP <=0 )
        {
            HP = 0;
            UIManager.Instance.Splashes.ShowDefeat();
            UIManager.Instance.TopDrawer.UpdateHP();
            _healthBar.value = 0;
            return;
        }
        HP -= dmg;
        _healthBar.value = HP;
        UIManager.Instance.TopDrawer.UpdateHP();
    }

    public void RestartScene()
    {
        var scene = SceneManager.GetActiveScene().name;
        AudioListener.pause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioListener.pause = false;
        Time.timeScale = 1;
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void Slowed(float slowAmount, float slowDuration)
    {
        
    }
}
