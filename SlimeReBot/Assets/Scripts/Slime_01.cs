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

    private bool rolling = false;

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
        if (Input.GetAxis("Vertical") < 0)
        {   
            Rolling = true;
            animator.runtimeAnimatorController = rollingAnimation;
        } else if(Input.GetAxis("Vertical") > 0 || OnKnockBack)
        {
            Rolling = false;
            animator.runtimeAnimatorController = defaultAnimation;
        }

        if (Rolling)
        {
            animator.SetBool("Stopped", movement2D.GetVelocity().x == 0);
        }
    }

    public override void OnKnockBackEnd()
    {
        base.OnKnockBackEnd();
        this.CanMove = true && !Rolling;
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
