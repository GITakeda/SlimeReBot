using UnityEngine;

public class Movement2d : MovementController
{
    [SerializeField]
    private MyAnimationController animator;
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
    private Vector2 lastSlopeCheckPos;
    #endregion

    private bool isJumping = false;

    private Collider2D isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    public bool IsFalling { get { return GetVelocity().y < 0 && !IsGrounded; } }
    public bool IsJumping { get { return isJumping && !IsFalling; } set { isJumping = value; } }
    public bool MaterialCanBeChanged { get { return materialCanBeChanged;  } set { materialCanBeChanged = value; }  }

    private void OnDrawGizmosSelected()
    {
        CheckSlope();
    }

    void Update()
    {
        if (IsGrounded && Input.GetAxis("Jump") <= 0)
        {
            IsJumping = false;
        }

        if (IsJumping && Input.GetAxis("Jump") <= 0)
        {
            StopJump();

        }
        else if (IsGrounded && !IsJumping && Input.GetAxis("Jump") > 0)
        {
            Jump(jumpHeight);
        }

        if (!p.CanMove)
        {
            horizontalInput = 0;
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector2 newVelocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        CheckGround(groundLayer, p.GetActiveCollider().size, transform.position);
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

        if (normalizeSlope && isOnSlope && IsGrounded && !IsJumping)
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
        IsJumping = true;
    }

    public void StopJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 4);
    }

    public void KnockBack()
    {
        rb.velocity = new Vector2(knockBack.x - rb.velocity.x , knockBack.y);
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

    public bool CheckGround(LayerMask groundLayer, Vector2 size, Vector2 pos)
    {
        Vector2 checkSize = size - new Vector2(0.04f, 0);
        Vector2 checkPos = pos - new Vector2(0, 0.1f);

        Collider2D hit = Physics2D.OverlapCapsule(checkPos,
            checkSize,
            CapsuleDirection2D.Horizontal,
            0f,
            groundLayer);

        if (hit)
        {
            float hitHeight = hit.ClosestPoint(transform.position).y - transform.position.y;

            float groundCheckOffset = p.GetActiveCollider().size.y * 4 / 10 * -1;

            if (hitHeight > groundCheckOffset)
            {
                isGrounded = null;
                return false;
            }
        }

        isGrounded = hit;

        return hit;
    }

    private void DefineSlopeCheckPosition(Collider2D groundHit, Vector2 pos)
    {
        Vector2 playerPosition = transform.position;

        if (!groundHit)
        {
            lastSlopeCheckPos = slopeCheckPos;
            slopeCheckPos = playerPosition - new Vector2(0, p.GetActiveCollider().size.y / 2);
            return;
        }

        Vector2 newSlopeCheckPos = groundHit.ClosestPoint(pos);

        if (!(GetVelocity().y < 0))
        {
            lastSlopeCheckPos = slopeCheckPos;
            slopeCheckPos = newSlopeCheckPos;
            return;
        }

        if (
            (playerPosition - newSlopeCheckPos).x < -0.01f || 
            (playerPosition - newSlopeCheckPos).x > 0.01f)
        {
            lastSlopeCheckPos = slopeCheckPos;
            slopeCheckPos = newSlopeCheckPos;
        }
        else if ((playerPosition - newSlopeCheckPos).y > 0.38f)
        {
            slopeCheckPos = lastSlopeCheckPos;
        }
    }

    public Vector2 CheckSlope()
    {
        DefineSlopeCheckPosition(isGrounded, transform.position);

        isOnSlope = false;
        Vector2 normalReturn = new Vector2();

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

        if (!(res.y > 0.01 || res.y < -0.01))
        {
            res.y = 0;
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
