using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //*UI
    [Header("Tower Picker ")]
    public GameObject TowerPickerParent;
    public CanvasGroup TowerPickerCanvasGroup;
    public GameObject TowerHolder;
    public GameObject TowerIcon;
    [Header("Tower Info ")]
    public CanvasGroup TowerInfoCanvasGroup;
    public Image TowerInfoIcon;
    public TextMeshProUGUI TowerInfo;
    public TextMeshProUGUI[] TowerResources;
    public static Action<SO_Tower> TowerSelected;
    private bool _selectedSet = false;

    void OnEnable()
    {
        TowersManager.TowerLoaded += OnTowerAdded;
        TowersManager.TowerPlaced += ShowTowers;
        TowersManager.PlacingCanceled += ShowTowers;
        ShowTowers();
    }
    void OnDisable()
    {
        TowersManager.TowerLoaded -= OnTowerAdded;
        TowersManager.TowerPlaced -= ShowTowers;
        TowersManager.PlacingCanceled -= ShowTowers;
    }

    private void OnTowerAdded(SO_Tower tower)
    {
        var towerHolder = Instantiate(TowerHolder, TowerPickerParent.transform);
        var towerIcon = Instantiate(TowerIcon, towerHolder.transform);

        towerHolder.GetComponent<Button>().onClick.AddListener(() => ShowInfo(tower));
        towerIcon.GetComponent<Image>().sprite = tower.Icon;

        if (_selectedSet) return;
        EventSystem.current.SetSelectedGameObject(towerIcon);
        _selectedSet = true;
    }

    private void ShowTowers()
    {
        TowerPickerCanvasGroup.alpha = 1;
        TowerPickerCanvasGroup.blocksRaycasts = true;
        TowerInfoCanvasGroup.alpha = 0;
        TowerInfoCanvasGroup.blocksRaycasts = false;
    }
    private void ShowInfo(SO_Tower tower)
    {   
        TowerPickerCanvasGroup.alpha = 0;
        TowerPickerCanvasGroup.blocksRaycasts = false;
        TowerInfoCanvasGroup.alpha = 1;
        TowerInfoCanvasGroup.blocksRaycasts = true;
        var i = 0;
        foreach (var towerResource in tower.Cost)
        {
            TowerResources[i].text = towerResource.ToString();
            i++;
        }
        TowerInfoIcon.sprite = tower.Icon;
        TowerInfo.text = tower.Info;
        TowerSelected?.Invoke(tower);
    }
}
