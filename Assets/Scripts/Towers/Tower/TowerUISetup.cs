using System;
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
    [Header("Tower Button")]
    private Button _towerButton;
    private Image _holder;

    void Awake()
    {
        _towerButton = GetComponent<Button>();    
        _holder = GetComponent<Image>();
    }

    void Update()
    {
        CheckIfPurchasable();
    }

    private void CheckIfPurchasable()
    {
        if (GameManager.Instance.Stone < int.Parse(TowerCost[0].text) || GameManager.Instance.Wood < int.Parse(TowerCost[1].text) || GameManager.Instance.Gold < int.Parse(TowerCost[2].text))
        {
            _towerButton.interactable = false;
            _holder.color = new Color(0.533f, 0.529f, 0.529f, 1.000f);
            
            
        }
        else
        {
            _towerButton.interactable = true;
            _holder.color = Color.white;
        }
    }

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
