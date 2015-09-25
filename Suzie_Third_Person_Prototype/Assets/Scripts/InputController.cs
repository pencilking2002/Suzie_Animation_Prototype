using UnityEngine;
using System.Collections;
using InControl;

public class InputController : MonoBehaviour {
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Public Variables
	//---------------------------------------------------------------------------------------------------------------------------
	
	public static InputController Instance;
	
	public static float h, v, orbitH, orbitV;

	
	// Input Events -------------------------------------------------------------
	public delegate void InputAction(InputEvent inputEvent);
	public static InputAction onInput;
	
	// Enum to use for chcking input events
	public enum InputEvent										
	{
		JumpUp,
		RecenterCam,
		CamBehind,
		OrbitCamera,
		faceOppositeDirection
	}
	
	[HideInInspector]
	public float jumpKeyHoldDuration = 0.0f;
	
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Priate Variables
	//---------------------------------------------------------------------------------------------------------------------------
	private InputDevice inputDevice;

	private void Awake ()
	{
		if (Instance == null)
			Instance = this;
		//inputDevice = InputManager.ActiveDevice;
	}
	
	private void Update ()
	{
		inputDevice = InputManager.ActiveDevice;
		
		h = inputDevice.LeftStickX.Value;
		v = inputDevice.LeftStickY.Value;
		orbitH = inputDevice.RightStickX;
		orbitV = inputDevice.RightStickY;
		//print (inputDevice.RightStickX);
		
		//----------------------------------------------------------------------------------------------------------------------
		// Jumping
		//----------------------------------------------------------------------------------------------------------------------
		
		if (inputDevice.Action4.IsPressed)
			jumpKeyHoldDuration += Time.deltaTime;
		
		
		// if pressed Y or pressed Space
		if (inputDevice.Action4.WasReleased)
		{
			onInput(InputEvent.JumpUp);
			
			// Reset the jump key timer
			jumpKeyHoldDuration = 0.0f;	
		}

		//----------------------------------------------------------------------------------------------------------------------
		// Recenter Camera
		//----------------------------------------------------------------------------------------------------------------------
		
		if (inputDevice.RightBumper.WasReleased)
			onInput(InputEvent.RecenterCam);	
		
		//----------------------------------------------------------------------------------------------------------------------
		// Camera Orbiting
		//----------------------------------------------------------------------------------------------------------------------
		
		if (inputDevice.RightStickX.IsPressed)
			onInput(InputEvent.OrbitCamera);
		

		if (inputDevice.RightStickX.WasReleased)
			onInput(InputEvent.CamBehind);
			
		if (inputDevice.LeftStickY.WasPressed)
			onInput(InputEvent.faceOppositeDirection);
	}
		
	
}
