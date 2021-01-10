using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallLoop : StateMachineBehaviour
{
    GroundDetector groundDetector = null;
    Rigidbody rb = null;
    WallRunAbility wj = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        groundDetector = animator.gameObject.GetComponentInChildren<GroundDetector>();
        rb = animator.gameObject.GetComponent<Rigidbody>();
        wj = animator.gameObject.GetComponent<WallRunAbility>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("IsRunningForward") == true)
            rb.transform.LookAt(wj.wall.transform);

        if (animator.GetBool("isGrounded") == true)
            animator.SetTrigger("land");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wj.invokeWallRunCancel();
        animator.SetBool("IsRunningForward", false);

        Physics.gravity = new Vector3(0, -9.81f, 0);
        rb.AddForce(Vector3.down * 10f);                                //cancel out bounce when landing
        animator.applyRootMotion = true;
    }

}
