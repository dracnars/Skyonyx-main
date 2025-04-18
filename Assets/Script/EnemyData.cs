using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/Enemy Patrol Data")]
public class EnemyData : ScriptableObject
{
    public float speed = 2f;
    public float leftLimit = -5f;
    public float rightLimit = 5f;
}