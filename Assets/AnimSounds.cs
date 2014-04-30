using UnityEngine;
using System.Collections;

public class AnimSounds : MonoBehaviour {

	public AudioClip jumpClip;
	public AudioClip attackClip;
	public AudioClip slashClip;
	public AudioClip evilDashClip; 
	public AudioClip goodDashClip; 
	public AudioClip footstepsClip;
	public float footstepsDistance = 2.0f;

	public float volume = 1.0f;

	private bool onGround = true;
	private bool walking = false;
	private PlatformerPhysics platformer;

	public void Start()
	{
		platformer = GetComponent<PlatformerPhysics>();
		StartCoroutine(footsteps());
	}
	
	public IEnumerator footsteps()
	{		
		// create an infinite loop that runs every frame:
		Vector3 lastPosition = transform.position;
		while (true) {
			float distance = (lastPosition - transform.position).magnitude;
			if (platformer.IsOnGround() && distance > footstepsDistance){
				lastPosition = transform.position;
				AudioSource.PlayClipAtPoint(footstepsClip, transform.position, volume);
				//yield return new WaitForSeconds(0.2f);
			}
			yield return null; // let Unity free till next frame
		}

	}
	
	void StartedEvilDash()
	{
		AudioSource.PlayClipAtPoint(evilDashClip, transform.position, volume);
	}
	void StartedGoodDash()
	{
		AudioSource.PlayClipAtPoint(goodDashClip, transform.position, volume);
	}
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
		//AudioSource.PlayClipAtPoint(dashClip, transform.position, volume);
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
		AudioSource.PlayClipAtPoint(slashClip, transform.position, volume);
	}
	void StartAttackB()
	{
		AudioSource.PlayClipAtPoint(attackClip, transform.position, volume);
		AudioSource.PlayClipAtPoint(slashClip, transform.position, volume);
	}
	void StartAttackC()
	{
		AudioSource.PlayClipAtPoint(attackClip, transform.position, volume);
		AudioSource.PlayClipAtPoint(slashClip, transform.position, volume);
	}
}
