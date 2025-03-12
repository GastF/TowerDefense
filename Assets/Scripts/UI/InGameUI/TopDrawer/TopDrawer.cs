using UnityEngine;
using TMPro;

public class TopDrawer : MonoBehaviour
{
    public TextMeshProUGUI stone;
    public TextMeshProUGUI wood;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI waves;

    public void UpdateResources()
    {
        stone.text = GameManager.Instance.Stone.ToString();
        wood.text = GameManager.Instance.Wood.ToString();
        gold.text = GameManager.Instance.Gold.ToString();
    }
    public void UpdateHP()
    {
        hp.text = GameManager.Instance.HP.ToString();
    }
    public void UpdateWaves()
    {
        waves.text = $"{GameManager.Instance.CurrentWave}/{GameManager.Instance.Waves}";
    }
   
}
