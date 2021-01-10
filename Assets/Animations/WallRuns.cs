using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRuns : StateMachineBehaviour
{
    WallRunAbility wj = null;
    Rigidbody rb = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wj = animator.gameObject.GetComponent<WallRunAbility>();
        rb = animator.gameObject.GetComponent<Rigidbody>();

        if (stateInfo.IsName("Wall Run Front"))   //if the wall run is on the front, the orientation of the player will need to be corrected because the collision physics will mess it up
            animator.SetBool("IsRunningForward", true);

        else
            animator.SetBool("IsRunningForward", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!wj.steps.isPlaying)
            wj.steps.Play();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wj.steps.Stop();

        animator.applyRootMotion = false;
        Physics.gravity = new Vector3(0, -16.81f, 0);

        rb.AddForce(Vector3.down * 2f);

        //push off wall effect; value differs based on wall run animations
        if (stateInfo.IsName("Wall Run Front"))
            rb.AddForce(wj.wall.GetContact(0).normal * 5f);                      

        else
            rb.AddForce(wj.wall.GetContact(0).normal * 15f);
    }

}
