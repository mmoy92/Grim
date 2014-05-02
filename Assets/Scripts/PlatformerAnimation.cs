using UnityEngine;
using System.Collections;

public class PlatformerAnimation : MonoBehaviour
{
	public Transform animatedPlayerModel; //Animated model that will have all the animations in it
	public Object necroTrail;
	public Object necroEffect;
	public Object holySpark;
	public Object holySpiral;
	bool mPlayerDead = false;
    bool mIdle = false;
	bool mGround = true;
	bool mAttack = false;
	bool isRight =  true;

	GameObject weapon;
	Transform renderedNinja;
	public bool switchWeapon = false;

	void Start () 
	{
		//Do some error checks first
		if (animatedPlayerModel == null)
		{
			Debug.LogError("The animated player model is not set.");
			this.enabled = false;
		}
		else if (!CheckAnims())
		{
			Debug.LogError("The animated player model does not seem to have the appropriate animations needed.");
			this.enabled = false;
		}
		else
		{
			//no errors
			animatedPlayerModel.animation["idle"].speed = 0;
		}
		if (switchWeapon) {
			swapWeapon ();
		}

		renderedNinja = animatedPlayerModel.Find("ninja");

	}
	void swapWeapon()
	{
		weapon = GameObject.Find ("ninja_sword");
		// Create a simple cube object 
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		// Re-parent the cube as child of the trackable gameObject
		cube.transform.parent = weapon.transform;
		
		// Adjust the position and scale 
		// so that it fits nicely on the target
		cube.transform.localPosition = new Vector3(0,0.8f,0);
		cube.transform.localRotation = Quaternion.identity;
		cube.transform.localScale = new Vector3(0.1f, 2.5f, 0.1f);
		
		// Disable collision of cube weapon
		cube.collider.isTrigger = true;
		//cube.active = true;

	}
	bool CheckAnims()
	{
		if (!animatedPlayerModel)
			return false;

		if (animatedPlayerModel.animation["walk"] == null ||
			animatedPlayerModel.animation["jump"] == null ||
			animatedPlayerModel.animation["slidein"] == null ||
			animatedPlayerModel.animation["slideout"] == null ||
			animatedPlayerModel.animation["death"] == null ||
			animatedPlayerModel.animation["onwall"] == null ||
		    animatedPlayerModel.animation["idle"] == null ||
		    animatedPlayerModel.animation["attackA"] == null||
		    animatedPlayerModel.animation["attackB"] == null||
		    animatedPlayerModel.animation["attackC"] == null

		    ) return false;

		return true;
	}

	void Update () 
	{
		//recalculate walking speed
		float walkingSpeed = Mathf.Abs(rigidbody.velocity.x)*0.075f;

		//Set mAttack to false if no attack animation is running
		if (mAttack && !animatedPlayerModel.animation ["attackA"].enabled && !animatedPlayerModel.animation ["attackB"].enabled && !animatedPlayerModel.animation ["attackC"].enabled)//&& !animatedPlayerModel.animation ["goodSpear"].enabled) 
		{
			mAttack = false;
			
		}
		if (!mAttack && mGround) //Do not animate walk during an attack animation or while in mid-air
		{
			animatedPlayerModel.animation["walk"].speed = walkingSpeed;

			//switch to idle animation if needed
			if (walkingSpeed == 0 && animatedPlayerModel.animation["walk"].enabled)
			{
				animatedPlayerModel.animation.Play("idle");
				mIdle = true;
			}
			if (walkingSpeed > 0.01f && mIdle) {
				mIdle = false;
				animatedPlayerModel.animation.CrossFade ("walk");
			}
		}

		if (!animatedPlayerModel.animation ["dash"].enabled  && !renderedNinja.renderer.enabled) 
		{
			EndGoodDash();
		}

	}

	void PlayAnim(string animName)
	{
		if (!mPlayerDead)
		{
			animatedPlayerModel.animation.Play(animName);
			animatedPlayerModel.transform.localPosition = Vector3.zero; //reset any position change made by on wall anim
		}
	}
	void GoLeft()
	{
		isRight = false;
		Vector3 localScale = animatedPlayerModel.transform.localScale;
		localScale.z = -Mathf.Abs(localScale.z);
		animatedPlayerModel.transform.localScale = localScale;
	}

	void GoRight()
	{
		isRight = true;
		Vector3 localScale = animatedPlayerModel.transform.localScale;
		localScale.z = Mathf.Abs(localScale.z);
		animatedPlayerModel.transform.localScale = localScale;
	}

	public void PlayerDied()
	{
        PlayAnim("death");
		mPlayerDead = true;
	}

	public void PlayerLives()
	{
		GoRight();
		mPlayerDead = false;
        PlayAnim("walk");
	}





	//MESSAGES CALLED BY PlatformerPhysics.cs:
	void StartedJump()
	{
        PlayAnim("jump");
		mGround = false;
	}

	void StartedWallJump()
	{
        PlayAnim("jump");
	}

	void StartedCrouching()
	{
        PlayAnim("slidein");
	}

	void StoppedCrouching()
	{
        PlayAnim("slideout");

		if (GetComponent<PlatformerPhysics>().IsOnWall())
			LandedOnWall();
		else
			animatedPlayerModel.animation.CrossFade("walk", 2.0f);
	}

	void LandedOnGround()
	{
		if (!GetComponent<PlatformerPhysics>().IsCrouching() &&!mAttack)
		{
            PlayAnim("walk");
		}
		mGround = true;
	}

	void LandedOnWall()
	{
        if (!GetComponent<PlatformerPhysics>().IsCrouching())
        {
            PlayAnim("onwall");

            if (!GetComponent<PlatformerPhysics>().IsWallOnRightSide())
            {
                animatedPlayerModel.transform.localPosition = new Vector3(0.45f, 0, 0);
                GoLeft();
            }
            else
            {
                animatedPlayerModel.transform.localPosition = new Vector3(-0.45f, 0, 0);
                GoRight();
            }
        }
	}

	void ReleasedWall()
	{
		if (!animatedPlayerModel.animation["jump"].enabled && !GetComponent<PlatformerPhysics>().IsCrouching())
            PlayAnim("walk");
	}
	void FinishedAttack()
	{
		if (mGround) {
			PlayAnim("walk");
		} else {
			PlayAnim("jump");
		}
	}
	void StartedEvilDash()
	{
		PlayAnim("dash");
		Vector3 spawn = transform.position;
		spawn.y += 1;
		if(isRight)
		{
			Instantiate(necroEffect, spawn, Quaternion.Euler(new Vector3(0,0,0)));
			Instantiate(necroTrail, spawn, Quaternion.Euler(new Vector3(0,0,0)));
		}
		else
		{
			Instantiate(necroEffect, spawn, Quaternion.Euler(new Vector3(0,0,180f)));
			Instantiate(necroTrail, spawn, Quaternion.Euler(new Vector3(0,0,180f)));
		}
		//print("Start Sprint");
	}
	void EndEvilDash()
	{

	}
	void StartedGoodDash()
	{

		PlayAnim("dash");
		renderedNinja.renderer.enabled = false;

		HolyEffect ();
		

	}
	void EndGoodDash()
	{
		HolyEffect ();
		renderedNinja.renderer.enabled = true;
	}
	void HolyEffect()
	{
		Vector3 spawn = transform.position;
		spawn.y += 1;
		Instantiate(holySpark, spawn, Quaternion.Euler(new Vector3(0,0,0)));
		Instantiate(holySpiral, spawn, Quaternion.Euler(new Vector3(0,0,0)));
	}
	void StartGoodSpear()
	{
		PlayAnim ("goodSpear");
		mAttack = true;
	}
	void StartEvilSplode()
	{
		PlayAnim ("goodSpear");
		mAttack = true;
	}
	void StartAttackA()
	{
		PlayAnim ("attackA");
		mAttack = true;
	}
	void StartAttackB()
	{
		PlayAnim ("attackB");
		mAttack = true;
	}
	void StartAttackC()
	{
		PlayAnim ("attackC");
		mAttack = true;
	}
}

