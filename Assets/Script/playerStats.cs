using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float speed = 5f;
    public float jumpForce = 8f;
    public float wallJumpForce = 6f;
    public float maxFallSpeed = -20f;
}