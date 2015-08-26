using UnityEngine;
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
		InAir
	}
	
	private State state = State.InAir;
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Private Methods
	//---------------------------------------------------------------------------------------------------------------------------	
	

	//---------------------------------------------------------------------------------------------------------------------------
	// Public Methods
	//---------------------------------------------------------------------------------------------------------------------------	
	
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
	
}
