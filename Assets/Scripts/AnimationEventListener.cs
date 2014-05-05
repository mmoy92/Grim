using UnityEngine;
using System.Collections;

public class AnimationEventListener : MonoBehaviour {
	PlayerCombat mPlayer;
	PlatformerPhysics mPhysics;
	// Use this for initialization
	void Start () {
		mPlayer = transform.parent.gameObject.GetComponent<PlayerCombat>();
		mPhysics = transform.parent.gameObject.GetComponent<PlatformerPhysics>();
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
