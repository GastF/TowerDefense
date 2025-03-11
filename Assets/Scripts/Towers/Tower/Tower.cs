using System;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class Tower: MonoBehaviour , IPointerClickHandler
{

       public float Speed;
       public float Damage;
       public float Range;
       public int []SellingPrice = {0,0,0};
       public int []UpgradeCost = {0,0,0};
       public int timesUpgraded;
       public bool UpgradePuchased;
       public GameObject TowerTier1 , TowerTier2 , TowerTier3;
    
    [HideInInspector]  public SO_Tower towerData;
    public static event Action<Tower> OnTowerClicked; 
    void OnEnable()
    {
        SellingPrice = new int[3];
        UpgradeCost = new int[3];
        timesUpgraded = 0;
        TowerTier1.SetActive(true);
        TowerTier2.SetActive(false);
        TowerTier3.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnTowerClicked?.Invoke(this);
    }
    public void Upgrade()
    {
        switch (timesUpgraded)
        {
            
            case 1:
                TowerTier1.SetActive(false);
                TowerTier2.SetActive(true);
                TowerTier3.SetActive(false);
                break;
            case 2:
                TowerTier1.SetActive(false);
                TowerTier2.SetActive(false);
                TowerTier3.SetActive(true);
                break;
        }
    }
    public void Sell()
    {
        Destroy(gameObject);
    }
}


