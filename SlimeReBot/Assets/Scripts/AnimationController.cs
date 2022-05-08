using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    private Movement2d movement2D;

    [SerializeField]
    private SpriteRenderer sp;

    void Update()
    {
        Vector2 velocity = movement2D.GetVelocity();
        bool falling = movement2D.IsFalling;

        animator.SetBool("Falling", falling);

        animator.SetBool("Jumping", movement2D.IsJumping && !movement2D.IsGrounded);
        
        animator.SetBool("Walking", velocity.normalized.x != 0);

        if (velocity.x < -0.1)
        {
            sp.flipX = true;
        }

        if (velocity.x > 0.1)
        {
            sp.flipX = false;
        }
    }

    public void SetJumping()
    {
        animator.SetBool("Jumping", true);
    }
}
