using UnityEngine;

public class Movement2d : MovementController
{
    [SerializeField]
    private AnimationController animator;
    [SerializeField]
    private AudioSource jumpSound;
    [SerializeField]
    private Player p;

    public float JumpHeight { get { return jumpHeight; } set { jumpHeight = value; } }
    public Transform GroundCheck { get { return groundCheck; } set { groundCheck = value; } }

    private float horizontalInput;

    private Vector2 slopePrepNormalized;

    private bool isJumping;

    public bool IsGrounded { get { return CheckSphere(groundCheck, groundLayer);} }
    public bool IsFalling { get { return rb.velocity.y < 0 && !IsGrounded; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }

    private void Start()
    {
    }

    void Update()
    {
        if (IsJumping && Input.GetAxis("Jump") <= 0)
        {
            StopJump();
        }

        if (!p.CanMove)
        {
            horizontalInput = 0;
            return;
        }

        if (IsGrounded && !IsJumping && Input.GetAxis("Jump") > 0)
        {
            Jump(jumpHeight);
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector2 newVelocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (!p.CanMove)
        {
            Vector2 lerped = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), 0.01f);
            if (lerped.x > 0.01 || lerped.x < -0.01) {
                newVelocity = lerped;
            }
        }

        //if (CheckSlope() && !p.Rolling && IsGrounded && rb.velocity.y < jumpHeight)
        //{
        //    newVelocity = new Vector2(-horizontalInput * speed * slopePrepNormalized.x, -horizontalInput * speed * slopePrepNormalized.y);
        //}

        rb.velocity = newVelocity;
    }

    public void Jump(float jumpHeight)
    {
        jumpSound.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        animator.SetJumping();
        IsJumping = true;
    }

    public void StopJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 4);
        IsJumping = false;
    }

    public void GroundPound()
    {
        horizontalInput = 0;
        rb.velocity = new Vector2(0, -jumpHeight * 2);
    }

    public void KnockBack(bool isFromRight)
    {
        int direction = isFromRight ? -1 : 1;
        rb.velocity = new Vector2(knockBack.x * direction - rb.velocity.x , knockBack.y);
    }

    public void ResetPosition()
    {
        rb.velocity = new Vector2(0, 0); 
    }

    public Vector2 GetVelocity()
    {
        Vector2 res = rb.velocity;

        if(!(res.x > 0.01 || res.x < -0.01))
        {
            res.x = 0;
        }

        return res;
    }

    public Vector2 GetPosition()
    {
        return this.transform.position;
    }

    public void ResetVelocity()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public float GetHorizontalInput()
    {
        return horizontalInput;
    }

    public static bool CheckSphere(Transform groundCheck, LayerMask groundLayer, float size = 0.05f) {
        return Physics2D.OverlapCircle(groundCheck.position, size, groundLayer);
    }

    public bool CheckSlope()
    {
        Vector2 checkPos = this.transform.position - new Vector3(0, p.GetActiveCollider().size.y / 2);
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, 0.5f, groundLayer);

        Vector2 perpendicular;
        float downAngle = 0;

        if (hit)
        {
            perpendicular = Vector2.Perpendicular(hit.normal);
            downAngle = Vector2.Angle(hit.normal, Vector2.up);

            Debug.DrawRay(hit.point, hit.normal, Color.green);

            slopePrepNormalized = perpendicular.normalized;
        }

        return downAngle != 0;
    }

    public void SetVelocity(Vector2 v)
    {
        rb.velocity = v;
    }
}
