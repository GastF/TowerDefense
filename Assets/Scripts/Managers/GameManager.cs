using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Player Resources")]
    [HideInInspector] public int Stone;
    [HideInInspector] public int Wood;
    [HideInInspector] public int Gold;

    [Header("Player Live")]
    [HideInInspector] public int HP;

    [Header("Remaining Waves")]
    [HideInInspector] public int Waves;
    private int _currentWave = 0;

    private void  Awake()
    {   
        if(Instance != null) return;
        Instance = this;
    }
   
    
    public void ManageResources(Tower tower)
    {
        Stone+= tower.SellingPrice[0];
        Wood += tower.SellingPrice[1];
        Gold += tower.SellingPrice[2];
    }
}
