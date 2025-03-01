using UnityEngine;

[CreateAssetMenu(fileName = "SO_Tower", menuName = "Scriptable Objects/Tower")]
public class SO_Tower : ScriptableObject
{
    [Header("Tower Name")]
    public string TowerName;
    [Header("Tower Icon in UI")]
    public Sprite Icon;

    [Header("Tower Info")]
    [TextArea(3,10)]
    public string Info;
    [Header("Tower Stats")]
    public float FireRate;
    public float Damage;
    public float Range;
    public int ShotsPerFire;

    
    [Header("Tower Cost")]
    public int[] Cost = {0,0,0};

    [Header("Tower Prefabs")]
    public GameObject TowerPrefab;
    public GameObject BulletPrefab;

}
