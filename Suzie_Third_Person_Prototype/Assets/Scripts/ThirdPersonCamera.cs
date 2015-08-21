using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour 
{
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Public Variables
	//---------------------------------------------------------------------------------------------------------------------------	
	
	public Vector3 camOffset = new Vector3(0, 5f, 5f); 		   // Used to position the camera a certain distance away from player
	public float distanceUp = 2.0f;
	public float distanceAway = 5.0f;
	
	public float smooth;
	
	[Range(0.0f, 100.0f)]
	public float camSmoothDampTime = 0.1f;

	[Range(0.0f, 100.0f)]
	public float camTargetSmoothDampTime = 0.1f;
	//private float originaCamSmoothTime;

	[Range(0.0f, 2.0f)]
	public float camSmoothDampTimeGoBack = 0.25f;	// damp time when the char is running into cam. This value is less so that we caa see the char us hes running into the cam

	[Range(0.0f, 2.0f)]
	public float lookDirDampTime = 0.1f;

	[Range(0.0f, 50f)]
	public float goBackLerpSpeed = 5f;
	// Camera recentering
	//[Header("Recenter Camera")]
	//public float wideScreen = 0.2f;
	//public float taegetingTime = 0.5f;
	
	// Camera States
	public enum CamState
	{
		Behind,
		FirstPerson,
		Target,
		Free
	}
	[HideInInspector]
	public CamState camState = CamState.Behind;
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Private Variables
	//---------------------------------------------------------------------------------------------------------------------------

	private float goBackVel;

	private float origCamSmoothDampTime;

	private Vector3 charOffset;
	private Transform follow;
	private Vector3 targetPos;
	private Vector3 lookDir; 		// Direction the cam will be looking in
	private Vector3 curLookDir;

	private CharState charState;

	//temp vars
	private Vector3 velocityCamSmooth = Vector3.zero;
	private Vector3 velocityLookDir = Vector3.zero;


	//---------------------------------------------------------------------------------------------------------------------------
	// Private Methods
	//---------------------------------------------------------------------------------------------------------------------------	
	
	private void Start()
	{
		//camTargetSmoothDampTime = camSmoothDampTime;
		origCamSmoothDampTime = camSmoothDampTime;

		// cache the original cam smoooth damp time
		//origCamSmoothDampTime = camSmoothDampTime;

		follow = GameObject.FindGameObjectWithTag("Follow").transform;
		curLookDir = follow.forward;

		// Get player's character state
		charState = follow.parent.GetComponent<CharState> ();
	}
	
	private void LateUpdate ()
	{
		charOffset = follow.position + new Vector3(0f, distanceUp, 0f);
		//camSmoothDampTime = origCamSmoothDampTime;

		//if (inputDevice.RightBumper.WasPressed)
		switch (camState) 
		{
			case CamState.Behind: 
	
				if (charState.IsInLocomotion())
				{
					print ("in locomotion");
					//lookDir = Vector3.Lerp (follow.right * (InputController.h < 0 ? 1f : -1f), follow.forward * (InputController.v < 0 ? -1f : 1f), Mathf.Abs(Vector3.Dot(transform.forward, follow.forward)) * goBackLerpSpeed * Time.deltaTime);
					curLookDir = Vector3.Normalize (charOffset - transform.position);
					lookDir.y = 0.0f;
					
					curLookDir = Vector3.SmoothDamp (curLookDir, lookDir, ref velocityLookDir, lookDirDampTime);
					
				}
				else
				{
					lookDir = charOffset - transform.position;
					lookDir.y = 0.0f;
					lookDir.Normalize ();
				}
				
				//targetPos = charOffset + follow.up * distanceUp - Vector3.Normalize (curLookDir) * distanceAway;
				
				break;

			case CamState.Target:
				
				//camSmoothDampTime = camTargetSmoothDampTime;
				curLookDir = follow.forward;
				lookDir = follow.forward;
				break;
		}


		targetPos = charOffset + follow.up * distanceUp - lookDir * distanceAway;

		CompensateForWalls(charOffset, ref targetPos);

		// if the char is runnign towards the camera, make the cam follow him with less damping
		//if (InputController.v < -0.5f)
		camSmoothDampTime = Mathf.SmoothDamp (camSmoothDampTime, InputController.v < -0.05 ? camSmoothDampTimeGoBack : origCamSmoothDampTime, ref goBackVel, goBackLerpSpeed * Time.deltaTime);
		//else
			//camSmoothDampTime = Mathf.SmoothDamp (camSmoothDampTime, origCamSmoothDampTime, ref goBackVel, goBackLerpSpeed * Time.deltaTime);

		transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocityCamSmooth, (camState == CamState.Target ? camTargetSmoothDampTime : camSmoothDampTime) * Time.deltaTime);
		
		transform.LookAt(charOffset);
		//var lookRot = Quaternion.LookRotation(charOffset - transform.position);
		//transform.rotation = Quaternion.Lerp(transform.rotation,lookRot,Time.deltaTime* 20f);
	}
	
	// Compensate the camera for wall collisions
	private void CompensateForWalls (Vector3 fromObject, ref Vector3 toTarget)
	{
		RaycastHit wallHit = new RaycastHit();
		if (Physics.Linecast (fromObject, toTarget, out wallHit))
		{
			print ("Wall hit");
			Debug.DrawRay (wallHit.point, Vector3.left, Color.red);
			toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
		}
	}
	
	// Hook on to Input event
	private void OnEnable () { InputController.onInput += RecenterCam; }
	private void OnDisable () { InputController.onInput -= RecenterCam; }
	
	private void RecenterCam (InputController.InputEvent _event)
	{
		if (_event == InputController.InputEvent.RecenterCam)
		{
			SetState (CamState.Target);
			print ("Recenter Cam");
		} else if (_event == InputController.InputEvent.CamBehind)
		{
		
			SetState(CamState.Behind);
			print ("Recenter Cam");		
		}
	}

	private void SetState (CamState _state)
	{
		camState = _state;
	}
	
}
