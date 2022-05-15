using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimationController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    private Movement2d movement2D;

    [SerializeField]
    private SpriteRenderer sp;

    private bool canSetAnimation;
    public bool CanSetAnimation { get { return canSetAnimation; } set { canSetAnimation = value; } }

    private void FixedUpdate()
    {
        Vector2 velocity = movement2D.GetVelocity();
        bool falling = movement2D.IsFalling;

        if (velocity.x < -0.1)
        {
            sp.flipX = true;
        }

        if (velocity.x > 0.1)
        {
            sp.flipX = false;
        }

        if (canSetAnimation)
        {
            animator.SetBool("Falling", falling);
            animator.SetBool("Grounded", movement2D.IsGrounded);
            animator.SetBool("Jumping", movement2D.IsJumping);
            animator.SetBool("Walking", velocity.normalized.x != 0);
        }
    }

    public void SetBool(string tag, bool value)
    {
        animator.SetBool(tag, value);
    }
}
