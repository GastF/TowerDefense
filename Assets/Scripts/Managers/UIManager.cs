using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private RectTransform _bottomDrawer;
    [SerializeField] private RectTransform _arrow;

    [Header("Drawers")]
    public TopDrawer TopDrawer;

    public FarmsUI Farms;
    [Serializable] public class FarmsUI
    {   
        public CanvasGroup FarmsCanvasGroup;
        public FarmInfo FarmInfo;
        public FarmUpgrade FarmUpgrade;
    }
    public TowersUI Towers;
    [Serializable] public class TowersUI
    {
        public CanvasGroup TowersCanvasGroup;
        public TowerPicker TowerPicker;

        public TowerInfo TowerInfo;

        public TowerStats TowerStats;
        
        public TowerUpgrade TowerUpgrade;
    }

    [Header("Colors")]
    public  Color AvailableColor;
    public Color UnavailableColor;
    [HideInInspector] public Tower CurrentTower;
    [HideInInspector] public Farm CurrentFarm;

    public static Action<SO_Tower> TowerSelected;
    public static Action<int[]> OnUseResources;
    private UIInputManager input;

    private bool _displayingInfo = false;
    private bool _drawerOpen = true;

    void Awake()
    {
        input = UIInputManager.Instance;
        if(Instance == null)
            Instance = this;
    
    }
    void OnEnable()
    {
        ShowTowers();

        TowersManager.PlacingCanceled += ShowTowers;
        Tower.OnTowerClicked += ShowTowerInfo;
        Farm.OnFarmClicked += ShowFarmInfo;
        input.RightClick.performed += HideInfo;   

    }

    void OnDisable()
    {
        TowersManager.PlacingCanceled -= ShowTowers;
        Tower.OnTowerClicked -= ShowTowerInfo;
        Farm.OnFarmClicked -= ShowFarmInfo;
        input.RightClick.performed -= HideInfo;
    }

    
    #region Tower UI
    
    public void ShowTowers()
    {
        CurrentTower = null;
        CurrentFarm = null;
        
        EnableCanvasGroup(Towers.TowersCanvasGroup);
        EnableCanvasGroup(Towers.TowerPicker.TowerPickerCanvasGroup);
        EnableCanvasGroup(Towers.TowerInfo.TowerInfoCanvasGroup);
        DisableCanvasGroup(Farms.FarmsCanvasGroup);
        DisableCanvasGroup(Towers.TowerUpgrade.TowerUpgradeCanvasGroup);
        DisableCanvasGroup(Towers.TowerInfo.TowerInfoCanvasGroup);
        DisableCanvasGroup(Towers.TowerInfo.TowerUpgradeSellCanvasGroup);
    }
    
    public void ShowBuildInfo(SO_Tower tower)
    {   
        DisableCanvasGroup(Towers.TowerPicker.TowerPickerCanvasGroup);
        DisableCanvasGroup(Towers.TowerUpgrade.TowerUpgradeCanvasGroup);
        DisableCanvasGroup(Towers.TowerInfo.TowerUpgradeSellCanvasGroup);

        EnableCanvasGroup(Towers.TowerInfo.TowerInfoCanvasGroup);
        EnableCanvasGroup(Towers.TowerInfo.TowerInfoCostCanvasGroup);    
  
        var i = 0;
        foreach (var towerResource in tower.Cost)
        {
           Towers. TowerInfo.TowerCost[i].text = towerResource.ToString();
            i++;
        }
        Towers.TowerStats.Damage.text = tower.Damage.ToString();
        Towers.TowerStats.Range.text = tower.Range.ToString();
        Towers.TowerStats.Speed.text = tower.RateOfFire.ToString();
        Towers.TowerInfo.TowerInfoIcon.sprite = tower.Icon;
        Towers.TowerInfo.TowerInfoText.text = tower.Info;
        TowerSelected?.Invoke(tower);
    }
    public void ShowTowerInfo(Tower tower)
    {   
        if(tower.towerData == null) return;
         CurrentTower = tower;
        _displayingInfo = true;
        if(!_drawerOpen) HideShowDrawer();
        EnableCanvasGroup(Towers.TowersCanvasGroup);
        DisableCanvasGroup(Farms.FarmsCanvasGroup);
        if(tower.UpgradePuchased)
        {
            EnableCanvasGroup (Towers.TowerUpgrade.TowerUpgradeCanvasGroup);
            DisableCanvasGroup(Towers.TowerInfo.TowerInfoCanvasGroup);
            DisableCanvasGroup(Towers.TowerPicker.TowerPickerCanvasGroup);

            return;
        }
        else
        {
            DisableCanvasGroup(Towers.TowerPicker.TowerPickerCanvasGroup);
            DisableCanvasGroup(Towers.TowerInfo.TowerInfoCostCanvasGroup);
            DisableCanvasGroup(Towers.TowerUpgrade.TowerUpgradeCanvasGroup);

            EnableCanvasGroup(Towers.TowerInfo.TowerInfoCanvasGroup);
            EnableCanvasGroup(Towers.TowerInfo.TowerUpgradeSellCanvasGroup);

            if (tower.Damage == 0 || tower.Range == 0 || tower.Speed == 0)
            {
                tower.Damage = tower.towerData.Damage;
                tower.Range = tower.towerData.Range;
                tower.Speed = tower.towerData.RateOfFire;
            }

            Towers.TowerStats.Damage.text = tower.Damage.ToString();
            Towers.TowerStats.Range.text = tower.Range.ToString();
            Towers.TowerStats.Speed.text = tower.Speed.ToString();

            
            SetTowerUpgradeAndSellPrices(tower);
            
            int i = 0;
            i = 0;
            foreach (var towerResource in tower.towerData.Cost)
            {
                Towers.TowerInfo.TowerCost[i].text = towerResource.ToString();
                i++;
            }



            Towers.TowerInfo.TowerInfoIcon.sprite = tower.towerData.Icon;
            Towers.TowerInfo.TowerInfoText.text = tower.towerData.Info;
        }
    }

    private void SetTowerUpgradeAndSellPrices(Tower tower)
    {
        int i = 0;
        int[] totalInvested = { 0, 0, 0 };
        CopyArrayValues(tower.towerData.Cost, tower.SellingPrice);
        CopyArrayValues(tower.towerData.UpgradeCost, tower.UpgradeCost);
        for (i = 0; i < tower.UpgradeCost.Length; i++)
        {
            if (tower.timesUpgraded == 0)
               Towers. TowerUpgrade.UpgradeCost[i].text = tower.UpgradeCost[i].ToString();
            else
               Towers. TowerUpgrade.UpgradeCost[i].text = (tower.UpgradeCost[i] * tower.timesUpgraded).ToString();
            totalInvested[i] = (tower.UpgradeCost[i] * tower.timesUpgraded) / 2;
        }
        for (i = 0; i < tower.SellingPrice.Length; i++)
        {
            if (tower.timesUpgraded == 0)
                Towers.TowerInfo.TowerSellingPrice[i].text = (tower.SellingPrice[i] / 2).ToString();
            else
                Towers.TowerInfo.TowerSellingPrice[i].text = (totalInvested[i] + (tower.SellingPrice[i] / 2)).ToString();
            tower.SellingPrice[i] = (totalInvested[i] + (tower.SellingPrice[i] / 2));
        }
        

    }
    
    private void HideInfo(InputAction.CallbackContext context)
    {
        if(!_displayingInfo) return;
        CurrentTower = null;
        CurrentFarm = null;
        _displayingInfo = false;
        ShowTowers();
    }
    public void ShowTowerUpgrade()
    {   
        OnUseResources?.Invoke(CurrentTower.UpgradeCost);
        EnableCanvasGroup(Towers.TowerUpgrade.TowerUpgradeCanvasGroup);
        DisableCanvasGroup(Towers.TowerInfo.TowerInfoCanvasGroup);
        CurrentTower.UpgradePuchased = true;
        CurrentTower.timesUpgraded++;
        CurrentTower.Upgrade();
        
    }
    public void HideTowerUpgrade()
    {
        ShowTowerInfo(CurrentTower);
    }
    
  
    #endregion
    #region Farm UI
    public void ShowFarmInfo(Farm farm)
    {   
        _displayingInfo = true;
        CurrentFarm = farm;
        if(!_drawerOpen) HideShowDrawer();
        if(farm.UpgradePuchased)
        {
            EnableCanvasGroup(Farms.FarmUpgrade.UpgradeCanvasGroup);
            DisableCanvasGroup(Farms.FarmInfo.InfoCanvasGroup);
            return;
        }
        Farms.FarmInfo.SetFarmIcon(CurrentFarm);
        Farms.FarmInfo.SetFarmStats(CurrentFarm);
        SetFarmUpgradePrices(CurrentFarm);
        EnableCanvasGroup(Farms.FarmsCanvasGroup);
        EnableCanvasGroup(Farms.FarmInfo.InfoCanvasGroup);
        DisableCanvasGroup(Towers.TowersCanvasGroup);
        DisableCanvasGroup(Farms.FarmUpgrade.UpgradeCanvasGroup);


    }
     public void ShowFarmUpgrade()
    {   
        switch(CurrentFarm.farm.level)
        {   
            case 1:
                OnUseResources?.Invoke(CurrentFarm.farmUpgradeCost.farmUpgradeCost1);
                break;
            case 2:
                OnUseResources?.Invoke(CurrentFarm.farmUpgradeCost.farmUpgradeCost2);
                break;
            case 3:
                OnUseResources?.Invoke(CurrentFarm.farmUpgradeCost.farmUpgradeCost3);
                break;
        }
       

        EnableCanvasGroup(Farms.FarmUpgrade.UpgradeCanvasGroup);
        DisableCanvasGroup(Farms.FarmInfo.InfoCanvasGroup);
        if(CurrentFarm.farm.interval == 1) Farms.FarmUpgrade.UpgradeCicleSpeedButton.gameObject.SetActive(false);
        else Farms.FarmUpgrade.UpgradeCicleSpeedButton.gameObject.SetActive(true);
        CurrentFarm.UpgradePuchased = true;
        CurrentFarm.farm.level++;
        CurrentFarm.Upgrade();
        
    }
    public void HideFarmUpgrade()
    {
        ShowTowers();
    }
    public void SetFarmUpgradePrices(Farm farm)
    {
        
        switch (farm.farm.level)
        {
            case 1:
                var cost = farm.farmUpgradeCost.farmUpgradeCost1;
                for(int i = 0 ; i < cost.Length;i++)
                {
                    Farms.FarmUpgrade.UpgradeCost[i].text = cost[i].ToString();
                }
                break;
            case 2:
                var cost2 = farm.farmUpgradeCost.farmUpgradeCost2;
                for(int i = 0 ; i < cost2.Length;i++)
                {
                    Farms.FarmUpgrade.UpgradeCost[i].text = cost2[i].ToString();
                }
                break;
            case 3:
                var cost3 = farm.farmUpgradeCost.farmUpgradeCost3;
                for(int i = 0 ; i < cost3.Length;i++)
                {
                    Farms.FarmUpgrade.UpgradeCost[i].text = cost3[i].ToString();
                }
                break;
        }
    }
    #endregion
    #region Utils
    private void CopyArrayValues(int[] source, int[] destination)
    {
        for (int i = 0; i < source.Length; i++)
        {
            destination[i] = source[i];
        }
    }
    private void DisableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    private void EnableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideShowDrawer()
    {
        float targetHeight = _drawerOpen ? 0 : 257.67f; 
        _arrow.DORotate(new Vector3(0, 0, _drawerOpen ? 270 : 90), 0.3f); 
        _arrow.DOMoveY(_drawerOpen ? 50 : 279.8f, 0.3f);
        _bottomDrawer.DOSizeDelta(new Vector2(_bottomDrawer.sizeDelta.x, targetHeight), 0.3f).SetEase(Ease.OutQuad);
        _drawerOpen = !_drawerOpen; 
    }
    #endregion
}
