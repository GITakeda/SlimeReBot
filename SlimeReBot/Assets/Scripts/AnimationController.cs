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
    private Player p;

    [SerializeField]
    private SpriteRenderer sp;

    void Start()
    {
    }

    void Update()
    {
        Vector2 velocity = movement2D.GetVelocity();
        float horizontalInput = Input.GetAxis("Horizontal");
        bool falling = movement2D.IsFalling;

        animator.SetBool("Falling", falling);

        if (falling)
        {
            animator.SetBool("Jumping", false);
        }

        animator.SetBool("Walking", velocity.normalized.x != 0);

        if (velocity.x < 0)
        {
            sp.flipX = true;
        }

        if (velocity.x > 0)
        {
            sp.flipX = false;
        }
    }

    public void SetJumping()
    {
        animator.SetBool("Jumping", true);
    }
}
