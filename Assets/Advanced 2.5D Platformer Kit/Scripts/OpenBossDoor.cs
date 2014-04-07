using UnityEngine;
using System.Collections;

public class OpenBossDoor : MonoBehaviour {

	private GameObject player;                      // Reference to the player.
	private PlayerInventory playerInventory;        // Reference to the player's inventory.
	
	
	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
		playerInventory = player.GetComponent<PlayerInventory>();
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		// If the colliding gameobject is the player...
		if (other.gameObject == player) {
			if (playerInventory.hasKey1 && !playerInventory.usedKey) // Check to see if the player has the key
			{
				// Destroy the door
				gameObject.GetComponent<MeshRenderer>().enabled=false;
				gameObject.GetComponent<MeshCollider>().enabled=false;
				playerInventory.usedKey = true;
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		gameObject.GetComponent<MeshRenderer> ().enabled = true;
		gameObject.GetComponent<MeshCollider> ().enabled = true;

	}
}
