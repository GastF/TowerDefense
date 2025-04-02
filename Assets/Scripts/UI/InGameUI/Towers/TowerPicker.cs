using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TowerPicker : MonoBehaviour
{
    public UnityEngine.GameObject TowerPickerParent;
    public CanvasGroup TowerPickerCanvasGroup;
    public UnityEngine.GameObject TowerHolder;
    private bool _selectedSet = false;

    void OnEnable()
    {
        TowersManager.TowerLoaded += OnTowerAdded;
    }   
    void OnDisable()
    {
        TowersManager.TowerLoaded -= OnTowerAdded;

    }
    private void OnTowerAdded(SO_Tower tower)
    {
        
        var towerHolder = Instantiate(TowerHolder,TowerPickerParent.transform);

        towerHolder.GetComponent<Button>().onClick.AddListener(() => UIManager.Instance.ShowBuildInfo(tower));
        towerHolder.GetComponent<TowerUISetup>().SetTowerParameters(tower);

        if (_selectedSet) return;
        EventSystem.current.SetSelectedGameObject(towerHolder);
        _selectedSet = true;
        
    }
   
}
