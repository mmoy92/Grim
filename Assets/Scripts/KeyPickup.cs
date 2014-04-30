using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour {

	private GameObject player;                      // Reference to the player
	private grimInfo griminfo;        // Reference to the player's inventory.
	private AnimSounds sounds;		  // Reference to sound controller.

	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
		griminfo = player.GetComponent<grimInfo>();
		sounds = player.GetComponent<AnimSounds>();
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		// If the colliding gameobject is the player
		if (other.gameObject == player) {
			griminfo.keys += 1;
			sounds.pickupKey(griminfo.keys);
			// Destroy this gameobject
			Destroy (gameObject);
		}
	}
}
