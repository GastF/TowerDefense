using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WavesUI : MonoBehaviour
{
    public TextMeshProUGUI WoodBonues;
    public TextMeshProUGUI StoneBonues;
    public TextMeshProUGUI GoldBonues;
    public CanvasGroup BonusPanel;
    public Toggle AutoStartNextWaveToggle;

    public void UpdateBonuses(int wood, int stone, int gold)
    {   
        BonusPanel.DOFade(1, 0.5f);
        WoodBonues.text = wood.ToString();
        StoneBonues.text = stone.ToString();
        GoldBonues.text = gold.ToString();
    }
    public void AutoStartNextWave()
    {
        WavesManager.Instance.AutoStartNextWave = AutoStartNextWaveToggle.isOn;
    }
    public void HideBonusPanel()
    {
        BonusPanel.DOFade(0, 0.5f);
        BonusPanel.blocksRaycasts = false;
        BonusPanel.interactable = false;
    }
}
