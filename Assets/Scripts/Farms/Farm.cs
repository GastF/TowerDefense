using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Farm : MonoBehaviour , IPointerClickHandler
{
    #region Variables
    public FarmType farmType;
    public enum FarmType
    {    
        none,
        StoneFarm,
        WoodFarm,
        GoldFarm
    }

    [HideInInspector]public bool UpgradePuchased;

    public FarmData farm;
    public FarmUpgradeCost farmUpgradeCost;
    public FarmTier farmTier;
    public float CurrentProgress;
    
    [Serializable] public class FarmData
    {
        public int level = 1;
        public int amount = 5;
        public float interval = 5f;
    }
    
    [Serializable] public class FarmUpgradeCost
    {
        public int[] farmUpgradeCost1 = {20,20,20};
        public int[] farmUpgradeCost2 = {30,30,30};
        public int[] farmUpgradeCost3 = {40,40,40};
    }
    
    [Serializable] public class FarmTier
    {
        public GameObject Tier1, Tier2, Tier3;
    }
    #endregion
    public static event Action<Farm> OnFarmClicked; 

    void OnEnable()
    {
        Init();
    }
    
    public void Init()
    {   
        farmTier.Tier1.SetActive(true);
        farmTier.Tier2.SetActive(false);
        farmTier.Tier3.SetActive(false);

     
        StartCoroutine(Produce());
    }
    public void Upgrade()
    {
         switch (farm.level)
        {
            
            case 2:
                farmTier.Tier1.SetActive(false);
                farmTier.Tier2.SetActive(true);
                farmTier.Tier3.SetActive(false);
                break;
            case 3:
                farmTier.Tier1.SetActive(false);
                farmTier.Tier2.SetActive(false);
                farmTier.Tier3.SetActive(true);
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {  
            OnFarmClicked?.Invoke(this);
            Debug.Log($"Farm {farmType} Clicked");
        }
    }
    IEnumerator Produce()
    {
        
        while (true)
        {
            float elapsedTime = 0f;
            CurrentProgress = 0f;

            while (elapsedTime < farm.interval)
            {
                elapsedTime += Time.deltaTime;
                CurrentProgress = Mathf.Clamp01(elapsedTime / farm.interval);
                yield return null; // Espera al siguiente frame
            }

            CurrentProgress = 1f;
            GameManager.Instance.AddResource(farm.amount, farmType);
        }
    }
    
    
}
