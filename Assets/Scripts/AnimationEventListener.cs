using UnityEngine;
using System.Collections;

public class AnimationEventListener : MonoBehaviour {
	PlayerCombat mPlayer;
	PlatformerPhysics mPhysics;
	// Use this for initialization
	void Start () {
		mPlayer = transform.gameObject.GetComponent<PlayerCombat>();
		mPhysics = transform.gameObject.GetComponent<PlatformerPhysics>();
	}
	
	void DealAttack()
	{
		print ("DEAL ATTACK");
		mPlayer.DealAttack ();
	}
	void EndDash()
	{
		mPhysics.EndDash ();
	}

	void OnAnimationEvent()

	{

		}
}
