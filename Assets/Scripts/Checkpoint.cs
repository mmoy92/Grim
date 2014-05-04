using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{
	private GameObject player;                      
	void OnTriggerEnter(Collider other)
	{
		player = GameObject.FindGameObjectWithTag("Player");
		PlatformerPhysics physics = other.gameObject.GetComponent<PlatformerPhysics>();
		if (other.gameObject == player)
		{
			//set new respawn point
			physics.SetRespawnPoint(transform.position);
			// Destroy this gameobject
			Destroy (gameObject);
		}
	}
}