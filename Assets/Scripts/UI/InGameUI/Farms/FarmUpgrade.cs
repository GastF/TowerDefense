using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmUpgrade : MonoBehaviour
{   
    public CanvasGroup UpgradeCanvasGroup;
    public TextMeshProUGUI[] UpgradeCost;
    public Image Background;
    public Button PurchaseUpgradeButton;
    public Button CancelUpgradeButton;
    public Button UpgradeResourcesButton;
    public Button UpgradeCicleSpeedButton;

    void Update()
    {
        if(UIManager.Instance.CurrentFarm == null) return;
        CheckUpgradable(UIManager.Instance.CurrentFarm);
    }

    private void CheckUpgradable(Farm farm)
    {
        switch(farm.farm.level)
        {
            case 1:
                var cost = farm.farmUpgradeCost.farmUpgradeCost1;
                for (int i = 0; i < cost.Length; i++)
                {
                    if (cost[i] > GameManager.Instance.Stone || cost[i] > GameManager.Instance.Wood || cost[i] > GameManager.Instance.Gold)
                    {
                        PurchaseUpgradeButton.interactable = false;
                        Background.color = UIManager.Instance.UnavailableColor;
                        return;
                    }
                }
                PurchaseUpgradeButton.interactable = true;
                Background.color = UIManager.Instance.AvailableColor;
                return;   
           
            case 2:
                var cost2 = farm.farmUpgradeCost.farmUpgradeCost1;
                for (int i = 0; i < cost2.Length; i++)
                {
                    if (cost2[i] > GameManager.Instance.Stone || cost2[i] > GameManager.Instance.Wood || cost2[i] > GameManager.Instance.Gold)
                    {
                        PurchaseUpgradeButton.interactable = false;
                        Background.color = UIManager.Instance.UnavailableColor;
                        return;
                    }
                }
                PurchaseUpgradeButton.interactable = true;
                Background.color = UIManager.Instance.AvailableColor;
                return;  
            case 3:
                var cost3 = farm.farmUpgradeCost.farmUpgradeCost1;
                for (int i = 0; i < cost3.Length; i++)
                {
                    if (cost3[i] > GameManager.Instance.Stone || cost3[i] > GameManager.Instance.Wood || cost3[i] > GameManager.Instance.Gold)
                    {
                        PurchaseUpgradeButton.interactable = false;
                        Background.color = UIManager.Instance.UnavailableColor;
                        return;
                    }
                }
                PurchaseUpgradeButton.interactable = true;
                Background.color = UIManager.Instance.AvailableColor;
                return;  
        }
    }

    public void UpgradeResources()
    {
        UIManager.Instance.CurrentFarm.farm.amount += 1;
        UIManager.Instance.CurrentFarm.UpgradePuchased = false;
        UIManager.Instance.ShowFarmInfo(UIManager.Instance.CurrentFarm);
    }
    public void UpgradeCicleSpeed()
    {
        UIManager.Instance.CurrentFarm.farm.interval -= 1;
        UIManager.Instance.CurrentFarm.UpgradePuchased = false;
        UIManager.Instance.ShowFarmInfo(UIManager.Instance.CurrentFarm);
    }
}
