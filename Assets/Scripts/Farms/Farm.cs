using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Farm : MonoBehaviour , IPointerClickHandler
{
   
    public FarmType farmType;
    public enum FarmType
    {    
        none,
        StoneFarm,
        WoodFarm,
        GoldFarm
    }

    public FarmData farm;
    public FarmUpgradeCost farmUpgradeCost;
    public FarmTier farmTier;
    
    [System.Serializable] public class FarmData
    {
        public int level = 1;
        public int amount = 5;
        public float interval = 5f;
    }
    
    [System.Serializable] public class FarmUpgradeCost
    {
        public int[] farmUpgradeCost1 = {20,20,20};
        public int[] farmUpgradeCost2 = {30,30,30};
        public int[] farmUpgradeCost3 = {40,40,40};
    }
    
    [System.Serializable] public class FarmTier
    {
        public GameObject Tier1, Tier2, Tier3;
    }
    

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
        farm.level++;
        farm.amount += 5;
        farm.interval -= 0.5f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Debug.Log($"Farm {farmType} Clicked");
    }
    IEnumerator Produce()
    {
        while (true)
        {
            yield return new WaitForSeconds(farm.interval);
            GameManager.Instance.AddResource(farm.amount,farmType);
        }
    }
    
}
