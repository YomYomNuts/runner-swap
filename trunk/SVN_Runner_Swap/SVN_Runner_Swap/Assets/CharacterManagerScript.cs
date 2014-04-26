using UnityEngine;
using System.Collections;

public class CharacterManagerScript : MonoBehaviour {

	private string CONTROLLER_JUMP;
	private string CONTROLLER_INCREASE_VELOCITY;
	private string CONTROLLER_DECREASE_VELOCITY;
	
	private float rightTriggerIdle;
	private float leftTriggerIdle;
	
	public float minVelocity;
	public float maxVelocity;
	public float acceleration;
	private float velocity;
	
	private bool isJumping;
	private bool isGoingUp;
	private bool isGoingDown;
	private float decreaseJump;
	private float jmp;
	private float jump;
	private float valueOfJmp;
	public float valueOfJump;
	public float maxHeighJump;
	
	// Use this for initialization
	void Start () {
		#if UNITY_STANDALONE_OSX
			CONTROLLER_JUMP = "Runner_OSX_Jump";
			CONTROLLER_INCREASE_VELOCITY = "Runner_OSX_IncreaseVelocity";
			CONTROLLER_DECREASE_VELOCITY = "Runner_OSX_DecreaseVelocity";
		#endif
		
		velocity = minVelocity;
		isJumping = false;
		decreaseJump = valueOfJump/maxHeighJump;
		
		rightTriggerIdle = 0.0f;
		leftTriggerIdle = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		// INCREASE SPEED
		if(Input.GetAxis(CONTROLLER_INCREASE_VELOCITY) != rightTriggerIdle)
		{
			rightTriggerIdle = -0.1f;
			
			float increase = Input.GetAxis(CONTROLLER_INCREASE_VELOCITY);
			
			if(increase < 0)
				increase *= -1;
			
			velocity += increase * acceleration;
			
			if(velocity > maxVelocity)
				velocity = maxVelocity;
		}
		
		// DECREASE SPEED
		if(Input.GetAxis(CONTROLLER_DECREASE_VELOCITY) != leftTriggerIdle)
		{
			leftTriggerIdle = -0.1f;
			
			float decrease = Input.GetAxis(CONTROLLER_DECREASE_VELOCITY);
			
			if(decrease < 0)
				decrease *= -1;
				
			velocity -= decrease * acceleration;
			
			if(velocity < minVelocity)
				velocity = minVelocity;
			
		}
		
		// JUMP
		if(Input.GetButtonDown(buttonName:CONTROLLER_JUMP))
		{
			if(!isJumping)
			{
				isJumping = true;
				isGoingUp = true;
				valueOfJmp = valueOfJump;
			}
		}
		
		if(isJumping)
		{
			if(isGoingUp)
			{
				jump += valueOfJmp;
				valueOfJmp -= decreaseJump;
				if(this.transform.position.y + jump >= maxHeighJump/2)
				{
					isGoingUp = false;
					jump = 0.0f;
				}
			}
				
		}
		
		this.transform.position = new Vector3(this.transform.position.x + (velocity * Time.deltaTime), this.transform.position.y + jump, this.transform.position.z);
	
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ground")
		{
			
			isJumping = false;
		}
	}
}
