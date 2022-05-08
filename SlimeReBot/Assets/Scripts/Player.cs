using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public Vector2 lastPosition;
    [SerializeField]
    protected Movement2d movement2D;
    [SerializeField]
    protected CapsuleCollider2D slimeCollider;
    [SerializeField]
    protected AudioSource hitSound;

    public static int fruitsCollected = 0;
    private bool canBeHurt = true;
    private bool canMove = true;
    private bool onKnockBack = false;

    public bool CanBeHurt { get { return canBeHurt; } set { canBeHurt = value; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool OnKnockBack { get { return onKnockBack; } set { onKnockBack = value; } }

    public void Update()
    {
        if (movement2D.IsGrounded)
        {
            lastPosition = transform.position;
        }

        if (transform.position.y < -25)
        {
            LastPoint();
        }

        HandleInput();
    }

    protected abstract void HandleInput();

    void LastPoint()
    {
        transform.position = lastPosition;
        movement2D.SetVelocity(new Vector2(0,0));
    }

    public void GotHit(int damage, bool isFromRight, float knockBackDuration)
    {
        hitSound.PlayDelayed(0);
        canMove = false;
        onKnockBack = true;
        movement2D.KnockBack(isFromRight);
        Invoke("OnKnockBackEnd", knockBackDuration);
    }

    public virtual void OnKnockBackEnd()
    {
        canMove = true;
        onKnockBack = false;
    }

    public virtual CapsuleCollider2D GetActiveCollider()
    {
        return slimeCollider;
    }
}
