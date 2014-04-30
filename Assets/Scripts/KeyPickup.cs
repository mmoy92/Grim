using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour {

	private GameObject player;                      // Reference to the player.
<<<<<<< HEAD
<<<<<<< HEAD
	private grimInfo griminfo;        // Reference to the player's inventory.
	private AnimSounds sounds;		  // Reference to sound controller.
=======
	private grimInfo grim;        // Reference to the player's inventory.
	
>>>>>>> parent of f9397d4... Revert "GUI Elements + Dash Cooldowns"
=======
	private grimInfo griminfo;        // Reference to the player's inventory.
	private AnimSounds sounds;		  // Reference to sound controller.
>>>>>>> 550af5a2f0a18856dc892149928ef60a65151888
	
	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
<<<<<<< HEAD
<<<<<<< HEAD
		griminfo = player.GetComponent<grimInfo>();
		sounds = player.GetComponent<AnimSounds>();
=======
		grim = player.GetComponent<grimInfo>();
>>>>>>> parent of f9397d4... Revert "GUI Elements + Dash Cooldowns"
=======
		griminfo = player.GetComponent<grimInfo>();
		sounds = player.GetComponent<AnimSounds>();
>>>>>>> 550af5a2f0a18856dc892149928ef60a65151888
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		// If the colliding gameobject is the player
		if (other.gameObject == player) {

<<<<<<< HEAD
<<<<<<< HEAD
			griminfo.keys += 1;
			sounds.pickupKey(griminfo.keys);
=======
			grimInfo.keys += 1;
			
>>>>>>> parent of f9397d4... Revert "GUI Elements + Dash Cooldowns"
=======
			griminfo.keys += 1;
			sounds.pickupKey(griminfo.keys);
>>>>>>> 550af5a2f0a18856dc892149928ef60a65151888
			// Destroy this gameobject
			Destroy (gameObject);
		}
	}
}
