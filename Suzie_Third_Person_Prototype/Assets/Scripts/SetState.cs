﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SetState : StateMachineBehaviour {
	
	 public CharState.State characterState;	// Which state to switch into on enter
	 
	 public enum StateEvent { Enter, Exit }
	 public StateEvent AnimationStateEvent;
	 
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (AnimationStateEvent == StateEvent.Enter)
			animator.GetComponent<CharState>().SetState(characterState);
	}
	
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (AnimationStateEvent == StateEvent.Exit)
			animator.GetComponent<CharState>().SetState(characterState);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
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
