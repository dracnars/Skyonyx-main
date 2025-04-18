using UnityEngine;

public class playerLife : MonoBehaviour
{
    private Vector3 positionSpawn;

    void Start()
    {
        positionSpawn = transform.position;
    }

    void jeSuisMouru()
    {
        transform.position = positionSpawn;
    }

    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.CompareTag("Respawn"))
        {
            positionSpawn = transform.position;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Kill"))
        {
            jeSuisMouru();
        }
    }
}