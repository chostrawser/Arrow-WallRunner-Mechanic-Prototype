using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landings : StateMachineBehaviour
{
    playerController pc = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pc = animator.gameObject.GetComponent<playerController>(); //play landing sound cue
        pc.landCue.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(pc.gameObject.transform.rotation.x != 0 || pc.gameObject.transform.rotation.z != 0)      //correct player's rotation upon landing should it mess up
            pc.gameObject.transform.rotation = new Quaternion(0, pc.gameObject.transform.rotation.y, 0, pc.gameObject.transform.rotation.w);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
