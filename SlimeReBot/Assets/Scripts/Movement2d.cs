using UnityEngine;

public class Movement2d : MovementController
{
    [SerializeField]
    private AnimationController animator;
    [SerializeField]
    private AudioSource jumpSound;
    [SerializeField]
    private Player p;
    [SerializeField]
    private PhysicsMaterial2D friction;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    protected LayerMask slopeMask;
    [SerializeField]
    private Vector2 groundCheckSize;

    public float JumpHeight { get { return jumpHeight; } set { jumpHeight = value; } }
    public Transform GroundCheck { get { return groundCheck; } set { groundCheck = value; } }

    private float horizontalInput;

    #region slopes
    private Vector2 slopePrepNormalized;
    private bool normalizeSlope = true;
    public bool NormalizeSlope { get { return normalizeSlope; } set { normalizeSlope = value; } }
    private bool materialCanBeChanged = true;
    private bool isOnSlope = false;
    private Vector2 slopeCheckPos;
    #endregion

    private Vector2 debugPoint;

    private bool isJumping = false;
    public bool IsGrounded { get { return CheckBox(groundCheck, groundLayer, p.GetActiveCollider().size - new Vector2(0.02f, 0), transform.position - new Vector3(0, 0.1f, 0));} }
    public bool IsFalling { get { return rb.velocity.y < 0 && !IsGrounded; } }
    public bool IsJumping { get { return isJumping && !IsFalling; } set { isJumping = value; } }
    public bool MaterialCanBeChanged { get { return materialCanBeChanged;  } set { materialCanBeChanged = value; }  }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector2(groundCheckSize.x, groundCheckSize.y));
        CheckSlope();
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
        CheckSlope();

        if (!p.CanMove)
        {
            Vector2 lerped = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), 0.01f);
            if (lerped.x > 0.01 || lerped.x < -0.01)
            {
                newVelocity = lerped;
            }

            rb.velocity = newVelocity;
            return;
        }

        if (isOnSlope && IsGrounded && !IsJumping)
        {
            newVelocity = new Vector2(-horizontalInput * speed * slopePrepNormalized.x, -horizontalInput * speed * slopePrepNormalized.y);
        }

        if (isOnSlope && horizontalInput == 0)
        {
            SetFriction();
        }

        if (horizontalInput != 0 || IsJumping)
        {
            SetNoFriction();
        }
        

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

    public void KnockBack(bool isFromRight)
    {
        int direction = isFromRight ? -1 : 1;
        rb.velocity = new Vector2(knockBack.x * direction - rb.velocity.x , knockBack.y);
    }

    public void ResetPosition()
    {
        rb.velocity = new Vector2(0, 0); 
    }

    public void ResetVelocity()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public static Collider2D CheckSphere(Transform groundCheck, LayerMask groundLayer, float size = 0.05f) {
        return Physics2D.OverlapCircle(groundCheck.position, size, groundLayer);
    }

    public bool CheckBox(Transform groundCheck, LayerMask groundLayer, Vector2 size, Vector2 pos)
    {
        Collider2D hit = Physics2D.OverlapCapsule(pos, new Vector2(size.x, size.y), CapsuleDirection2D.Horizontal, 0f, groundLayer);
        if (hit)
        {
            slopeCheckPos = hit.ClosestPoint(pos);
        }
        return hit;
    }

    public Vector2 CheckSlope()
    {
        isOnSlope = false;
        Vector2 normalReturn = new Vector2();

        //Vector2 checkPos = transform.position - new Vector3(0, p.GetActiveCollider().size.y / 2);

        RaycastHit2D hit = Physics2D.Raycast(slopeCheckPos, Vector2.down, 0.5f, slopeMask);

        Debug.DrawRay(hit.point, hit.normal, Color.green);

        if (hit)
        {
            slopePrepNormalized = Vector2.Perpendicular(hit.normal).normalized;

            isOnSlope = slopePrepNormalized.x != 0;

            normalReturn = hit.normal;

            Debug.DrawRay(hit.point, Vector2.Perpendicular(hit.normal).normalized, Color.black);
        }

        return normalReturn;
    }

    public void SetVelocity(Vector2 v)
    {
        rb.velocity = v;
    }

    public void SetFriction()
    {
        SetMaterial(friction);
    }

    public void SetNoFriction()
    {
        SetMaterial(noFriction);
    }

    public Vector2 GetVelocity()
    {
        Vector2 res = rb.velocity;

        if (!(res.x > 0.01 || res.x < -0.01))
        {
            res.x = 0;
        }

        return res;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public float GetHorizontalInput()
    {
        return horizontalInput;
    }

    private void SetMaterial(PhysicsMaterial2D material)
    {
        if (materialCanBeChanged)
        {
            rb.sharedMaterial = material;
        }
    }
}
