using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WavesManager : MonoBehaviour
{   
    public static WavesManager Instance;
    [SerializeField] private SO_Wave[] ScriptableWaves;
    [SerializeField] private Transform[] SpawnPoints;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _surface;
    public bool AutoStartNextWave; 

    private bool _firstWave = false;
    private int _totalWaves;
    private int _currentWave = 0;
    private int _enemiesInWave;
    private int _enemiesFromCurrentWaveAlive;
    public int EnemiesAliveInTotal;
    private List<Coroutine> _activeWaves = new List<Coroutine>(); // Mantiene referencia a oleadas en ejecución

    public List<Enemy> EnemiesAlive = new() ;
    public List<GameObject> EnemiesToSpawn = new();
    private Dictionary<EnemyType.Type, ObjectPool<GameObject>> enemyPools = new ();

    private List<Wave> _waves = new List<Wave>();
    private class Wave
    {
        public float SpawnInterval;
        public int EnemyCount;
        public List<GameObject> EnemiesToSpawn = new List<GameObject>();
    }
    

    void Start()
    {   
        if(Instance != null)
        {
            Destroy(Instance);
        } 
        Instance = this;
        
        SetPool();
        InitWaves();
        
    }
    void OnDisable()
    {
        if(Instance != null)
        foreach(var pool in enemyPools.Keys)
        {
            if(enemyPools.ContainsKey(pool))
            {
                enemyPools[pool].Clear();
            }
        }
        enemyPools.Clear();
        
        Destroy(Instance);
        StopAllCoroutines();
    }
   
    private void InitWaves()
    {   
        foreach (var ScriptableWave in ScriptableWaves)
        {   
            var wave = new Wave();
            wave.EnemyCount = ScriptableWave.EnemiesToSpawn.Length;
            wave.SpawnInterval = ScriptableWave.SpawnInterval;
            _totalWaves++;
           
            foreach (var enemy in ScriptableWave.EnemiesToSpawn)
            {
                wave.EnemiesToSpawn.Add(enemy);
            }
            
            _waves.Add(wave);
        }
        GameManager.Instance.TotalWaves = _totalWaves;
        Debug.Log($"Waves init");
    }
    private void SetPool()
    {   
        foreach(var Enemy in EnemiesToSpawn)
        {
            var enemyType = Enemy.GetComponent<EnemyType>();
            if(enemyType != null)
            {
                if(!enemyPools.ContainsKey(enemyType.TypeOfEnemy))
                {
                    enemyPools.Add(enemyType.TypeOfEnemy, new ObjectPool<GameObject>(
                        () => {return Spawn(Enemy);},
                        Enemy => { ReSpawn(Enemy);},
                        Enemy => { Enemy.gameObject.SetActive(false); },
                        Enemy => {Destroy(Enemy.gameObject);},
                        false,500,1000));
                }
            }
        }
        Debug.Log($"POOL SET");
    }
    public void ReturnToPool(EnemyType.Type enemy, GameObject enemyObject)
    {
        
        enemyPools[enemy].Release(enemyObject);
    }
    GameObject Spawn(GameObject enemy)
    {    
        var wave = _waves[_currentWave];
        int spawnIndex = Random.Range(0, SpawnPoints.Length);
        
        
        Instantiate
        (
            enemy, 
            SpawnPoints[spawnIndex].position, 
            Quaternion.identity,
            _surface
        );
        enemy.transform.localPosition = Vector3.zero;
        if(_enemiesFromCurrentWaveAlive > 0)
            _enemiesFromCurrentWaveAlive--;

        CalculateBonusReward();
        enemy.SetActive(false); 
        return  enemy;
    }
    GameObject ReSpawn(GameObject enemy)
    {    
        int spawnIndex = Random.Range(0, SpawnPoints.Length);
        enemy.transform.position = SpawnPoints[spawnIndex].position;
        enemy.SetActive(true);
        
        if(_enemiesFromCurrentWaveAlive > 0)
            _enemiesFromCurrentWaveAlive--;
        CalculateBonusReward();
        return  enemy;
    }
    public void RemoveFromEnemiesAlive(Enemy enemy)
    {
        if (EnemiesAlive.Contains(enemy))
        {
            EnemiesAlive.Remove(enemy);
            if(EnemiesAliveInTotal > 0)
                EnemiesAliveInTotal--;

            UIManager.Instance.TopDrawer.UpdateEnemiesAlive();
        }

        if (EnemiesAlive.Count == 0)
        {   
            if (_currentWave + 1 == _totalWaves && !GameManager.Instance.Defeated)
            {   
                UIManager.Instance.Splashes.ShowVictory();
                Debug.Log("¡Todas las oleadas han terminado!");
                return;
            }
            if (AutoStartNextWave)
            {
                CallNextWave();
            }
        }
    }
    public void CallNextWave(bool earlyCall = false)
    {   
        if (_currentWave + 1 == _totalWaves)
        {
            Debug.Log("¡Todas las oleadas han terminado!");
            return;
        }

        // No permitir avanzar si aún hay enemigos y no se está llamando temprano
        if (!earlyCall && (EnemiesAlive.Count > 0))
        {
            Debug.Log("Aún quedan enemigos en la oleada actual.");
            return;
        }

        if (earlyCall && EnemiesAlive.Count > 0)
        {
            int bonusReward = CalculateBonusReward();

            // Aplicar la recompensa
            GameManager.Instance.AddResource(bonusReward, Farm.FarmType.GoldFarm);
            GameManager.Instance.AddResource(bonusReward, Farm.FarmType.WoodFarm);
            GameManager.Instance.AddResource(bonusReward, Farm.FarmType.StoneFarm);
            
            Debug.Log($"Llamaste temprano la oleada! Bonus: {bonusReward} Oleada: {_currentWave + 1}");
        }

        // Avanzar a la siguiente oleada
        if (_currentWave < _waves.Count)
        {   
            UIManager.Instance.Splashes.ShowWave();

            if(_firstWave)
                _currentWave++;

            if(this != null && isActiveAndEnabled){
            Coroutine newWave = StartCoroutine(StartWave(_currentWave));
            _activeWaves.Add(newWave); // Agregar la oleada a la lista de corrutinas activas
            
            Debug.Log($"WAVE CALLED");
             _firstWave = true;}
        }
    }
    public int CalculateBonusReward()
    {
        // Calcular el porcentaje de enemigos restantes
            float remainingPercentage = (float)_enemiesFromCurrentWaveAlive / _enemiesInWave;
            
            var bonusReward = Mathf.RoundToInt(remainingPercentage * 50);
            UIManager.Instance.Waves.UpdateBonuses(bonusReward, bonusReward, bonusReward);

            return  bonusReward;
    }
  
    private IEnumerator StartWave(int waveIndex)
    {
       

        Debug.Log($"Starting Wave { waveIndex + 1}");
        
        GameManager.Instance.CurrentWave = waveIndex;
        var wave = _waves[waveIndex];
        UIManager.Instance.TopDrawer.UpdateWaves();
        
        int enemiesInWave = wave.EnemyCount;
        float spawnInterval = wave.SpawnInterval;
        

        _enemiesInWave = enemiesInWave;

        _enemiesFromCurrentWaveAlive = enemiesInWave;

        Debug.Log($"{wave.EnemyCount}");
        

        foreach(var enemy in wave.EnemiesToSpawn)
        {
            var count =enemy.GetComponent<EnemyType>().Amount;
            if(count != 1)
                EnemiesAliveInTotal += count;
            else
                EnemiesAliveInTotal++;
        }
        
        for (int i = 0; i < enemiesInWave; i++)
        {
            if (i >= wave.EnemiesToSpawn.Count)
            {
                yield break;
            }
            var enemyInfo = wave.EnemiesToSpawn[i].GetComponent<EnemyType>();
            
            
            var typeOfEnemy = enemyInfo.TypeOfEnemy;

            enemyPools[typeOfEnemy].Get();

            

            
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return null;
    }
}
