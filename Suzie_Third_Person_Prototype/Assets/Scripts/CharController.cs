using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Public Variables
	//---------------------------------------------------------------------------------------------------------------------------	
	
	public float DirectionDampTime = 0.25f;
	public float directionSpeed = 3.0f;
	
	public float jumpForce = 10f;
	private float maxJumpForce;
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Private Variables
	//---------------------------------------------------------------------------------------------------------------------------	
	
	
	private float totalJump;			// Total amount of jump to add to the character
		
	private Animator animator;
	private Rigidbody rb;
	
	private float speed = 0.0f;
	private float direction = 0.0f;
	private Transform cam;
	
	// Temp vars
	//private Vector3 rootDirection;
	private Vector3 stickDirection;
	private Vector3 cameraDirection;
	private Vector3 moveDirection;
	private Vector3 axisSign;
	private float angleRootToMove;
	Quaternion referentialShift;
	
	private float rotationDegreePerSecond = 120f;
	private AnimatorStateInfo stateInfo;

	CharState charState;
	
	//---------------------------------------------------------------------------------------------------------------------------
	// Private Methods
	//---------------------------------------------------------------------------------------------------------------------------	
	
	private void Awake ()
	{
		cam = Camera.main.transform;
		animator = GetComponent<Animator>();
		
		charState = GetComponent<CharState>();
		rb = GetComponent<Rigidbody>();
		
		maxJumpForce = jumpForce + 20f;
	}
	
	private void Update ()
	{

		StickToWorldSpace (ref direction, ref speed);
		
		animator.SetFloat ("Speed", speed);
		animator.SetFloat ("Direction", InputController.h == 0 ? 0 : direction, DirectionDampTime, Time.deltaTime);
		
//		if (charState.IsJumping() && rb.velocity.y < 0.0f && Physics.Raycast(transform.position, Vector3.down , 0.1f))
//		{
//			animator.SetTrigger("JumpDown");
//		}
			
		
	}
	
	// Hook on to Input event
	private void OnEnable () { InputController.onInput += Jump; }
	private void OnDisable () { InputController.onInput -= Jump; }
	
	// Trigger the jump animation and disable root motion
	public void Jump (InputController.InputEvent _event)
	{
		if (_event == InputController.InputEvent.JumpUp)
		{
			//print ("Jump");
			
			JumpUpAnim();
			
			//ApplyRootMotion(false);
			
			totalJump = Mathf.Clamp(jumpForce + jumpForce * InputController.Instance.jumpKeyHoldDuration, 0, maxJumpForce);
			
			rb.AddForce(new Vector3(0, totalJump, 0), ForceMode.Impulse);
		}
		
	}
	
	// Trigger the jump up animation
	private void JumpUpAnim()
	{
		if (speed == 0 )
		{
			//charState.SetState(CharState.State.IdleJumping);
	 		animator.SetTrigger("IdleJump");
	 		print ("Triggered idle jump");
	 	}
	 	else
	 	{
			//charState.SetState(CharState.State.RunningJumping);
			animator.SetTrigger("RunningJump");
		}
	}
	
	private void FixedUpdate ()
	{
		if (charState.Is (CharState.State.Running) && ((direction >= 0 && InputController.h >= 0) || (direction < 0 && InputController.h < 0 )))
		{
			//print ("In locomotion");
			Vector3 rotationAmount = Vector3.Lerp (Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (InputController.h < 0f ? -1f : 1f), 0f), Mathf.Abs (InputController.h));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			transform.rotation = (transform.rotation * deltaRotation);
		}
	}
	
	// Handle collisding with the floor
//	private void OnCollisionEnter(Collision coll)
//	{
//		if (Util.IsGround(coll.collider.gameObject))
//		{
//			if (InputController.v == 0)
//				charState.SetState (CharState.State.Idle);
//			else
//				charState.SetState (CharState.State.Running);
//		}
//	}
	
	//---------------------------------------------------------------------------------------------------------------------------
	// public Methods
	//---------------------------------------------------------------------------------------------------------------------------

	
	public void StickToWorldSpace(ref float directionOut, ref float speedOut)
	{
		//rootDirection = root.forward;
		
		stickDirection = new Vector3(InputController.h, 0, InputController.v);
		
		speedOut = stickDirection.sqrMagnitude;
		
		cameraDirection = cam.forward;
		cameraDirection.y = 0.0f;
		
		referentialShift = Quaternion.FromToRotation (Vector3.forward, cameraDirection);
		
		moveDirection = referentialShift * stickDirection;
		axisSign = Vector3.Cross (moveDirection, transform.forward);
		
		angleRootToMove = Vector3.Angle (transform.forward, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		
		angleRootToMove /= 180;
		
		directionOut = angleRootToMove * directionSpeed;
		
	}
	
	// Enable 
	public void ApplyRootMotion ()
	{
		animator.applyRootMotion = true;
	}
	
	private void OnCollisionEnter (Collision coll)
	{
		if (coll.collider.gameObject.layer == 8 && charState.IsJumping())
		{
			ContactPoint contact = coll.contacts[0];
			
			if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
			{
				animator.SetTrigger("Land");
				print ("Collision from below. Jump Down");
			}
		}
	}
	
	
	
}
