using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PackOfEnemies : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemiesInPack = new List<Enemy>();
    [SerializeField] private EnemyType _enemyType;

   
    void Awake()
   {
    _enemyType = GetComponent<EnemyType>();
   }
    public void CheckPack()
    {
        foreach (var enemy in _enemiesInPack)
        {
            if (!enemy.IsDead)
            {
                return;
            }
        }
        OnPackCleared();
    }

    private void OnPackCleared()
    {
        Invoke("ResetEnemiesPositions",1);

    }

    private void ResetEnemiesPositions()
    {
        foreach (var enemy in _enemiesInPack)
        {
            if (enemy != null)
            {
                enemy.ResetEnemy(); // Asegúrate de reiniciar estados como vida, NavMeshAgent, etc.
            }
        }
                WavesManager.Instance.ReturnToPool(_enemyType.TypeOfEnemy, gameObject);

    }
}