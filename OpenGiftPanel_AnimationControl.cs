using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenGiftPanel_AnimationControl : StateMachineBehaviour
{
    GameObject GiftPanel;
    GameObject WheelSound;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GiftPanel = GameObject.FindGameObjectWithTag("Gift");
        WheelSound = GameObject.FindGameObjectWithTag("WheelSound");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GiftPanel.transform.localScale = new Vector2(1, 1);
        GiftPanel.GetComponent<AudioSource>().enabled = true;
        GiftPanel.GetComponentInChildren<Gift_AnimationControl>().enabled = true;
        WheelSound.GetComponent<AudioSource>().Stop();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {}
}
