﻿using UnityEngine;
using System.Collections;

public class CharState : MonoBehaviour {

	//---------------------------------------------------------------------------
	// Public Variables
	//---------------------------------------------------------------------------	
	
	public enum State
	{
		Idle,
		Landing,
		IdleJumping,
		RunningJumping,
		Climbing,
		Swimming,
		Falling,
		Running,
		InCombat,
		InAir,
		Pivoting
	}

	//---------------------------------------------------------------------------------------------------------------------------
	// Private Variables
	//---------------------------------------------------------------------------------------------------------------------------

	private State state = State.InAir;

	// mechanim
	private Animator animator;
	private AnimatorTransitionInfo transInfo;
	private float locomotionPivotLT_ID = 0;
	private float locomotionPivotRT_ID = 0;

	//---------------------------------------------------------------------------------------------------------------------------
	// Public Methods
	//---------------------------------------------------------------------------------------------------------------------------	

	private void Awake ()
	{
		animator = GetComponent<Animator> ();
		locomotionPivotLT_ID = Animator.StringToHash ("Locomotion -> LocomotionPivotL");
		locomotionPivotRT_ID = Animator.StringToHash ("Locomotion -> LocomotionPivotR");
	}

	private void Update ()
	{
		transInfo = animator.GetAnimatorTransitionInfo (0);
	}

	public void SetState (State _state)
	{
		state = _state;
	}
	
	public State GetState ()
	{
		return state;
	}
	
	public bool Is (State _state)
	{
		return state == _state;
	}

	public bool IsInLocomotion()
	{
		return state == State.Running;
	}
	
	public bool IsJumping()
	{
		return (state == State.IdleJumping || state == State.RunningJumping || state == State.Falling);
	}

	public bool IsInPivot()
	{
		return state == State.Pivoting ||
			transInfo.nameHash == locomotionPivotLT_ID ||
			transInfo.nameHash == locomotionPivotRT_ID;
	}

	
}
