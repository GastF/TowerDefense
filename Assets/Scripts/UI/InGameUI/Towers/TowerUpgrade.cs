using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerUpgrade : MonoBehaviour
{
    public CanvasGroup TowerUpgradeCanvasGroup;
    public TextMeshProUGUI[] UpgradeCost;
    public Image Background;

    public Button PurchaseUpgradeButton;
    public Button CancelUpgradeButton;
    public Button UpgradeDamageButton;
    public Button UpgradeRangeButton;
    public Button UpgradeSpeedButton;

    [SerializeField] private  int DamageUpgrade = 10;
    [SerializeField] private  int SpeedUpgrade = 10;
    [SerializeField] private  int RangeUpgrade = 10;

    void Update()
    {
        if(UIManager.Instance.CurrentTower == null) return;
        CheckUpgradable(UIManager.Instance.CurrentTower);
    }
    private void CheckUpgradable(Tower tower)
    {
        for (int i = 0; i < tower.UpgradeCost.Length; i++)
        {
            if (tower.UpgradeCost[i] > GameManager.Instance.Stone || tower.UpgradeCost[i] > GameManager.Instance.Wood || tower.UpgradeCost[i] > GameManager.Instance.Gold)
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
    public void UpgradeDamage()
    {
        UIManager.Instance.CurrentTower.Damage += DamageUpgrade;
        UIManager.Instance.CurrentTower.UpgradePuchased = false;
        UIManager.Instance.ShowTowerInfo(UIManager.Instance.CurrentTower);
    }
    public void UpgradeSpeed()
    {
        UIManager.Instance.CurrentTower.Speed += SpeedUpgrade;
        UIManager.Instance.CurrentTower.UpgradePuchased = false;
        UIManager.Instance.ShowTowerInfo(UIManager.Instance.CurrentTower);
    }
    public void UpgradeRange()
    {
        UIManager.Instance.CurrentTower.Range += RangeUpgrade;
        UIManager.Instance.CurrentTower.UpgradePuchased = false;
        UIManager.Instance.ShowTowerInfo(UIManager.Instance.CurrentTower);
    }
}
