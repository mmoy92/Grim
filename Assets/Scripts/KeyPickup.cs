using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour {

	private GameObject player;                      // Reference to the player.
<<<<<<< HEAD
	private grimInfo griminfo;        // Reference to the player's inventory.
	private AnimSounds sounds;		  // Reference to sound controller.
=======
	private grimInfo grim;        // Reference to the player's inventory.
	
>>>>>>> parent of f9397d4... Revert "GUI Elements + Dash Cooldowns"
	
	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
<<<<<<< HEAD
		griminfo = player.GetComponent<grimInfo>();
		sounds = player.GetComponent<AnimSounds>();
=======
		grim = player.GetComponent<grimInfo>();
>>>>>>> parent of f9397d4... Revert "GUI Elements + Dash Cooldowns"
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		// If the colliding gameobject is the player
		if (other.gameObject == player) {

<<<<<<< HEAD
			griminfo.keys += 1;
			sounds.pickupKey(griminfo.keys);
=======
			grimInfo.keys += 1;
			
>>>>>>> parent of f9397d4... Revert "GUI Elements + Dash Cooldowns"
			// Destroy this gameobject
			Destroy (gameObject);
		}
	}
}
