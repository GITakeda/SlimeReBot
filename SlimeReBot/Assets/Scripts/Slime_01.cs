using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_01 : Player
{
    [SerializeField]
    protected CapsuleCollider2D rollingCollider;
    [SerializeField]
    private RuntimeAnimatorController rollingAnimation;
    [SerializeField]
    private RuntimeAnimatorController defaultAnimation;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MyAnimationController myAnimationController;
    [SerializeField]
    private float slopeSpeed;

    private bool rolling = false;
    private State state = State.normal;

    enum State
    {
        normal,
        beingBoosted,
        wasBoosted
    }

    public bool Rolling { 
        get { 
            return rolling; 
        } 
        set {
            slimeCollider.enabled = !value;
            rollingCollider.enabled = value;
            rolling = value;

            if (OnKnockBack)
            {
                return;
            }

            CanMove = !value; 
        }
    }

    public void FixedUpdate()
    {
        myAnimationController.CanSetAnimation = !Rolling;

        if (Rolling)
        {
            animator.SetBool("RollingIdle", movement2D.GetVelocity().x == 0);
            CheckBoost();
        }
    }

    protected override void HandleInput()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            Rolling = true;
            animator.runtimeAnimatorController = rollingAnimation;
            state = State.normal;
            movement2D.NormalizeSlope = false;
            movement2D.SetNoFriction();
            movement2D.MaterialCanBeChanged = false;
        }
        else if (Input.GetAxis("Vertical") > 0 || OnKnockBack)
        {
            Rolling = false;
            animator.runtimeAnimatorController = defaultAnimation;
            movement2D.NormalizeSlope = true;
            movement2D.MaterialCanBeChanged = true;
        }
    }

    private void CheckBoost()
    {
        Vector2 slope = movement2D.CheckSlope();
        Vector2 boost = new Vector2(slope.x, slope.x);

        if (boost.y > 0)
        {
            boost.y *= -1;
        }

        if (slope.x != 0 && (state == State.normal || state == State.beingBoosted))
        {
            movement2D.SetVelocity(movement2D.GetVelocity() + boost);
            state = State.beingBoosted;
        }

        if (state == State.beingBoosted && slope.x == 0)
        {
            movement2D.SetVelocity(movement2D.GetVelocity() + new Vector2(10f * boost.normalized.x, 0));
            state = State.wasBoosted;
        }

        if (state == State.wasBoosted && movement2D.GetVelocity().x < 2.5f)
        {
            state = State.normal;
        }


        //switch (state)
        //{
        //    case State.normal:
        //        if (slope.x != 0)
        //        {
        //            state = State.beingBoosted;
        //            Debug.Log(State.beingBoosted.ToString());
        //        }
        //        break;
        //    case State.beingBoosted:
        //        if (slope.x == 0)
        //        {
        //            state = State.wasBoosted;
        //            return;
        //        }
        //        float lerped = Mathf.Lerp(movement2D.GetVelocity().x, 15f * boost.x, 0.01f);
        //        boost.x = lerped;
        //        movement2D.SetVelocity(movement2D.GetVelocity() + boost);
        //        break;
        //    case State.wasBoosted:
        //        if (velocity.x < 2.5f)
        //        {
        //            state = State.normal;
        //            return;
        //        }
        //        Vector2 lerpedVelocity = Vector2.Lerp(velocity, new Vector2(0, velocity.y), 0.01f);
        //        if (lerpedVelocity.x > 0.01f || lerpedVelocity.x < -0.01f)
        //        {
        //            movement2D.SetVelocity(lerpedVelocity);
        //        }
        //        break;
        //}
    }

    public override void OnKnockBackEnd()
    {
        base.OnKnockBackEnd();
        CanMove = !Rolling;
    }

    public override CapsuleCollider2D GetActiveCollider()
    {
        if (Rolling)
        {
            return rollingCollider;
        }

        return base.GetActiveCollider();
    }
}
