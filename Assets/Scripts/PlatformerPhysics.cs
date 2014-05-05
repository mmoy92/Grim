using UnityEngine;
using System.Collections;

public class PlatformerPhysics : MonoBehaviour
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//NOTE: changing these numbers will only change the default values of the script, not the values of an object the script is already applied to
	//If you already applied the script to an object, you have to change the values in the inspector to get an actual change
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Configurable variables regarding movement
	public float accelerationWalking	= 35;		//Character acceleration while walking
	public float accelerationSprinting	= 60;		//Character acceleration while sprinting
	public float maxSpeedWalking		= 15;		//Maximum character speed while walking
	public float moveFriction			= 0.9f;		//Friction multiplier if the character is on ground and no moving buttons are pressed
	public float speedToStopAt			= 5.0f;		//If the character's speed falls below this while being on the ground, the character stops
	public float airFriction			= 0.98f;	//Air friction is always applied to the character
	public float maxGroundWalkingAngle	= 30.0f;	//Maximum angle the ground can be for the character to still be able to jump off and not slide down

	//Configurable variables regarding jumping
	public float jumpVelocity			= 12;		//Velocity while jumping
	public int jumpTimeFrames			= 15;		//Amount of frames the jump can be held, the player can release the jump button earlier for a lower jump
	public bool canDoubleJump			= false;	//Whether the character can double jump or not
	public bool canWallJump				= true;		//Whether the character can do a wall jump or not

	public bool hasEvilDash				= true;
	public bool hasGoodDash 			= false;

	public bool hasEvilAttack			= false;
	public bool hasGoodAttack			= true;

	public float goodDashSpeed = 30.0f;
	public float evilDashSpeed = 10.0f;

	public float wallJumpVelocity		= 15;		//Sideways velocity when doing a walljump
	public float wallStickyness			= 0.5f;		//Amount of seconds the player has to move away from a wall to let go of it. The idea behind this is that players can press the opposite direction to prepare for a walljump without immediately letting go of the wall
	public float gravityMultiplier		= 3.5f;		//Amount of gravity applied to the character compared to the rest of the physics world

	public Object dustEffect;


	//Private variables, no need to configure these
	bool mOnGround						= false;	//Are we on the ground or not?
	bool mSprinting						= false;	//Are we sprinting or not?
	bool mDashing						= false; 	//Are we dashing or not?
	Vector3 mGroundDirection			= Vector3.right; //The direction of the ground we are standing on

	bool mCanDash						= true;

	bool mInJump						= false;	//Are we in a jump
	bool mJumpPressed					= false;	//Was the jump button still pressed this frame?
	bool mSecondJumpLeft				= true;		//Do we have our second jump left (for double jump)
	int mJumpFramesLeft					= 0;		//Amount of frames left that we can hold the jump button to jump higher

	bool mOnWall						= false;	//Are we on a wall? (being on the ground while against a wall will keep this false)
	bool mWallIsOnRightSide				= false;	//Is the wall on the right side of us?
	float mWallStickynessLeft			= 0;		//Amount of seconds left the player needs to press the opposite direction of the wall to let go of it

	float mStoppingForce				= 0;		//This variable holds whether or not a player was moving this frame, if a player doesnt press move, the character will slowly stop
	bool mGoingRight					= true;		//Are we going to the right?
	
	float mCharacterHeight;							//Character bounding box height
	float mCharacterWidth;							//Character bounding box width

	Vector3 mStartPosition;							//Position used for respawning

	float origColliderCenterY;						//Original sizes of collision box
	float origColliderSizeY;

	float rawAxis_H						= 0.0f;
	float dustTimer						= 0.0f;
	
	PlayerCombat combatComponent;
	grimInfo infoComponent;

	public void Start () 
	{
		//do some checks to make sure we have the required components
		if (!rigidbody)
		{
			Debug.LogError("The PlatformerPhysics component requires a rigidbody.");
			enabled = false;
		}

		if (!collider || collider.GetType() != typeof(BoxCollider))
		{
			Debug.LogError("The PlatformerPhysics component requires a box collider.");
			enabled = false;
		}

		if (rigidbody.useGravity)
			Debug.LogWarning("You should turn off 'use gravity' on the platformer rigidbody. This will give strange behaviour.");

		combatComponent = GetComponent<PlayerCombat>();
		infoComponent = GetComponent<grimInfo> ();

		mStartPosition = transform.position;
		RecalcBounds();
		origColliderCenterY = ((BoxCollider)collider).center.y;
		origColliderSizeY = ((BoxCollider)collider).size.y;
	}

	public void Reset() //resets all private variables to their starting values
	{
		mOnGround = false;
		mSprinting = false;
		mGroundDirection = Vector3.right;
		mInJump = false;
		mJumpPressed = false;
		mSecondJumpLeft = true;
		mJumpFramesLeft = 0;
		mOnWall = false;
		mWallIsOnRightSide = false;
		mStoppingForce = 0;
		transform.position = mStartPosition;
		rigidbody.velocity = Vector3.zero;
		mGoingRight = true;
		mDashing = false;
		mCanDash = true;
	}


    //Player update
	void FixedUpdate () 
	{
		if(canWallJump)
			UpdateWallInfo();		//Check the sides to see if we are against a wall
		UpdateGroundInfo();			//Check below to see if we are on the ground

		UpdateJumping();
		ApplyGravity();
		ApplyMovementFriction();
	}


    //Called when the player presses a walking button (direction -1.0f is full left, and 1.0f is full right)
	public void Walk(float direction) 
	{
		rawAxis_H = direction;
		//See if we need to stick to a wall
		if (mOnWall && mWallStickynessLeft > 0)
		{
			//remove time from the stickyness left
			if ((mWallIsOnRightSide && direction < 0) ||
				(!mWallIsOnRightSide && direction > 0))
			{
				mWallStickynessLeft -= Time.fixedDeltaTime;
			}

			//see if we just released the wall
			if (mWallStickynessLeft <= 0)
			{

				SendAnimMessage("ReleasedWall");
			}

			return;
		}

		if (!combatComponent.IsAttacking() && !mDashing)  //Do not allow horizontal walking movement during attacks
		{
			//get an acceleration amount
			float accel = accelerationWalking;

			//apply actual force 
			rigidbody.AddForce (mGroundDirection * direction * accel, ForceMode.Acceleration);

			mStoppingForce = 1 - Mathf.Abs (direction);


			if (direction < 0 && mGoingRight) {
					mGoingRight = false;
					SendAnimMessage ("GoLeft");
			}
			if (direction > 0 && !mGoingRight) {
					mGoingRight = true;
					SendAnimMessage ("GoRight");
			}
			if(mOnGround && rawAxis_H != 0)
			{
				/*if(dustTimer > 0){
					dustTimer -= Time.deltaTime;
				} else {
					dustTimer = 0.2f;

					Vector3 spawnLoc = gameObject.transform.position;
					spawnLoc.y += 1;
					Instantiate (dustEffect,spawnLoc, Quaternion.Euler (new Vector3 (0, 0, 0)));
				}*/
			}
		}
	}


    //Called when the player holds down the jump key
	public void Jump() 
	{
		mJumpPressed = true;

		//See if we can start a jump
		if (mJumpFramesLeft == 0 && !mInJump  && !mDashing)
		{
			if (!mOnGround && mSecondJumpLeft && canDoubleJump) //Second jump
			{
				mSecondJumpLeft = false;

				mJumpFramesLeft = jumpTimeFrames;
				mInJump = true;

				spawnDust();
                SendAnimMessage("StartedJump");
			}

			if (mOnGround || mOnWall) //First jump
			{
				mSecondJumpLeft = true;

				mJumpFramesLeft = jumpTimeFrames;
				mInJump = true;

				if (mOnWall) //A wall jump needs sideways velocity as well
				{
					if (mWallIsOnRightSide)
						rigidbody.velocity += wallJumpVelocity * Vector3.left;
					else
						rigidbody.velocity += wallJumpVelocity * Vector3.right;

                    SendAnimMessage("StartedWallJump");
				}
				else
				{
					spawnDust();
                    SendAnimMessage("StartedJump");
				}
			}
		}

		//Check if we are in the middle of a jump
		if(mJumpFramesLeft != 0 && !mOnWall)
		{
			Vector3 vel = rigidbody.velocity;
			vel.y = jumpVelocity;
			rigidbody.velocity = vel;
		}
	}
	//Called when the player presses the attack button
	public void Attack() 
	{
		if (mDashing) {
				mDashing = false;
				mCanDash = false;
		}
		combatComponent.Attack(rigidbody.velocity, mGoingRight);
	}
	//Called when the player presses the sp. attack button
	public void SpecialAttack() 
	{
		if (hasEvilAttack || hasGoodAttack) {
			if (mDashing) {
					mDashing = false;
					mCanDash = false;
			}
			if(hasEvilAttack){
				combatComponent.EvilAttack (rigidbody.velocity, mGoingRight);
			}else {
			
				combatComponent.GoodAttack (rigidbody.velocity, mGoingRight);
			}
		}
	}
	


	//Called when the player presses the dash button
	public void StartDash() 
	{
		if (mCanDash) {
			mCanDash = false;
			mDashing = true;
			if(hasEvilDash){
				//rigidbody.AddForce (mGroundDirection * 500, ForceMode.VelocityChange);
				Vector3 vel = rigidbody.velocity;
				vel.y = 0;
				vel.x = mGoingRight ? evilDashSpeed : -evilDashSpeed;
				rigidbody.velocity = vel;
				SendAnimMessage ("StartedEvilDash");
			} else if (hasGoodDash){
				infoComponent.setInvulnFor(1.0f);
				Vector3 vel = rigidbody.velocity;
				vel.y = 0;
				vel.x = mGoingRight ? goodDashSpeed : -goodDashSpeed;
				rigidbody.velocity = vel;
				SendAnimMessage ("StartedGoodDash");

			}

		}
	}
	public void EndDash()
	{
		mDashing = false;

		Vector3 vel = rigidbody.velocity;
		vel.x = 0;
		rigidbody.velocity  = vel;
		if (hasGoodDash) {
			SendAnimMessage("EndGoodDash");
		} else {
			SendAnimMessage("EndEvilDash");
		}
		SendAnimMessage ("StartedJump");
	}
    //Called when the player presses the sprint button
	public void StartSprint() 
	{
		mSprinting = true;
		SendAnimMessage("StartedSprinting");
	}

    //Called when the player releases the sprint button
	public void StopSprint() 
	{
		mSprinting = false;
        SendAnimMessage("StoppedSprinting");
	}


	void ApplyGravity()
	{
		if (!mOnGround && !mDashing &&!mOnWall) //basic gravity, only applied when we are not on the ground
		{
			rigidbody.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
		}


	}
	


	void UpdateJumping()
	{
		if (!mJumpPressed && mInJump) //see if we released the jump button
		{
			mJumpFramesLeft = 0;
			mInJump = false;
		}
		mJumpPressed = false;

		if (mJumpFramesLeft != 0)
			mJumpFramesLeft--;
	}

	void ApplyMovementFriction()
	{

		Vector3 velocity = rigidbody.velocity;

		//Apply ground friction
		if (mOnGround && mStoppingForce > 0.0f)
		{
			Vector3 velocityInGroundDir = Vector3.Dot(velocity, mGroundDirection) * mGroundDirection; //project velocity on ground direction
			Vector3 newVelocityInGroundDir = velocityInGroundDir * Mathf.Lerp(1.0f, moveFriction, mStoppingForce); //apply ground friction on velocity
			velocity -= (velocityInGroundDir - newVelocityInGroundDir); //apply to actual velocity
		}

		//Apply air friction
		velocity *= airFriction;

		float absSpeed = Mathf.Abs(velocity.x);

		//Apply maximum speed
		float maxSpeed = maxSpeedWalking;

		if (!mDashing) {
			if (absSpeed > maxSpeed ){
				velocity.x *= maxSpeed / absSpeed;
			}
		}
		//Apply minimum speed
		if (absSpeed < speedToStopAt && mStoppingForce == 1.0f)
			velocity.x = 0;

		//Apply final velicty to rigid body
		rigidbody.velocity = velocity;

		mStoppingForce = 1.0f; //if no walking is done this frame, the character will start stopping next frame
	}


	void UpdateGroundInfo()
	{
		//We will trace 2 rays from the front and back of the character both downwards, to see if there is any ground under the character's feet

		float epsilon = 0.05f; //the amount the ray will trace below the feet of the character to check if there is ground
		float extraHeight = mCharacterHeight * 0.75f;
		float halfPlayerWidth = mCharacterWidth * 0.49f;

		//Origins of the ray
		Vector3 origin1 = GetBottomCenter() + Vector3.right * halfPlayerWidth + Vector3.up * extraHeight;
		Vector3 origin2 = GetBottomCenter() + Vector3.left * halfPlayerWidth + Vector3.up * extraHeight;
		Vector3 direction = Vector3.down;
		RaycastHit hit;

		//Actual physic traces
		if (Physics.Raycast(origin1, direction, out hit) && (hit.distance < extraHeight + epsilon))
			HitGround(origin1, hit);
		else if (Physics.Raycast(origin2, direction, out hit) && (hit.distance < extraHeight + epsilon))
			HitGround(origin2, hit);
		else
		{
			mOnGround = false; //We didnt hit anything, so we are in the air
			mGroundDirection = Vector3.right;
		}
	}

	void HitGround(Vector3 origin, RaycastHit hit)
	{
		//Calculate the angle of the ground we are standing on based on the normal
		mGroundDirection = new Vector3(hit.normal.y, -hit.normal.x, 0);
		float groundAngle = Vector3.Angle(mGroundDirection, new Vector3(mGroundDirection.x, 0, 0));

		//Check if we can walk on this angle of ground
		if (groundAngle <= maxGroundWalkingAngle)
		{
			if(!mOnGround){
				if(rigidbody.velocity.y < -8)
					spawnDust();

				SendAnimMessage("LandedOnGround");
			}
			Debug.DrawLine(hit.point+Vector3.up, hit.point, Color.green);
			Debug.DrawLine(hit.point, hit.point + mGroundDirection, Color.magenta);
			mOnGround = true;
			mOnWall = false;
			mDashing = false;
			mCanDash = true;
		}
		else
		{
			Debug.DrawLine(hit.point, hit.point + mGroundDirection, Color.grey);
		}


	
		return;
	}


	void UpdateWallInfo()
	{
		//We will trace 2 rays from the center of the character to the left and right, to see if we are on any wall

		float epsilon = 0.05f;
		float halfPlayerWidth = mCharacterWidth * 0.5f;

		Vector3 origin = GetBottomCenter() + Vector3.up * mCharacterHeight * 0.5f;
		RaycastHit hit;


		//Key is pressing right and raycast going to the right
		// 
		if (rawAxis_H > 0 && Physics.Raycast(origin, Vector3.right, out hit))
		{
			if (hit.collider.tag == "Level" && hit.distance < halfPlayerWidth + epsilon && !mOnGround)
			{
				//remove collider penetration
				transform.position += Vector3.left * (halfPlayerWidth - hit.distance);

				HitWall(true);
				Debug.DrawLine(origin, hit.point, Color.yellow);
				return;
			}
		}

		//Key is pressing left and raycast going to the left
		if (rawAxis_H < 0 && Physics.Raycast(origin, Vector3.left, out hit))
		{
			if (hit.collider.tag == "Level" && hit.distance < halfPlayerWidth + epsilon && !mOnGround)
			{
				//remove collider penetration
				transform.position += Vector3.right * (halfPlayerWidth - hit.distance);

				HitWall(false);
				Debug.DrawLine(origin, hit.point, Color.yellow);
				return;
			}
		}

		//We hit no wall, but we used to be on the wall, this means we just released
		if (mOnWall)
		{
            SendAnimMessage("ReleasedWall");
		}

		mWallStickynessLeft = 0;
		mOnWall = false;
	}

	void HitWall(bool onRightSide)
	{
		mWallIsOnRightSide = onRightSide;
		mGoingRight = mWallIsOnRightSide;

		if (!mOnWall)
		{
			rigidbody.velocity = new Vector3(0, 0, 0); //Remove horizontal speed
			mWallStickynessLeft = wallStickyness;
			mOnWall = true;
            SendAnimMessage("LandedOnWall");
		}


		mOnWall = true;
		mDashing = false;
		mCanDash = true;
	}
	void spawnDust()
	{
		Vector3 spawnLoc = gameObject.transform.position;
		spawnLoc.y += 1;
		Instantiate (dustEffect,spawnLoc, Quaternion.Euler (new Vector3 (0, 0, 0)));
	}
    //send a message to all other scripts to trigger for example the animations
    void SendAnimMessage(string message)
    {
        SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }

	void RecalcBounds()
	{
		mCharacterHeight = collider.bounds.size.y;
		mCharacterWidth = collider.bounds.size.x;
	}

	public void SetRespawnPoint(Vector3 spawnPoint)
	{
		mStartPosition = spawnPoint;
	}

	public Vector3 GetBottomCenter()
	{
		return collider.bounds.center+collider.bounds.extents.y*Vector3.down;
	}
	
	//getter functions
	public bool IsWallOnRightSide() { return mWallIsOnRightSide; }
	public bool IsOnWall() { return mOnWall; }
	public bool IsOnGround() { return mOnGround; }
	public bool IsSprinting() { return mSprinting; }

}

