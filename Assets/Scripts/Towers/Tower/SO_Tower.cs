using UnityEngine;

[CreateAssetMenu(fileName = "SO_Tower", menuName = "Scriptable Objects/Tower")]
public class SO_Tower : ScriptableObject
{
    [Header("Tower Name")]
    public string TowerName;
    [Header("Tower Icon in UI")]
    public Sprite UiIcon;
    [Header("Tower Stats")]
    public float FireRate;
    public float Damage;
    public float Range;
    
    [Header("Tower Cost")]
    public int[] Cost = {0,0,0};

    [Header("Tower Prefabs")]
    public GameObject TowerPrefab;
    public GameObject BulletPrefab;

}
