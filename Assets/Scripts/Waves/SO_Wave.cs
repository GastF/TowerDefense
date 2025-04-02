using UnityEngine;

[CreateAssetMenu(fileName = "SO_Wave", menuName = "Scriptable Objects/Wave")]
public class SO_Wave : ScriptableObject
{
    public float SpawnInterval;
    public UnityEngine.GameObject[] EnemiesToSpawn;
}
