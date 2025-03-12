using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmInfo : MonoBehaviour
{
    public CanvasGroup InfoCanvasGroup;
    public Image FarmIcon;
    
    [Header("Stats")]
    public TextMeshProUGUI ResourcesAmount;
    public TextMeshProUGUI Interval;
    public TextMeshProUGUI Level;
    [Header("Progress")]
    public Image Progress;
    [Header("Icons")]
        public Sprite[] FarmIcons;

    void Update()
    {   
        if(UIManager.Instance.CurrentFarm == null) return;
        Progress.fillAmount = UIManager.Instance.CurrentFarm.CurrentProgress;
    }
    public void SetFarmStats(Farm farm)
    {
        ResourcesAmount.text = farm.farm.amount.ToString();
        Interval.text = farm.farm.interval.ToString();
        Level.text = farm.farm.level.ToString();
    }
    public void SetFarmIcon(Farm farm)
    {
        switch (farm.farmType)
        {
            case Farm.FarmType.StoneFarm:
                FarmIcon.sprite = FarmIcons[0];
            break;
            case Farm.FarmType.WoodFarm:
                FarmIcon.sprite = FarmIcons[1];
            break;
            case Farm.FarmType.GoldFarm:
                FarmIcon.sprite = FarmIcons[2];
            break;

        }
    }


}
