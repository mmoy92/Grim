using UnityEngine;
using System.Collections;

public class GrimAnimation : MonoBehaviour
{
	public Transform animatedPlayerModel; //Animated model that will have all the animations in it
	public Animator anim; 
	public Object necroTrail;
	public Object necroEffect;
	public Object teleportParticle;

	bool mPlayerDead = false;
	bool mIdle = false;
	bool mGround = true;
	bool mAttack = false;
	bool isRight =  true;
	
	private bool isEvilDash = false;
	
	GameObject weapon;
	Transform renderedGrim;
	public bool switchWeapon = false;

	void Start () 
	{
		if (animatedPlayerModel == null)
		{
			Debug.Log("The animated player model is not set.");
		}
		anim = gameObject.GetComponent<Animator> ();
		if (anim == null)
						Debug.Log ("Uh-oh");
		renderedGrim = animatedPlayerModel.Find ("Cloak");
		if (renderedGrim == null)
						Debug.Log ("Shits broke, yo");
				else
						Debug.Log ("You good");
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
/*	bool CheckAnims()
	{
		if (!animatedPlayerModel)
			return false;
		
		if (animatedPlayerModel.animation["walk"] == null ||
		    animatedPlayerModel.animation["jump"] == null ||
		    animatedPlayerModel.animation["death"] == null ||
		    animatedPlayerModel.animation["onwall"] == null ||
		    animatedPlayerModel.animation["idle"] == null ||
		    animatedPlayerModel.animation["attackA"] == null||
		    animatedPlayerModel.animation["attackB"] == null||
		    animatedPlayerModel.animation["attackC"] == null
		    
		    ) return false;
		
		return true;
	}*/
	
	void Update () 
	{
		//recalculate walking speed
		float walkingSpeed = Mathf.Abs (rigidbody.velocity.x);//*0.075f;
		//Debug.Log (walkingSpeed);
		//Set mAttack to false if no attack animation is running
		if (mAttack && !anim.GetBool("Attack") && !anim.GetBool("SpinAttack")) 
		{
			mAttack = false;
			
		}
		if (!mAttack && mGround) //Do not animate walk during an attack animation or while in mid-air
		{
			//animatedPlayerModel.animation["walk"].speed = walkingSpeed;

			//switch to idle animation if needed
			if (walkingSpeed == 0 && anim.GetBool("Walk"))
			{
//				animatedPlayerModel.animation.Play("idle");
				//Debug.Log("walk false");
				anim.SetBool("Walk", false);
				mIdle = true;
			}
			if (walkingSpeed > 0.01f && mIdle) {
				mIdle = false;
				//Debug.Log("walk true");
//				animatedPlayerModel.animation.CrossFade ("walk");
				anim.SetBool("Walk", true);
				//Debug.Log(anim.GetBool("Walk"));
			}
		}
		
		if (anim.GetBool("Charge")) {
			if(isEvilDash){
				Vector3 spawn = transform.position;
				spawn.y += 1;
				
				Instantiate (necroTrail, spawn, Quaternion.Euler (new Vector3 (0, 0, 0)));
			}
		}else if ( !renderedGrim.renderer.enabled) 
		{
			EndGoodDash();
		}
		
	}
///------------------------------------------------------------------	
/*	void PlayAnim(string animName)
	{
		if (!mPlayerDead)
		{
			animatedPlayerModel.animation.Play(animName);
			animatedPlayerModel.transform.localPosition = Vector3.zero; //reset any position change made by on wall anim
		}
	}*/
/// -----------------------------------------------------------------
	void GoLeft()
	{
		isRight = false;
		anim.SetBool ("Walk", true);
		//transform.Rotate(Vector3.left);
		Vector3 localScale = animatedPlayerModel.transform.localScale;
		localScale.z = -Mathf.Abs(localScale.z);
		animatedPlayerModel.transform.localScale = localScale;
	}
	
	void GoRight()
	{
		isRight = true;
		anim.SetBool ("Walk", true);
		//transform.Rotate(Vector3.right);
		Vector3 localScale = animatedPlayerModel.transform.localScale;
		localScale.z = Mathf.Abs(localScale.z);
		animatedPlayerModel.transform.localScale = localScale;
	}
	
	public void PlayerDied()
	{
//		PlayAnim("death");
		anim.SetBool ("Alive", false);
		mPlayerDead = true;
	}
	
	public void PlayerLives()
	{
		GoRight();
		mPlayerDead = false;
//		PlayAnim("walk");
		anim.SetBool ("Alive", true);
	}

	//MESSAGES CALLED BY PlatformerPhysics.cs:
	void StartedJump()
	{
//		PlayAnim("jump");
		anim.SetBool ("Jump", true);
		anim.SetBool ("Walk", false);
		anim.SetBool ("Attack", false); 
		anim.SetBool ("SpinAttack", false);
		anim.SetBool ("Climb", false);
		anim.SetBool ("Charge", false); 
		mGround = false;
	}
	
	void StartedWallJump()
	{
//		PlayAnim("jump");
		anim.SetBool ("Jump", true);
		anim.SetBool ("Walk", false);
		anim.SetBool ("Attack", false); 
		anim.SetBool ("SpinAttack", false);
		anim.SetBool ("Climb", false);
		anim.SetBool ("Charge", false); 
	}
	
	
	void LandedOnGround()
	{
		if (!mAttack)
		{
//			PlayAnim("walk");
			anim.SetBool ("Jump", false);
			anim.SetBool ("Walk", true);
			anim.SetBool ("Attack", false); 
			anim.SetBool ("SpinAttack", false);
			anim.SetBool ("Climb", false);
			anim.SetBool ("Charge", false); 
		}
		mGround = true;
	}
	
	void LandedOnWall()
	{
		
		//PlayAnim("onwall");
		anim.SetBool ("Jump", false);
		anim.SetBool ("Walk", false);
		anim.SetBool ("Attack", false); 
		anim.SetBool ("SpinAttack", false);
		anim.SetBool ("Climb", true);
		anim.SetBool ("Charge", false); 
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
	
	void ReleasedWall()
	{
//		if (!animatedPlayerModel.animation["jump"].enabled)
//			PlayAnim("walk");
		anim.SetBool ("Climb", false); 
	}
	void FinishedAttack()
	{
		if (mGround) {
//			PlayAnim("walk");
			anim.SetBool ("Attack", false); 
			anim.SetBool ("SpinAttack", false);
		} else {
//			PlayAnim("jump");
			anim.SetBool ("Attack", false); 
			anim.SetBool ("SpinAttack", false);
		}
	}
	void StartedEvilDash()
	{
//		PlayAnim("dash");
		anim.SetBool ("Jump", false);
		anim.SetBool ("Walk", false);
		anim.SetBool ("Attack", false); 
		anim.SetBool ("SpinAttack", false);
		anim.SetBool ("Climb", false);
		anim.SetBool ("Charge", true); 
		isEvilDash = true;
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
		anim.SetBool ("Charge", false); 
	}
	void StartedGoodDash()
	{
		isEvilDash = false;
//		PlayAnim("dash");
		renderedGrim.renderer.enabled = false;
		
		teleportEffect ();
		
		
	}
	void EndGoodDash()
	{
		teleportEffect();
		renderedGrim.renderer.enabled = true;
	}
	void teleportEffect()
	{
		Vector3 spawn = transform.position;
		spawn.y += 1;
		Instantiate(teleportParticle, spawn, Quaternion.Euler(new Vector3(0,0,0)));
	}
	void StartGoodSpear()
	{
//		PlayAnim ("goodSpear");
		anim.SetBool ("Spear", true); 
		anim.SetBool ("Jump", false);
//		anim.SetBool ("Walk", false);
		anim.SetBool ("Attack", false); 
		anim.SetBool ("SpinAttack", false);
		anim.SetBool ("Climb", false);
		anim.SetBool ("Charge", false); 
		mAttack = true;
	}
	void StartEvilSplode()
	{
//		PlayAnim ("goodSpear");
		anim.SetBool ("Spear", true); 
		anim.SetBool ("Jump", false);
//		anim.SetBool ("Walk", false);
		anim.SetBool ("Attack", false); 
		anim.SetBool ("SpinAttack", false);
		anim.SetBool ("Climb", false);
		anim.SetBool ("Charge", false); 
		mAttack = true;
	}
	void StartAttackA()
	{
		//		PlayAnim ("attackA");
		anim.SetBool ("SpinAttack", false);
		anim.SetBool ("Attack", true);
		mAttack = true;
	}
	void StartAttackB()
	{
		//		PlayAnim ("attackB");
		anim.SetBool ("Attack", false);
		anim.SetBool ("SpinAttack", true);
		mAttack = true;
	}
	void StartAttackC()
	{
		//		PlayAnim ("attackC");
		anim.SetBool ("SpinAttack", false);
		anim.SetBool ("Attack", true);
		mAttack = true;
	}
	void EndCharge()
	{
		anim.SetBool ("Charge", false);
	}
	void EndSpear()
	{
		anim.SetBool ("Spear", false);
	}
}

