using UnityEngine;
using System.Collections;

public class CharState : MonoBehaviour {

	//---------------------------------------------------------------------------
	// Public Variables
	//---------------------------------------------------------------------------	
	
	public enum State
	{
		Idle,
		IdleJumping,
		RunningJumping  ,
		Climbing,
		Swimming,
		Falling,
		Running,
		InCombat,
		InAir
	}
	
	public State state = State.InAir;
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Private Methods
	//---------------------------------------------------------------------------------------------------------------------------	
	
	#if UNITY_EDITOR
	
	private void OnGUI ()
	{
		if (GameManager.debug)
		{
			GUI.Button(new Rect(30, 30, 170, 50), "Squirrel State: " + state.ToString());
		}
	}
	
	#endif
	
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
	
}
