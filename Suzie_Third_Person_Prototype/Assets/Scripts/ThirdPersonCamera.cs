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
	
	[Range(0.0f, 2.0f)]
	public float camSmoothDampTime = 0.1f;
	
	// Camera recentering
	[Header("Recenter Camera")]
	public float wideScreen = 0.2f;
	public float taegetingTime = 0.5f;
	
	// Camera States
	public enum CamState
	{
		Behind,
		FirstPerson,
		Target,
		Free
	}
	private CamState camState = CamState.Behind;
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Private Variables
	//---------------------------------------------------------------------------------------------------------------------------
		
	private Vector3 charOffset;
	private Transform follow;
	private Vector3 targetPos;
	private Vector3 lookDir; 		// Direction the cam will be looking in
	
	//temp vars
	private Vector3 velocityCamSmooth = Vector3.zero;
	
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Private Methods
	//---------------------------------------------------------------------------------------------------------------------------	
	
	private void Start()
	{
		follow = GameObject.FindGameObjectWithTag("Follow").transform;
	}
	
	private void LateUpdate ()
	{
		charOffset = follow.position + new Vector3(0f, distanceUp, 0f);
		
		//if (Input.GetAxis (
		lookDir = charOffset - transform.position;
		lookDir.y = 0.0f;
		lookDir.Normalize();

		targetPos = charOffset + follow.up * distanceUp - lookDir * distanceAway;
		
		CompensateForWalls(charOffset, ref targetPos);
		
		transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocityCamSmooth, camSmoothDampTime);
		
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
			print ("Recenter Cam");
	}

	
}
