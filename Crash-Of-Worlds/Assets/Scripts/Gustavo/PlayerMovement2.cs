using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 9f;
    bool isFacingRight = false;
    float jumpPower = 7f;
    bool isJumping = false;
    Rigidbody2D rb;
    [SerializeField] private float dashCooldown = 1f;
    private float timer = 1f;

    [SerializeField] private float dashSpeed = 10f;
    private bool canDash;
    private Vector2 dashDirection;

    [Header("Dashing")]
    TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // FlipSprite();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
        }

        timer += Time.deltaTime;
    }

    private void OnDash()
    {
        if (timer >= dashCooldown)
        {
            // determine mouse world position and set dash direction
            Vector3 mouseWorld = Camera.main != null ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;
            Vector2 toMouse = mouseWorld - transform.position;

            // fallback to facing direction if mouse is exactly on player
            if (toMouse.sqrMagnitude < 0.001f)
            {
                toMouse = new Vector2(transform.localScale.x >= 0 ? 1f : -1f, 0f);
            }

            dashDirection = toMouse.normalized;
            canDash = true;
            timer = 0;
        }
    }

    private IEnumerator PlayerDash()
    {
        float playerGravity = rb.gravityScale;
        rb.gravityScale = 0;

        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }

        // dash toward the mouse direction (full 2D vector)
        rb.linearVelocity = new Vector2(dashDirection.x * dashSpeed, dashDirection.y * dashSpeed);

        yield return new WaitForSeconds(0.5f);

        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

        rb.gravityScale = playerGravity;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (canDash)
        {
            StartCoroutine(PlayerDash());
            canDash = false;
        }
    }

    //void FlipSprite()
    //{
    //   if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
    //   {
    //        isFacingRight = !isFacingRight;
    //        Vector3 ls = transform.localScale;
    //        ls.x *= -1f;
    //        transform.localScale = ls;
    //    }
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }
}
