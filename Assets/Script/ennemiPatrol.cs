using UnityEngine;

public class ennemiPatrol : MonoBehaviour
{
    public EnemyData data;

    private Vector3 limiteDroitePosition;
    private Vector3 limiteGauchePosition;
    private Rigidbody2D rb;
    private float direction = 1f;
    private SpriteRenderer skin;
    private Vector3 startPosition;

    void Start()
    {
        if (data == null)
        {
            Debug.LogError("EnemyData non assigné à " + gameObject.name);
            enabled = false;
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        skin = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        limiteDroitePosition = new Vector3(data.rightLimit, transform.position.y, transform.position.z);
        limiteGauchePosition = new Vector3(data.leftLimit, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (transform.position.x > limiteDroitePosition.x) direction = -1f;
        if (transform.position.x < limiteGauchePosition.x) direction = 1f;

        rb.linearVelocity = new Vector2(direction * data.speed, rb.linearVelocity.y);
        if (skin != null)
            skin.flipX = direction < 0f;
    }

    void OnDrawGizmos()
    {
        if (data != null)
        {
            Vector3 droite = new Vector3(data.rightLimit, transform.position.y, transform.position.z);
            Vector3 gauche = new Vector3(data.leftLimit, transform.position.y, transform.position.z);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(droite, gauche);
            Gizmos.DrawCube(droite, Vector3.one * 0.2f);
            Gizmos.DrawCube(gauche, Vector3.one * 0.2f);
        }
    }
}