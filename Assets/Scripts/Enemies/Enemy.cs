using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour,ITargetable,IAttack
{

    private  NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource gruntSFX;
    [SerializeField] private int _hp = 3;
    private int _initialHP;
    [SerializeField] private int _dmg = 1;
    [SerializeField] private float _movementSpeed = 3;
    [SerializeField] private float _attackInterval = 2;
    [SerializeField] private float _attackRange = 10;
    
    [SerializeField] private GameObject _slowEffect;
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private LayerMask _towerLayer;
    [SerializeField] private  bool _isPack = false;
    [SerializeField] private PackOfEnemies _packOfEnemies;

    [HideInInspector] public bool IsDead = false;
    private bool _isAttacking = false;
    public Transform Target;
    
    
   void Awake()
   {
    _enemyType = GetComponent<EnemyType>();
    _hpBar.maxValue = _hp;
    _hpBar.value = _hp;
    _initialHP = _hp;
   }
    void OnEnable()
    {   
        _slowEffect.SetActive(false);
        _isAttacking = false;
        IsDead = false;
        WavesManager.Instance.EnemiesAlive.Add(this);
        UIManager.Instance.TopDrawer.UpdateEnemiesAlive();
        GetComponent<Collider>().enabled = true;

        anim = GetComponent<Animator>();
                SetTarget();


    }
  
    
    void OnDisable()
    {
        RemoveFromEnemiesAlive(this);
    }
    void Update()
    {
        if(_isAttacking) return;

        RunToTarget();
        
        Attack(Target);
    }

    private void RunToTarget()
    {  
        if(agent.velocity.magnitude > 1)
        {
            anim.SetBool("Run",true);
        }
        else
        {
            anim.SetBool("Run",false);
        }
    }

    public void Attack(Transform target)
    {   

        var distance = Vector3.Distance(target.position,transform.position);
        if(distance <= _attackRange)
        {
            //Debug.Log($"Attacking!");
            StartCoroutine(AttackCoroutine());
            
        }
    }
    
    
    public void RemoveFromEnemiesAlive(Enemy enemy)
    {
        WavesManager.Instance.RemoveFromEnemiesAlive(enemy);
    }
        
    private void Die()
    {   
        

        float range = 200f;
        agent.enabled = false;
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position,range,_towerLayer);
        
        foreach (var hitCollider in hitColliders)
        {   
            Tower tower = hitCollider.GetComponent<Tower>();
            if(tower != null)
            {
                tower.EnemyDestroyed(gameObject);
            }
        }

        anim.SetTrigger("Die");
        sfx.Stop();
        gruntSFX.Play();
        agent.speed = 0;

        _isAttacking = false;
        IsDead = true;

        if(!_isPack)
        {    
            Invoke("ReturnToPool",2);
        }
        else
        {   
            RemoveFromEnemiesAlive(this);
            _packOfEnemies.CheckPack();
        }
        
    }
    private void ReturnToPool()
    {
        if (!IsDead) return;
        ResetEnemy();
        WavesManager.Instance.ReturnToPool(_enemyType.TypeOfEnemy,gameObject);

    }
    public void ResetEnemy()
    {
        IsDead = false;
        agent.enabled = true;
        _slowEffect.SetActive(false);
        anim.SetTrigger("Idle");
        transform.localPosition = Vector3.zero;
        _hp = _initialHP;
        _hpBar.value = _hp;
    }
    public void SetTarget()
    {   
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        Target = GameManager.Instance.Castle;        

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing!");
            return;
        }

        if (!agent.isActiveAndEnabled)
        {
            Debug.LogError("NavMeshAgent is disabled!");
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(Target.position);
        agent.speed = _movementSpeed;
    }
    IEnumerator AttackCoroutine()
    {
        anim.SetBool("Run",false);
        agent.speed = 0;
        _isAttacking = true;
        while(_isAttacking)
        {   
            Target.GetComponent<ITargetable>().Damaged(_dmg);
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(_attackInterval);

            yield return null;
        }
        yield return null;
    }
    public void Damaged(int dmg)
    {
        _hp -= dmg;
        _hpBar.value = _hp;
        if(_hp <= 0)
        {
            
            Die();
        }
    }
    public void Slowed(float slowAmount, float slowDuration)
    {   
        if(IsDead || !isActiveAndEnabled) return;

        StartCoroutine(SlowCoroutine(slowAmount, slowDuration));
    }
    IEnumerator SlowCoroutine(float slowAmount, float slowDuration)
    {
        if (agent == null) yield break;
        _slowEffect.SetActive(true);
        var speed = agent.speed;
        agent.speed = speed - slowAmount;
        yield return new WaitForSeconds(slowDuration);
        _slowEffect.SetActive(false);
        agent.speed = _movementSpeed;
        yield return null;
    }
}
