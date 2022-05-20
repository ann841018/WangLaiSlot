using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevel_AnimationControl : StateMachineBehaviour
{
    [SerializeField] float ScaleSize;
    [SerializeField] float time;

    GameObject Panel_Close;
    GameObject LevelPic;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Panel_Close = GameObject.FindGameObjectWithTag("LevelPanel");
        LevelPic = GameObject.FindGameObjectWithTag("LevelPic");
        LevelPic.transform.localScale = new Vector2(1, 1);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LevelPic.transform.localScale = Vector2.Lerp(LevelPic.transform.localScale, new Vector2(ScaleSize, ScaleSize), Time.deltaTime * time);
        if(LevelPic.transform.localScale.x<=0.1f) Panel_Close.SetActive(false); 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
