using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class Tower: MonoBehaviour , IPointerClickHandler
{

    public Transform ShootPoint;
    
    [SerializeField] private GameObject RangeArea;
    [SerializeField] private GameObject Object;
    
    public MeshRenderer[] meshes;
    
    public TowerStats  Stats;
    public TowerData Data;
    [Serializable] public class TowerStats
    {
        public float Speed;
        public int Damage;
        public float Range;
    }
    [Serializable] public class TowerData
    {
        public int []SellingPrice = {0,0,0};
        public int []UpgradeCost = {0,0,0};
        public int timesUpgraded;
        public bool UpgradePuchased;
        public GameObject TowerTier1 , TowerTier2 , TowerTier3;
        [HideInInspector]  public SO_Tower ScriptableData;

    }
    
    public List<GameObject> EnemiesInRange = new();
    public GameObject CurrentTarget = null;
    private bool _attacking = false;
    public static event Action<Tower> OnTowerClicked; 
    private float _baseAttackCooldown = 2f; 

    private float _attackCooldownTime => _baseAttackCooldown / Stats.Speed;
    private bool _canAttack = true;
    void OnEnable()
    {
        Data.SellingPrice = new int[3];
        Data.UpgradeCost = new int[3];
        Data.timesUpgraded = 0;
        Data.TowerTier1.SetActive(true);
        Data.TowerTier2.SetActive(false);
        Data.TowerTier3.SetActive(false);
    }
    void Update()
    {
        if(EnemiesInRange.Count > 0 && CurrentTarget == null)
        {
            UpdateClosestTarget();
        }
        if (CurrentTarget != null)
        {   
            if(Object!=null)
                RotateTowards();
            if(!_attacking)
                Attack();
        }
    }

    void RotateTowards( )
    {
        Vector3 direction = (CurrentTarget.transform.position - Object.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Object.transform.rotation = Quaternion.Slerp(Object.transform.rotation, targetRotation, Time.deltaTime * 10f); // 5 es la velocidad de rotación
        
    }

    public void Attack()
    {
        if (!_canAttack) return;

        var projectile = TowersManager.Instance.ProjectilePools[Data.ScriptableData.TypeOfProjectile].Get().GetComponent<Projectile>();
        projectile.Initialize(
            ShootPoint, 
            CurrentTarget.transform, 
            Data.ScriptableData.TypeOfProjectile,
            Stats.Damage
         );

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldownTime);
        _canAttack = true;
    }
    public void EnemyDestroyed(GameObject enemy)
    {
    
        if(EnemiesInRange.Contains(enemy))
        {
            
            EnemiesInRange.Remove(enemy);
            CurrentTarget = null;
            UpdateClosestTarget();
        }
        
        
    }
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left )
            OnTowerClicked?.Invoke(this);
    }
    public void Upgrade()
    {
        switch (Data.timesUpgraded)
        {
            
            case 1:
                Data.TowerTier1.SetActive(false);
                Data.TowerTier2.SetActive(true);
                Data.TowerTier3.SetActive(false);
                break;
            case 2:
                Data.TowerTier1.SetActive(false);
                Data.TowerTier2.SetActive(false);
                Data.TowerTier3.SetActive(true);
                break;
        }
    }
    public void Sell()
    {
        Destroy(gameObject);
    }
    public void ShowRange()
    {
        RangeArea.GetComponent<MeshRenderer>().enabled = true;
        float diameter = Stats.Range * 2f;
        RangeArea.transform.localScale = new Vector3(diameter, diameter, diameter);
    }
   
    public void HideRange()
    {
        RangeArea.GetComponent<MeshRenderer>().enabled = false;

    }
    public void DisableRangeCollider()
    {
        RangeArea.GetComponent<Collider>().enabled = false;
    }
    public void EnableRangeCollider()
    {
        RangeArea.GetComponent<Collider>().enabled = true;
    }
    public void UpdateClosestTarget()
    {

        if(CurrentTarget != null)
        {
            return;
        }

        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var enemy in EnemiesInRange)
        {   
            if(enemy == null) return;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {   
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;          
            }
        }
        if(closestEnemy != null)
        {
            CurrentTarget = closestEnemy;
        }
        else
        {
            CurrentTarget = null;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
           
            EnemiesInRange.Add(other.gameObject);
            UpdateClosestTarget();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
           
            EnemiesInRange.Remove(other.gameObject);
            CurrentTarget = null;
            UpdateClosestTarget();
        }
    }

}


