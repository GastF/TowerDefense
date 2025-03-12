using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class TowerInfo : MonoBehaviour
{
    public CanvasGroup TowerInfoCanvasGroup;
    public CanvasGroup TowerInfoCostCanvasGroup;
    public CanvasGroup TowerUpgradeSellCanvasGroup;
    public Image TowerInfoIcon;
    public TextMeshProUGUI TowerInfoText;
    public TextMeshProUGUI[] TowerCost;
    public TextMeshProUGUI[] TowerSellingPrice;
    public Button CancelButton;
    public Button SellButton;
    
    public static Action<Tower> OnTowerSold;
    public void SellTower()
    {   
        UIManager.Instance.CurrentTower.Sell();
        OnTowerSold?.Invoke(UIManager.Instance.CurrentTower);
        UIManager.Instance.ShowTowers();
    }
}
