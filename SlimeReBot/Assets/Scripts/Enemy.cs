using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float knockBackDuration;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Transform walkingCheck;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Animator animator;

    private int flipped = 1;
    private bool canMove = true;

    public bool IsGrounded { get { return Movement2d.CheckSphere(groundCheck, 0.05f, groundLayer); } }

    void Update()
    {
        animator.SetBool("Falling", rb.velocity.y < 0 && !IsGrounded);

        if (!IsGrounded || !canMove)
        {
            return;
        }

        if(!Movement2d.CheckSphere(walkingCheck, 0.05f, groundLayer) || Movement2d.CheckSphere(wallCheck, 0.05f, groundLayer))
        {
            InvertDirection();
        }

        rb.velocity = new Vector2(speed * flipped, rb.velocity.y);

        this.transform.rotation.Normalize();
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
        bool isFromRight;
        Vector2 impulse;

        isFromRight = p.transform.position.x < this.transform.position.x;
        p.GotHit(1, isFromRight, this.knockBackDuration);

        impulse = new Vector2(rb.velocity.x, 5f);

        if((flipped == 1 && p.transform.position.x > this.transform.position.x) || (flipped == -1 && p.transform.position.x < this.transform.position.x))
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
        if (this.transform.rotation.y == 0)
        {
            this.transform.rotation = new Quaternion(0, 180, 0, Quaternion.identity.w);
        }
        else
        {
            this.transform.rotation = new Quaternion(0, 0, 0, Quaternion.identity.w);
        }

        flipped *= -1;
    }
}
