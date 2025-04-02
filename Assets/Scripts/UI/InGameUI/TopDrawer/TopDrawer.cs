using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TopDrawer : MonoBehaviour
{
    public TextMeshProUGUI Stone;
    public TextMeshProUGUI Wood;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI Hp;
    public TextMeshProUGUI EnemiesAlive;
    public TextMeshProUGUI Waves;

    public void UpdateResources()
    {
        Stone.text = GameManager.Instance.Stone.ToString();
        Wood.text = GameManager.Instance.Wood.ToString();
        Gold.text = GameManager.Instance.Gold.ToString();
    }
    
    public void UpdateHP()
    {
        Hp.text = GameManager.Instance.HP.ToString();
    }
    public void UpdateWaves()
    {
        Waves.text = $"{GameManager.Instance.CurrentWave + 1}/{GameManager.Instance.TotalWaves}";
    }
    
    public void UpdateEnemiesAlive()
    {
        
        if(WavesManager.Instance.EnemiesAliveInTotal == 0)
        {
            EnemiesAlive.text = "0";
            return;
        }
        EnemiesAlive.text = WavesManager.Instance.EnemiesAliveInTotal.ToString();
    }
   
}
