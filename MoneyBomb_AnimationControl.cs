using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBomb_AnimationControl : StateMachineBehaviour
{
    [SerializeField] float ScaleSize;
    [SerializeField] int ReSetScaleSize;
    [SerializeField] float time;

    GameObject MoneyBomb;
    Button Panel_Close;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoneyBomb = GameObject.FindGameObjectWithTag("Money");
        Panel_Close = GameObject.FindGameObjectWithTag("PrizePanel").GetComponent<Button>();
        Panel_Close.enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (MoneyBomb.transform.localScale.x <= 1)
        {
            MoneyBomb.transform.localScale = Vector2.Lerp(MoneyBomb.transform.localScale, new Vector2(ScaleSize, ScaleSize), Time.deltaTime*time);
        }else if (MoneyBomb.transform.localScale.x > 1)
        {
            MoneyBomb.transform.localScale = Vector2.Lerp(MoneyBomb.transform.localScale, new Vector2(ScaleSize, ScaleSize), Time.deltaTime * time*MoneyBomb.transform.localScale.x);
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoneyBomb.transform.localScale = new Vector2(ReSetScaleSize, ReSetScaleSize);
        Panel_Close.enabled = true;

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {}
}
