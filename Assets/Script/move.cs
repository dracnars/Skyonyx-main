using UnityEngine;

public class Move : MonoBehaviour
{
    public PlayerStats stats;
    public Transform shadowPrefab;

    private Rigidbody2D rb;
    private CapsuleCollider2D monColl;
    private bool grounded;
    private float rayonDetection;
    private bool wallSliding;
    private float wallSlideSpeed;
    private float wallSlideTimer;
    private bool touchingWallLeft, touchingWallRight;
    private bool touchingWallLeftFeet, touchingWallRightFeet;
    private bool touchingWallLeftMid, touchingWallRightMid;
    private SpriteRenderer skin;
    private Animator anim;
    private Transform shadow;
    private float lastWallJumpTime;
    public Transform groundChecker;
    public LayerMask wallLayer;

    void Start()
    {
        monColl = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        skin = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        shadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
    }

    void Update()
    {
        groundCheck();
        checkWall();
        moveCheck();

        if (grounded)
            flipCheck();

        animCheck();
        shadowUpdate();

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapWithShadow();
        }
    }

    void checkWall()
    {
        float checkDistance = 0.55f;
        Vector2 position = transform.position;
        float colliderHeight = monColl.size.y * 0.5f;

        Vector2 feetPosition = position + Vector2.down * (colliderHeight - 0.5f);
        Vector2 midPosition = position + Vector2.down * ((colliderHeight * 0.5f) - 0.25f);

        RaycastHit2D hitLeft = Physics2D.Raycast(position, Vector2.left, checkDistance, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(position, Vector2.right, checkDistance, wallLayer);
        RaycastHit2D hitLeftFeet = Physics2D.Raycast(feetPosition, Vector2.left, checkDistance, wallLayer);
        RaycastHit2D hitRightFeet = Physics2D.Raycast(feetPosition, Vector2.right, checkDistance, wallLayer);
        RaycastHit2D hitLeftMid = Physics2D.Raycast(midPosition, Vector2.left, checkDistance, wallLayer);
        RaycastHit2D hitRightMid = Physics2D.Raycast(midPosition, Vector2.right, checkDistance, wallLayer);

        touchingWallLeft = hitLeft.collider != null;
        touchingWallRight = hitRight.collider != null;
        touchingWallLeftFeet = hitLeftFeet.collider != null;
        touchingWallRightFeet = hitRightFeet.collider != null;
        touchingWallLeftMid = hitLeftMid.collider != null;
        touchingWallRightMid = hitRightMid.collider != null;

        bool nearWall = touchingWallLeft || touchingWallRight || touchingWallLeftFeet || touchingWallRightFeet || touchingWallLeftMid || touchingWallRightMid;
        wallSliding = !grounded && nearWall && Time.time > lastWallJumpTime + 0.1f;

        if (wallSliding)
        {
            wallSlideTimer += Time.deltaTime;
            float slideFactor = Mathf.Clamp01(wallSlideTimer / 1.5f);
            wallSlideSpeed = Mathf.Lerp(0.5f, stats.maxFallSpeed, slideFactor);
        }
        else
        {
            wallSlideTimer = 0f;
        }
    }

    void groundCheck()
    {
        grounded = false;
        rayonDetection = monColl.size.x * 0.20f;

        Collider2D[] colls = Physics2D.OverlapCircleAll((Vector2)transform.position + Vector2.up * (monColl.offset.y + rayonDetection * 0.8f - (monColl.size.y / 2)), rayonDetection);

        foreach (Collider2D coll in colls)
        {
            if (coll != monColl && !coll.isTrigger && coll.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = true;
                break;
            }
        }
    }

    void moveCheck()
    {
        Vector2 velocity = rb.linearVelocity;

        if (grounded)
        {
            velocity.x = Input.GetAxis("Horizontal") * stats.speed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                velocity.y = stats.jumpForce;
            }
            else if (wallSliding && Time.time > lastWallJumpTime + 0.4f)
            {
                lastWallJumpTime = Time.time;
                velocity.y = stats.jumpForce;
                velocity.x = (touchingWallLeft || touchingWallLeftFeet || touchingWallLeftMid) ? stats.wallJumpForce : -stats.wallJumpForce;
                wallSliding = false;
            }
        }

        if (wallSliding && velocity.y < 0)
        {
            velocity.y = -wallSlideSpeed;
        }

        rb.linearVelocity = velocity;
    }

    void flipCheck()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            skin.flipX = Input.GetAxis("Horizontal") < 0;
        }
    }

    void animCheck()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("grounded", grounded);
    }

    void shadowUpdate()
    {
        if (shadow != null)
        {
            Vector3 cameraCenter = Camera.main.transform.position;
            float distanceToCameraX = transform.position.x - cameraCenter.x;
            Vector3 mirroredPosition = new Vector3(cameraCenter.x - distanceToCameraX, transform.position.y, transform.position.z);
            shadow.position = mirroredPosition;
        }
    }

    void SwapWithShadow()
    {
        if (shadow == null || shadow.GetComponent<Collider2D>() == null) return;

        Vector3 originalPosition = transform.position;
        Vector3 shadowPosition = shadow.position;

        Collider2D[] results = new Collider2D[10];
        int count = shadow.GetComponent<Collider2D>().Overlap(new ContactFilter2D().NoFilter(), results);

        foreach (var coll in results)
        {
            if (coll != null && coll.gameObject != gameObject && !coll.isTrigger)
            {
                transform.position = originalPosition;
                GetComponent<playerLife>()?.SendMessage("jeSuisMouru");
                return;
            }
        }

        transform.position = shadowPosition;

        // Corriger direction de déplacement après swap
        Vector2 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(-velocity.x, velocity.y);

        shadow.position = originalPosition;
    }

    void OnDrawGizmos()
    {
        if (monColl == null) return;

        float checkDistance = 0.55f;
        Vector2 position = transform.position;
        float colliderHeight = monColl.size.y * 0.5f;

        Vector2 feetPosition = position + Vector2.down * (colliderHeight - 0.5f);
        Vector2 midPosition = position + Vector2.down * ((colliderHeight * 0.5f) - 0.25f);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(position, position + Vector2.left * checkDistance);
        Gizmos.DrawLine(position, position + Vector2.right * checkDistance);
        Gizmos.DrawLine(feetPosition, feetPosition + Vector2.left * checkDistance);
        Gizmos.DrawLine(feetPosition, feetPosition + Vector2.right * checkDistance);
        Gizmos.DrawLine(midPosition, midPosition + Vector2.left * checkDistance);
        Gizmos.DrawLine(midPosition, midPosition + Vector2.right * checkDistance);

        Gizmos.color = Color.green;

        Vector2 circleCenter = (Vector2)transform.position + Vector2.up * (monColl.offset.y + rayonDetection * 0.8f - (monColl.size.y / 2));
        Gizmos.DrawWireSphere(circleCenter, monColl.size.x * 0.20f);
    }
}