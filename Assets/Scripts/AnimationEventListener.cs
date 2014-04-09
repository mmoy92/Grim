using UnityEngine;
using System.Collections;

public class AnimationEventListener : MonoBehaviour {
	PlayerCombat mPlayer;
	// Use this for initialization
	void Start () {
		mPlayer = transform.parent.gameObject.GetComponent<PlayerCombat>();
	}
	
	void DealAttack()
	{
		mPlayer.DealAttack ();
	}
}
