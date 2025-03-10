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
    
    [HideInInspector]  public SO_Tower towerData;
    public static event Action<Tower> OnTowerClicked; 
    void OnEnable()
    {
        SellingPrice = new int[3];
        UpgradeCost = new int[3];
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnTowerClicked?.Invoke(this);
    }
    public void Sell()
    {
        Destroy(gameObject);
    }
}


