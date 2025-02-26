using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowersManager : MonoBehaviour
{
    [Header("Available Towers Setup")]
    [SerializeField] private SO_Tower[] Towers;

    [Header("UI Setup")]
    [SerializeField] private GameObject _towerUIDrawer;
    [SerializeField] private GameObject _towerUIHolder;
    [SerializeField] private GameObject _towerUIIcon;
    private bool _selectedSet = false;


    void Awake()
    {
        foreach(var tower in Towers)
        {
            //*Tower SETUP
                
            //*UI SETUP
            var towerHolder = Instantiate(_towerUIHolder,_towerUIDrawer.transform);
            var towerIcon = Instantiate(_towerUIIcon,towerHolder.transform);
            towerIcon.GetComponent<Image>().sprite = tower.UiIcon;

            if(_selectedSet) continue;
            EventSystem.current.SetSelectedGameObject(towerIcon);
             
        }
    }
}
