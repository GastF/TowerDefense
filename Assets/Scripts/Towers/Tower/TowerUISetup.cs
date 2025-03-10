using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUISetup : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI TowerName;
    public TextMeshProUGUI[] TowerCost;
    [Header("Tower Icon")]
    public Image TowerIcon;

    public void SetTowerParameters(SO_Tower tower)
    {
        TowerName.text = tower.Name;
        TowerIcon.sprite = tower.Icon;
        for (int i = 0; i < tower.Cost.Length; i++)
        {
            TowerCost[i].text = tower.Cost[i].ToString();
        }
    }
}
