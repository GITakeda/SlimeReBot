using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : MovementController
{
    [SerializeField]
    private Animator animator;

    private int flipped = 1;
    private bool canMove = true;

    public bool IsGrounded { get { return Movement2d.CheckSphere(groundCheck, groundLayer); } }

    public bool IsFacingWall { get { return Movement2d.CheckSphere(wallCheck, groundLayer, 0.1f);  } }

    public RaycastHit2D CanFall { get { return Physics2D.Raycast(walkingCheck.position, Vector2.down, 3f, groundLayer); } }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(walkingCheck.position, CanFall.point);
    }

    void Update()
    {
        animator.SetBool("Falling", rb.velocity.y < 0 && !IsGrounded);

        if (!IsGrounded || !canMove)
        {
            return;
        }

        if(IsFacingWall || !CanFall)
        {
            InvertDirection();
        }

        rb.velocity = new Vector2(speed * flipped, rb.velocity.y);

        transform.rotation.Normalize();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player p = collision.transform.GetComponent<Player>();
        
        if (p != null && p.CanBeHurt)
        {
            canMove = false;
            Collided(p);

            Invoke("OnGetUp", knockBackDuration);
        }
    }

    private void Collided(Player p)
    {
        Vector2 impulse;
        p.GotHit(1, knockBackDuration);

        impulse = new Vector2(rb.velocity.x, 5f);

        if((flipped == 1 &&
            p.transform.position.x > transform.position.x) ||
            (flipped == -1 && p.transform.position.x < transform.position.x))
        {
            InvertDirection();
        } 

        rb.velocity = impulse;
    }

    private void OnGetUp()
    {
        canMove = true;
    }

    private void InvertDirection()
    {
        if (transform.rotation.y == 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, Quaternion.identity.w);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, Quaternion.identity.w);
        }

        flipped *= -1;
    }
}
