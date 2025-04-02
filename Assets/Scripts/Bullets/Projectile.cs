using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float TravelVelocity;
    [HideInInspector]public int Damage;
    public GameObject ImpactEffect;
    public GameObject ProjectileModel;
    public SFX SFXData;
    public Explosive ExplosiveData;
    public Slow SlowData;
    private Transform target;
    private Enemy enemyTarget;
    private bool _isInitialized = false;

    private SO_Tower.ProjectileType _type;
    [Serializable] public class SFX
    {   
        public AudioSource FireSFX;
        public AudioSource ImpactSFX;
    }
    [Serializable] public class Slow
    {
        public bool HasSlow = false;
        public float SlowAmount = 0.5f;
        public float SlowDuration = 2f;
    }
    [Serializable]  public class Explosive
    {   

        public bool HasArea;
        public float Duration;
        public float Radius;
        public GameObject Explosion;
    }

    public void Initialize(Transform FirePosition,Transform Target,SO_Tower.ProjectileType Type,int Damage)
    {
        target = Target;
        enemyTarget = Target.GetComponent<Enemy>();
        this.Damage = Damage;
        _type = Type;
       
        gameObject.transform.position = FirePosition.position;
        gameObject.SetActive(true);
        _isInitialized = true;

    }
    void OnEnable()
    {
        if (SFXData.FireSFX != null)
            SFXData.FireSFX.Play();
        if(ImpactEffect != null)
            ImpactEffect.SetActive(false);
        if(ProjectileModel != null)
            ProjectileModel.SetActive(true);
    }

    void Update()
    {
        if(!_isInitialized) return;
        if(!target.gameObject.activeInHierarchy)
        {
            TowersManager.Instance.ReturnToPool(_type, gameObject);
            return;
        }
       
        if (target == null)
        {
            TowersManager.Instance.ReturnToPool(_type, gameObject);
            return;
        }
        else if(enemyTarget.IsDead && _type == SO_Tower.ProjectileType.Base)
        {
            TowersManager.Instance.ReturnToPool(_type, gameObject);
            return;
        }
        else
        {
              // Moverse hacia el objetivo
            Vector3 direction = target.transform.position - transform.position;

            if (direction != Vector3.zero)
                transform.forward = direction;
            

            transform.position = Vector3.MoveTowards(transform.position, target.position, TravelVelocity * Time.deltaTime);
            
        
        }

      
    }
  

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {   
            if(SFXData.ImpactSFX != null)
                SFXData.ImpactSFX.transform.position = other.transform.position;
                SFXData.ImpactSFX.transform.localScale = Vector3.one * 0.5f;
                SFXData.ImpactSFX.Play();
            
            if (ExplosiveData.HasArea)
                Explosion();
            else
                Attack();
        
        }
    }

    public virtual void Explosion()
    {
        if (ExplosiveData.Explosion == null) return;
        
        ExplosiveData.Explosion.SetActive(true);

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosiveData.Radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {   
                var target = collider.GetComponent<ITargetable>();
                target.Damaged(Damage);

                if(SlowData.HasSlow)
                    target.Slowed( SlowData.SlowAmount,SlowData.SlowDuration);
            }
        }
        
        StartCoroutine(ExplosionCoroutine());

    
    }
    IEnumerator ExplosionCoroutine()
    {   
       
        yield return new WaitForSeconds(ExplosiveData.Duration);
        target = null;
        ExplosiveData.Explosion.SetActive(false);
        TowersManager.Instance.ReturnToPool(_type, gameObject);
    }
   
    public virtual void Attack()
    {
        
        
        target.GetComponent<ITargetable>().Damaged(Damage);
        ImpactEffect.SetActive(true);
        ProjectileModel.SetActive(false);
        StartCoroutine(AttackCoroutine());
        

    }
    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        
        target = null;
        TowersManager.Instance.ReturnToPool(_type, gameObject);
    }
}
