using UnityEngine;
using System.Collections;

public class CharacterManagerScript : MonoBehaviour {
	
	private string CONTROLLER_JUMP;
	private string CONTROLLER_INCREASE_VELOCITY;
	private string CONTROLLER_DECREASE_VELOCITY;
	private string CONTROLLER_SWAP_GRAVITY;
	private string CONTROLLER_SHOOT;
	
	private float rightTriggerIdle;
	private float leftTriggerIdle;
	
	// MOVE
	public float defaultMinVelocity;
	public float defaultMaxVelocity;
	private float minVelocity;
	private float maxVelocity;
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

	private GameObject bullet;
	private bool scaleMaxVelocity;
	private float tempMaxVelocity;
	public GameObject camera;
	
	// Use this for initialization
	void Start () {
		#if UNITY_STANDALONE_OSX
			CONTROLLER_JUMP = "Runner_OSX_Jump";
			CONTROLLER_INCREASE_VELOCITY = "Runner_OSX_IncreaseVelocity";
			CONTROLLER_DECREASE_VELOCITY = "Runner_OSX_DecreaseVelocity";
			CONTROLLER_SWAP_GRAVITY = "Runner_OSX_SwapGravity";
			CONTROLLER_SHOOT = "Runner_OSX_Shoot";
		#endif

		minVelocity = defaultMinVelocity;
		maxVelocity = defaultMaxVelocity;
		velocity = minVelocity;

		isJumping = false;
		decreaseJump = valueOfJump/maxHeighJump;
		
		gravity = gravityWay.UP;
		
		rightTriggerIdle = 0.0f;
		leftTriggerIdle = 0.0f;

		scaleMaxVelocity = false;
	}
	
	// Update is called once per frame
	void Update () {
		// INCREASE SPEED
		if((Input.GetAxis(CONTROLLER_INCREASE_VELOCITY) != rightTriggerIdle) || (Input.GetKeyDown(KeyCode.D)))
		{
			rightTriggerIdle = -0.1f;
			
			float increase = Input.GetAxis(CONTROLLER_INCREASE_VELOCITY);
			
			if(Input.GetKeyDown(KeyCode.D))
			{
				rightTriggerIdle = 0.0f;
				increase = 0.7f;
			}
				
			if(increase < 0)
				increase *= -1;
			
			velocity += increase * acceleration;

			if(velocity > maxVelocity)
				velocity = maxVelocity;
		}
		
		// DECREASE SPEED
		if((Input.GetAxis(CONTROLLER_DECREASE_VELOCITY) != leftTriggerIdle) || (Input.GetKeyDown(KeyCode.Q)))
		{
			leftTriggerIdle = -0.1f;
			
			float decrease = Input.GetAxis(CONTROLLER_DECREASE_VELOCITY);
			
			if(Input.GetKeyDown(KeyCode.Q)) {
				leftTriggerIdle = 0.0f;
				decrease = 0.7f;
			}
				
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
				
				float boxColliderSize = this.gameObject.GetComponent<BoxCollider>().size.y / 2;
				
				this.transform.GetChild(0).transform.position = new Vector3(this.transform.position.x, this.transform.position.y + boxColliderSize, this.transform.position.z);
				
				this.transform.GetChild(0).transform.localRotation = Quaternion.Euler(180.0f, -90.0f, 0.0f);
			}
			else
			{
				gravity = gravityWay.UP;
				
				float boxColliderSize = this.gameObject.GetComponent<BoxCollider>().size.y / 2;
				
				this.transform.GetChild(0).transform.position = new Vector3(this.transform.position.x, this.transform.position.y - boxColliderSize, this.transform.position.z);
				
				this.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
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

		if (Input.GetButtonDown (buttonName: CONTROLLER_SHOOT)) {
			Vector3 popPosition = this.rigidbody.position;
			popPosition.x += 1f;
			bullet = (GameObject)Instantiate(Resources.Load("Bullet"));
			bullet.rigidbody.position = popPosition;
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
					
					if(this.transform.position.y + jump <= (-1) * maxHeighJump/2)
					{
						isGoingUp = false;
						jump = 0.0f;
					}
				}
			}				
		}

		if (scaleMaxVelocity) {			
			if(velocity > maxVelocity) {			
				velocity -= 15 * Time.deltaTime;
			} else {
				scaleMaxVelocity = false;
				velocity = maxVelocity;
			}
		}
		
		this.transform.position = new Vector3(this.transform.position.x + (velocity * Time.deltaTime), this.transform.position.y + jump, this.transform.position.z);
		camera.transform.position = new Vector3(camera.transform.position.x + (velocity * Time.deltaTime), camera.transform.position.y, camera.transform.position.z);
	}

	public void ResetMaxVelocity() {
		scaleMaxVelocity = false;
		maxVelocity = defaultMaxVelocity;
	}

	public void ChangeMaxVelocity(float value) {
		if (value > defaultMinVelocity) {
			scaleMaxVelocity = true;		
			maxVelocity = value;	
		}
	}

	public void ChangeVelocity(float value) {
		Debug.Log ("Change velocity");
		velocity += value;
		if(velocity < minVelocity)
			velocity = minVelocity;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ground")
		{
			isJumping = false;
		}
	}
}
