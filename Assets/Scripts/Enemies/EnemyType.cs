using UnityEngine;

public class EnemyType : MonoBehaviour
{   
    public Type TypeOfEnemy;
    public int Amount;
    public enum Type
    {
        Basic,
        Runner,
        Tank,
        PackOfFive1,
        PackOfFive2,
        PackOfFive3,
        PackOfFive4,
        PackOfFive5,
        PackOfFive6,
        PackOfTen
        
    }
}
