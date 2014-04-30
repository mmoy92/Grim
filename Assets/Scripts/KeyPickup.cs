using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour {

	private GameObject player;                      // Reference to the player.
	private grimInfo grim;        // Reference to the player's inventory.
	
	
	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
		grim = player.GetComponent<grimInfo>();
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		// If the colliding gameobject is the player
		if (other.gameObject == player) {

			grimInfo.keys += 1;
			
			// Destroy this gameobject
			Destroy (gameObject);
		}
	}
}
