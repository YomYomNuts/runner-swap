using UnityEngine;
using System.Collections;

public class CharacterManagerScript : MonoBehaviour {

	private string CONTROLLER_JUMP;
	private string CONTROLLER_INCREASE_VELOCITY;
	private string CONTROLLER_DECREASE_VELOCITY;
	private string CONTROLLER_SWAP_GRAVITY;
	
	private float rightTriggerIdle;
	private float leftTriggerIdle;
	
	// MOVE
	public float minVelocity;
	public float maxVelocity;
	public float acceleration;
	private float velocity;
	
	// JUMP
	private bool isJumping;
	private bool isGoingUp;
	private bool isGoingDown;
	private float decreaseJump;
	private float jmp;
	private float jump;
	private float valueOfJmp;
	public float valueOfJump;
	public float maxHeighJump;
	
	// GRAVITY
	private enum gravityWay{UP, DOWN};
	private gravityWay gravity;
	
	// Use this for initialization
	void Start () {
		#if UNITY_STANDALONE_OSX
			CONTROLLER_JUMP = "Runner_OSX_Jump";
			CONTROLLER_INCREASE_VELOCITY = "Runner_OSX_IncreaseVelocity";
			CONTROLLER_DECREASE_VELOCITY = "Runner_OSX_DecreaseVelocity";
			CONTROLLER_SWAP_GRAVITY = "Runner_OSX_SwapGravity";
		#endif
		
		velocity = minVelocity;
		isJumping = false;
		decreaseJump = valueOfJump/maxHeighJump;
		
		gravity = gravityWay.UP;
		
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
		
		// SWAP GRAVITY
		if(Input.GetButtonDown(buttonName:CONTROLLER_SWAP_GRAVITY))
		{
			Physics.gravity *= -1;
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y * -1, this.transform.position.z);
			
			if(gravity == gravityWay.UP)
			{
				gravity = gravityWay.DOWN;
			}
			else
			{
				gravity = gravityWay.UP;
			}
			
			if(isJumping && !isGoingUp)
				this.GetComponent<Rigidbody>().rigidbody.velocity = new Vector3(this.GetComponent<Rigidbody>().rigidbody.velocity.x, this.GetComponent<Rigidbody>().rigidbody.velocity.y * (-1), this.GetComponent<Rigidbody>().rigidbody.velocity.z);
				
		}
		
		// JUMP
		if(Input.GetButtonDown(buttonName:CONTROLLER_JUMP))
		{
			if(!isJumping)
			{
				isJumping = true;
				isGoingUp = true;
				jump = 0.0f;
				
				valueOfJmp = valueOfJump;
			}
		}
		
		if(isJumping)
		{
			if(isGoingUp)
			{
				if(gravity == gravityWay.UP)
				{
					jump = Mathf.Abs(jump);
					jump += valueOfJmp;
					valueOfJmp -= decreaseJump;
					Debug.Log("up pos: "+ (this.transform.position.y + jump) + "  /  heigh: " + maxHeighJump/2);
					if(this.transform.position.y + jump >= maxHeighJump/2)
					{
						isGoingUp = false;
						jump = 0.0f;
					}
				}
				else
				{
					jump = Mathf.Abs (jump) * -1;
					jump -= valueOfJmp;
					valueOfJmp += decreaseJump;
					Debug.Log("do pos: "+ (this.transform.position.y - jump) + "  /  heigh: " + (-1) * maxHeighJump/2);
					if(this.transform.position.y - jump <= (-1) * maxHeighJump/2)
					{
						isGoingUp = false;
						jump = 0.0f;
					}
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
