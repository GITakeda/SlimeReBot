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
    [SerializeField]
    private float topSpeed;

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

        int curDirection = CheckDirection(slope);

        switch (state)
        {
            case State.normal:
                if (curDirection != 0)
                {
                    state = State.beingBoosted;
                    direction = curDirection;
                    CheckBoost(slope);
                }
                break;
            case State.beingBoosted:
                if (curDirection != 0 && curDirection != direction)
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
                CalculateVelocity();
                if (curDirection == 0)
                {
                    state = State.normal;
                }
                break;
        }
    }

    private void CalculateVelocity()
    {
        Vector2 newVelocity = movement2D.GetVelocity() + new Vector2(currentBoost.x * slopeSpeed, currentBoost.y);

        if(Mathf.Abs(newVelocity.x) > topSpeed)
        {
            newVelocity.x = topSpeed * direction;
        }

        if(state == State.wasBoosted)
        {
            newVelocity.y = movement2D.GetVelocity().y;
        }

        movement2D.SetVelocity(newVelocity);
    }

    private void CheckBoost(Vector2 slope)
    {
        boost = new Vector2(slope.x, slope.x);

        if (boost.y > 0.1)
        {
            boost.y *= -1;
        }

        if(movement2D.GetVelocity().y > 0.1)
        {
            boost.y = 0;
            currentBoost.y = 0;
        }

        if (boost.x > 0.1 || boost.x < -0.1)
        {
            currentBoost = boost;
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
