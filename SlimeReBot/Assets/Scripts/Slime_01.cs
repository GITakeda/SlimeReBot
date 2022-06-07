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

    private Vector2 boost;
    private Vector2 currentBoost;
    private bool rolling = false;
    private State state = State.normal;
    private int direction = 0;

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
            CheckSlope();
        }
    }

    protected override void HandleInput()
    {
        if (Input.GetAxis("Vertical") < 0 && !Rolling)
        {
            Rolling = true;
            animator.runtimeAnimatorController = rollingAnimation;
            state = State.normal;
            //TODO revisar o inicio do rolamento
            if (movement2D.IsGrounded)
            {
                movement2D.SetPosition(new Vector2(0, -0.06f));
            }
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

    private void CheckSlope()
    {
        Vector2 slope = movement2D.SlopeDelta;
        
        if (state == State.normal){
            direction = CheckDirection(slope);
        }

        switch (state)
        {
            case State.normal:
                if (direction != 0)
                {
                    state = State.beingBoosted;
                    direction = CheckDirection(slope);
                    CheckBoost(slope);
                }
                break;
            case State.beingBoosted:
                if (CheckDirection(slope) != 0 && CheckDirection(slope) != direction)
                {
                    state = State.wasBoosted;
                }
                else
                {
                    CheckBoost(slope);
                    CalculateVelocity();
                }
                break;
            case State.wasBoosted:
                direction = CheckDirection(slope);
                CalculateVelocity();
                if (direction == 0)
                {
                    state = State.normal;
                }
                break;
        }
    }

    private void CalculateVelocity()
    {
        movement2D.SetVelocity(movement2D.GetVelocity() + new Vector2(boost.x * slopeSpeed, boost.y));
    }

    private void CheckBoost(Vector2 slope)
    {
        boost = new Vector2(slope.x, slope.x);

        if (boost.y > 0)
        {
            boost.y *= -1;
        }
    }

    private int CheckDirection(Vector2 slope)
    {
        if (slope.x > 0.01f)
        {
            return 1;
        }
        else if (slope.x < -0.01f)
        {
            return -1;
        }

        return 0;
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
