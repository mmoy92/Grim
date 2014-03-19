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
	public float maxSpeedSprinting		= 20;		//Maximum character speed while sprinting
	public float moveFriction			= 0.9f;		//Friction multiplier if the character is on ground and no moving buttons are pressed
	public float speedToStopAt			= 5.0f;		//If the character's speed falls below this while being on the ground, the character stops
	public float airFriction			= 0.98f;	//Air friction is always applied to the character
	public float maxGroundWalkingAngle	= 30.0f;	//Maximum angle the ground can be for the character to still be able to jump off and not slide down
	public float crouchColliderScale	= 0.5f;		//Multiplier to the Y-size of the collider when crouching
	public float crouchedAccelMultiplier= 0.1f;		//Maximum speed factor while crouched

	//Configurable variables regarding jumping
	public float jumpVelocity			= 12;		//Velocity while jumping
	public int jumpTimeFrames			= 15;		//Amount of frames the jump can be held, the player can release the jump button earlier for a lower jump
	public float crouchDownwardForce	= 20;		//Extra gravity added to the character if the crouch button is pressed
	public bool canDoubleJump			= false;	//Whether the character can double jump or not
	public bool canWallJump				= true;		//Whether the character can do a wall jump or not
	public float wallJumpVelocity		= 15;		//Sideways velocity when doing a walljump
	public float wallStickyness			= 0.5f;		//Amount of seconds the player has to move away from a wall to let go of it. The idea behind this is that players can press the opposite direction to prepare for a walljump without immediately letting go of the wall
	public float gravityMultiplier		= 3.5f;		//Amount of gravity applied to the character compared to the rest of the physics world



	//Private variables, no need to configure these
	bool mOnGround						= false;	//Are we on the ground or not?
	bool mSprinting						= false;	//Are we sprinting or not?
	bool mCrouching						= false;	//Are we crouching or not?
	bool mTryingToUncrouch				= false;	//Are we trying to get out of crouch at the moment?
	Vector3 mGroundDirection			= Vector3.right; //The direction of the ground we are standing on

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

		mStartPosition = transform.position;
		RecalcBounds();
		origColliderCenterY = ((BoxCollider)collider).center.y;
		origColliderSizeY = ((BoxCollider)collider).size.y;
	}

	public void Reset() //resets all private variables to their starting values
	{
		mOnGround = false;
		mSprinting = false;
		StopCrouch();
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
	}


    //Player update
	void FixedUpdate () 
	{
		if(canWallJump)
			UpdateWallInfo();		//Check the sides to see if we are against a wall
		UpdateGroundInfo();			//Check below to see if we are on the ground

		UpdateJumping();
		UpdateCrouching();
		ApplyGravity();
		ApplyMovementFriction();
	}


    //Called when the player presses a walking button (direction -1.0f is full left, and 1.0f is full right)
	public void Walk(float direction) 
	{
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

		//get an acceleration amount
		float accel = accelerationWalking;
		if (mSprinting)
			accel = accelerationSprinting;
		if (mCrouching && mOnGround)
			accel = accelerationWalking * crouchedAccelMultiplier;

        //apply actual force 
		rigidbody.AddForce(mGroundDirection * direction * accel, ForceMode.Acceleration);

		mStoppingForce = 1 - Mathf.Abs(direction);

		if (direction < 0 && mGoingRight)
		{
			mGoingRight = false;
			SendAnimMessage("GoLeft");
		}
		if (direction > 0 && !mGoingRight)
		{
			mGoingRight = true;
			SendAnimMessage("GoRight");
		}
	}


    //Called when the player holds down the jump key
	public void Jump() 
	{
		mJumpPressed = true;

		//See if we can start a jump
		if (mJumpFramesLeft == 0 && !mInJump && !mCrouching)
		{
			if (!mOnGround && mSecondJumpLeft && canDoubleJump) //Second jump
			{
				mSecondJumpLeft = false;

				mJumpFramesLeft = jumpTimeFrames;
				mInJump = true;

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
                    SendAnimMessage("StartedJump");
				}
			}
		}

		//Check if we are in the middle of a jump
		if(mJumpFramesLeft != 0)
		{
			Vector3 vel = rigidbody.velocity;
			vel.y = jumpVelocity;
			rigidbody.velocity = vel;
		}
	}


    //Called when the player presses the crouch button
	public void Crouch() 
	{
		if (!mCrouching) //make sure we aren't crouching
		{
			mCrouching = true;

			CrouchCollider();

			RecalcBounds();

			SendAnimMessage("StartedCrouching");
		}
	}

	public void CrouchCollider()
	{
		//change collider scale		
		BoxCollider myCollider = (BoxCollider)collider;

		Vector3 center = myCollider.center;
		Vector3 size = myCollider.size;

		//adjust the center and size in a way that it doesn't matter if the box collider has a center pivot or bottom pivot
		size.y = origColliderSizeY * crouchColliderScale;
		center.y = origColliderCenterY - (origColliderSizeY * (1.0f - crouchColliderScale))*0.5f;

		myCollider.size = size;
		myCollider.center = center;
	}

    //Called when the player releases the crouch button
	public void UnCrouch()
	{
		mTryingToUncrouch = true; //try to uncrouch if possible
	}

	//Called when actually going out of crouch
	void StopCrouch() 
	{
		mTryingToUncrouch = false;
		mCrouching = false;
		UnCrouchCollider();
		RecalcBounds();
		SendAnimMessage("StoppedCrouching");
	}

	public void UnCrouchCollider()
	{
		//reset collider scale
		BoxCollider myCollider = (BoxCollider)collider;

		Vector3 center = myCollider.center;
		Vector3 size = myCollider.size;

		size.y = origColliderSizeY;
		center.y = origColliderCenterY;

		myCollider.size = size;
		myCollider.center = center;
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
		if (!mOnGround) //basic gravity, only applied when we are not on the ground
		{
			rigidbody.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
		}

		if (mCrouching) //extra gravity for when we are holding crouch
		{
			rigidbody.AddForce(Vector3.down * crouchDownwardForce, ForceMode.Acceleration);
		}
	}

	void UpdateCrouching()
	{
		if (mTryingToUncrouch && CanUnCrouch())
		{
			StopCrouch();
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
		if (mSprinting)
			maxSpeed = maxSpeedSprinting;

		if (absSpeed > maxSpeed)
			velocity.x *= maxSpeed / absSpeed;

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
			if(!mOnGround)
                SendAnimMessage("LandedOnGround");

			Debug.DrawLine(hit.point+Vector3.up, hit.point, Color.green);
			Debug.DrawLine(hit.point, hit.point + mGroundDirection, Color.magenta);
			mOnGround = true;
			mOnWall = false;
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

		//Raycast going to the right
		if (Physics.Raycast(origin, Vector3.right, out hit))
		{
			if (hit.distance < halfPlayerWidth + epsilon && !mOnGround)
			{
				//remove collider penetration
				transform.position += Vector3.left * (halfPlayerWidth - hit.distance);

				HitWall(true);
				Debug.DrawLine(origin, hit.point, Color.yellow);
				return;
			}
		}

		//Raycast going to the left
		if (Physics.Raycast(origin, Vector3.left, out hit))
		{
			if (hit.distance < halfPlayerWidth + epsilon && !mOnGround)
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
			rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0); //Remove horizontal speed
			mWallStickynessLeft = wallStickyness;
			mOnWall = true;
            SendAnimMessage("LandedOnWall");
		}

		mOnWall = true;
	}

	bool CanUnCrouch()
	{
		//We will trace 2 rays from the front and back of the character both upwards, to see if we can uncrouch
		float epsilon = 0.05f; //the amount the ray will trace below the feet of the character to check if there is ground
		float origCharHeight = origColliderSizeY;
		float extraHeight = origCharHeight * 0.75f;
		float halfPlayerWidth = mCharacterWidth * 0.49f;

		//Origins of the ray
		Vector3 origin1 = GetBottomCenter() + Vector3.right * halfPlayerWidth + Vector3.up * (origCharHeight - extraHeight);
		Vector3 origin2 = GetBottomCenter() + Vector3.left * halfPlayerWidth + Vector3.up * (origCharHeight - extraHeight);
		Vector3 direction = Vector3.up;
		RaycastHit hit;

		bool canUncrouch = true;

		//Actual physic traces
		if (Physics.Raycast(origin1, direction, out hit) && (hit.distance < extraHeight + epsilon))
			canUncrouch = false;
		else if (Physics.Raycast(origin2, direction, out hit) && (hit.distance < extraHeight + epsilon))
			canUncrouch = false;

		return canUncrouch;
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
	public bool IsCrouching() { return mCrouching; }
	public bool IsOnWall() { return mOnWall; }
	public bool IsOnGround() { return mOnGround; }
	public bool IsSprinting() { return mSprinting; }
}

