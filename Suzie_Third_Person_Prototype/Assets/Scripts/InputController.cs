using UnityEngine;
using System.Collections;
using InControl;

public class InputController : MonoBehaviour {
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Public Variables
	//---------------------------------------------------------------------------------------------------------------------------
	
	public static InputController Instance;
	
	public static float h, v;
	
	// Input Events -------------------------------------------------------------
	public delegate void InputAction(InputEvent inputEvent);
	public static InputAction onInput;
	
	// Enum to use for chcking input events
	public enum InputEvent										
	{
		JumpUp,
		RecenterCam,
		CamBehind
	}
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Priate Variables
	//---------------------------------------------------------------------------------------------------------------------------
	
	private void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}
	
	private void Update ()
	{
		var inputDevice = InputManager.ActiveDevice;
		
		h = inputDevice.LeftStickX;
		v = inputDevice.LeftStickY;
		//print (v);
		
		// if pressed Y or pressed Space
		if (inputDevice.Action4.WasPressed)
		{
			onInput(InputEvent.JumpUp);	
			print ("Pressed Jump");
		}
		
		if (inputDevice.RightBumper.WasPressed)
		{
			RecenterCam();
		}

//		if (inputDevice.RightBumper.WasReleased)
//		{
//			onInput(InputEvent.CamBehind);	
//		}
		
	}

	private void RecenterCam()
	{
		onInput(InputEvent.RecenterCam);
		
		Util.Instance.DelayedAction(() => {
			print ("Run");
			onInput(InputEvent.CamBehind);
		}, 2f);
	}
		
	
}
