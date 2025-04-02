using UnityEngine;

[CreateAssetMenu(fileName = "SO_Tower", menuName = "Scriptable Objects/Tower")]
public class SO_Tower : ScriptableObject
{
    [Header("Tower Name")]
    public string Name;
    [Header("Tower Icon in UI")]
    public Sprite Icon;

    [Header("Tower Info")]
    [TextArea(3,10)]
    public string Info;
    [Header("Tower Stats")]
    public float RateOfFire;
    public int Damage;
    public float Range;

    
    [Header("Tower Cost")]
    public int[] Cost = {0,0,0};
    [Header("Tower Upgrade Cost")]
    public int[] UpgradeCost = {0,0,0};
    [Header("Tower Type of Projectile")]
    public ProjectileType TypeOfProjectile;

    public enum ProjectileType
    {
        Base,
        Explosive,
        Slow,
        ExplosiveAndSlow
    }
    [Header("Tower Prefabs")]
    public UnityEngine.GameObject TowerPrefab;
    public UnityEngine.GameObject ProjectilePrefab;

}
