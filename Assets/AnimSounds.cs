using UnityEngine;
using System.Collections;

public class AnimSounds : MonoBehaviour {

	public AudioClip jumpClip;
	public AudioClip attackClip;
	public AudioClip dashClip;
	public AudioClip crouchClip;
	public AudioClip landClip;
	public AudioClip wallClip;
	public float volume = 2.0f;
	void StartedJump()
	{
		AudioSource.PlayClipAtPoint(jumpClip, transform.position, volume);
	}
	
	void StartedWallJump()
	{
		AudioSource.PlayClipAtPoint(jumpClip, transform.position, volume);
	}
	
	void StartedCrouching()
	{
		AudioSource.PlayClipAtPoint(crouchClip, transform.position, volume);
	}
	
	void StoppedCrouching()
	{
	}
	
	void LandedOnGround()
	{
		//AudioSource.PlayClipAtPoint(landClip, transform.position, volume);
	}
	
	void LandedOnWall()
	{
		AudioSource.PlayClipAtPoint(landClip, transform.position, volume);
		// todo: play wallclip
	}
	
	void ReleasedWall()
	{
		// todo: play wallclip
	}
	void FinishedAttack()
	{
	}
	void StartedDashing()
	{
		AudioSource.PlayClipAtPoint(dashClip, transform.position, volume);
	}
	void StartedSprinting()
	{
	}
	
	void StoppedSprinting()
	{
	}
	void StartAttackA()
	{
		AudioSource.PlayClipAtPoint(attackClip, transform.position, volume);
	}
	void StartAttackB()
	{
		AudioSource.PlayClipAtPoint(attackClip, transform.position, volume);
	}
	void StartAttackC()
	{
		AudioSource.PlayClipAtPoint(attackClip, transform.position, volume);
	}
}
