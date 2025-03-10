using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerUpgrade : MonoBehaviour
{
    public CanvasGroup TowerUpgradeCanvasGroup;
    public TextMeshProUGUI[] UpgradeCost;
    public Button CancelUpgradeButton;
    public Button UpgradeDamageButton;
    public Button UpgradeRangeButton;
    public Button UpgradeSpeedButton;
    public int DamageUpgrade = 10;
    public int SpeedUpgrade = 10;
    public int RangeUpgrade = 10;
}
