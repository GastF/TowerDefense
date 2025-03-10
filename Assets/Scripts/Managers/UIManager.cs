using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private RectTransform _bottomDrawer;
    [SerializeField] private RectTransform _arrow;

    [Header("Top Drawer")]
    [SerializeField] private TopDrawer TopDrawer;
    [Header("Bottom Drawer")]

    [Header("Tower Picker ")]
    [SerializeField] private TowerPicker TowerPicker;

    [Header("Tower  Info ")]
    [SerializeField] private TowerInfo TowerInfo;

    [Header("Tower Stats ")]
    [SerializeField] private TowerStats TowerStats;
    
    [Header("Tower Upgrade ")]
    [SerializeField] private TowerUpgrade TowerUpgrade;

    private Tower _currentTower;
    public static Action<SO_Tower> TowerSelected;
    
    private UIInputManager input;

    private GameManager gameManager;
    private bool _displayingInfo = false;
    private bool _selectedSet = false;
    private bool _drawerOpen = true;

    void Awake()
    {
        input = UIInputManager.Instance;
        gameManager = GameManager.Instance;
    }
    void OnEnable()
    {
        ShowTowers();

        TowersManager.TowerPlaced += ShowTowers;
        TowersManager.PlacingCanceled += ShowTowers;
        Tower.OnTowerClicked += ShowInfo;
        TowersManager.TowerLoaded += OnTowerAdded;
        input.RightClick.performed += HideInfo;   

    }

    void OnDisable()
    {
        TowersManager.TowerLoaded -= OnTowerAdded;
        TowersManager.TowerPlaced -= ShowTowers;
        TowersManager.PlacingCanceled -= ShowTowers;
        Tower.OnTowerClicked -= ShowInfo;
        input.RightClick.performed -= HideInfo;
    }

    private void OnTowerAdded(SO_Tower tower)
    {
        
        var towerHolder = Instantiate(TowerPicker.TowerHolder,TowerPicker.TowerPickerParent.transform);

        towerHolder.GetComponent<Button>().onClick.AddListener(() => ShowBuildInfo(tower));
        towerHolder.GetComponent<TowerUISetup>().SetTowerParameters(tower);

        if (_selectedSet) return;
        EventSystem.current.SetSelectedGameObject(towerHolder);
        _selectedSet = true;
        
    }

    public void ShowTowers()
    {
        _currentTower = null;
        EnableCanvasGroup(TowerPicker.TowerPickerCanvasGroup);
        EnableCanvasGroup(TowerInfo.TowerInfoCanvasGroup);
        DisableCanvasGroup(TowerUpgrade.TowerUpgradeCanvasGroup);
        DisableCanvasGroup(TowerInfo.TowerInfoCanvasGroup);
        DisableCanvasGroup(TowerInfo.TowerUpgradeSellCanvasGroup);
    }
    private void ShowBuildInfo(SO_Tower tower)
    {   
        DisableCanvasGroup(TowerPicker.TowerPickerCanvasGroup);
        DisableCanvasGroup(TowerUpgrade.TowerUpgradeCanvasGroup);
        DisableCanvasGroup(TowerInfo.TowerUpgradeSellCanvasGroup);

        EnableCanvasGroup(TowerInfo.TowerInfoCanvasGroup);
        EnableCanvasGroup(TowerInfo.TowerInfoCostCanvasGroup);    
  
        var i = 0;
        foreach (var towerResource in tower.Cost)
        {
            TowerInfo.TowerCost[i].text = towerResource.ToString();
            i++;
        }
        TowerStats.Damage.text = tower.Damage.ToString();
        TowerStats.Range.text = tower.Range.ToString();
        TowerStats.Speed.text = tower.RateOfFire.ToString();
        TowerInfo.TowerInfoIcon.sprite = tower.Icon;
        TowerInfo.TowerInfoText.text = tower.Info;
        TowerSelected?.Invoke(tower);
    }
    private void ShowInfo(Tower tower)
    {   
        if(tower.towerData == null) return;
         _currentTower = tower;
        _displayingInfo = true;
        if(!_drawerOpen) HideShowDrawer();
        if(tower.UpgradePuchased)
        {
            EnableCanvasGroup(TowerUpgrade.TowerUpgradeCanvasGroup);
            DisableCanvasGroup(TowerInfo.TowerInfoCanvasGroup);
            DisableCanvasGroup(TowerPicker.TowerPickerCanvasGroup);

            return;
        }
        else
        {
            DisableCanvasGroup(TowerPicker.TowerPickerCanvasGroup);
            DisableCanvasGroup(TowerInfo.TowerInfoCostCanvasGroup);
            DisableCanvasGroup(TowerUpgrade.TowerUpgradeCanvasGroup);

            EnableCanvasGroup(TowerInfo.TowerInfoCanvasGroup);
            EnableCanvasGroup(TowerInfo.TowerUpgradeSellCanvasGroup);

            if (tower.Damage == 0 || tower.Range == 0 || tower.Speed == 0)
            {
                tower.Damage = tower.towerData.Damage;
                tower.Range = tower.towerData.Range;
                tower.Speed = tower.towerData.RateOfFire;
            }

            TowerStats.Damage.text = tower.Damage.ToString();
            TowerStats.Range.text = tower.Range.ToString();
            TowerStats.Speed.text = tower.Speed.ToString();

            
            UpgradeAndSellPrices(tower);
            
            int i = 0;
            i = 0;
            foreach (var towerResource in tower.towerData.Cost)
            {
                TowerInfo.TowerCost[i].text = towerResource.ToString();
                i++;
            }



            TowerInfo.TowerInfoIcon.sprite = tower.towerData.Icon;
            TowerInfo.TowerInfoText.text = tower.towerData.Info;
        }
    }

    private void UpgradeAndSellPrices(Tower tower)
    {
        int i = 0;
        int[] totalInvested = { 0, 0, 0 };
        CopyArrayValues(tower.towerData.Cost, tower.SellingPrice);
        CopyArrayValues(tower.towerData.UpgradeCost, tower.UpgradeCost);

        for (i = 0; i < tower.UpgradeCost.Length; i++)
        {
            if (tower.timesUpgraded == 0)
                TowerUpgrade.UpgradeCost[i].text = tower.UpgradeCost[i].ToString();
            else
                TowerUpgrade.UpgradeCost[i].text = (tower.UpgradeCost[i] * tower.timesUpgraded).ToString();
            totalInvested[i] = (tower.UpgradeCost[i] * tower.timesUpgraded) / 2;
        }
        for (i = 0; i < tower.SellingPrice.Length; i++)
        {
            if (tower.timesUpgraded == 0)
                TowerInfo.TowerSellingPrice[i].text = (tower.SellingPrice[i] / 2).ToString();
            else
                TowerInfo.TowerSellingPrice[i].text = (totalInvested[i] + (tower.SellingPrice[i] / 2)).ToString();
            tower.SellingPrice[i] = (totalInvested[i] + (tower.SellingPrice[i] / 2));
        }

    }

    private void CopyArrayValues(int[] source, int[] destination)
    {
        for (int i = 0; i < source.Length; i++)
        {
            destination[i] = source[i];
        }
    }
    private void HideInfo(InputAction.CallbackContext context)
    {
        if(!_displayingInfo) return;
        _currentTower = null;
        _displayingInfo = false;
        ShowTowers();
    }
    public void ShowUpgrade()
    {   
        EnableCanvasGroup(TowerUpgrade.TowerUpgradeCanvasGroup);
        DisableCanvasGroup(TowerInfo.TowerInfoCanvasGroup);
        _currentTower.UpgradePuchased = true;
        _currentTower.timesUpgraded++;
        
        
    }
    public void HideUpgrade()
    {
        ShowInfo(_currentTower);
    }
    public void SellTower()
    {   
        _currentTower.Sell();
        gameManager.ManageResources(_currentTower);
        UpdateResourcesUI();
        ShowTowers();
    }

    private void UpdateResourcesUI()
    {
        TopDrawer.stone.text = gameManager.Stone.ToString();
        TopDrawer.wood.text = gameManager.Wood.ToString();
        TopDrawer.gold.text = gameManager.Gold.ToString();
    }

    public void UpgradeDamage()
    {
        _currentTower.Damage += TowerUpgrade.DamageUpgrade;
        _currentTower.UpgradePuchased = false;
        ShowInfo(_currentTower);
    }
    public void UpgradeSpeed()
    {
        _currentTower.Speed += TowerUpgrade.SpeedUpgrade;
        _currentTower.UpgradePuchased = false;
        ShowInfo(_currentTower);
    }
    public void UpgradeRange()
    {
        _currentTower.Range += TowerUpgrade.RangeUpgrade;
        _currentTower.UpgradePuchased = false;
        ShowInfo(_currentTower);
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

  
}
