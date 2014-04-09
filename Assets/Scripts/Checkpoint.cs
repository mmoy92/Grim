using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		PlatformerPhysics physics = other.gameObject.GetComponent<PlatformerPhysics>();
		if (physics)
		{
			//set new respawn point
			physics.SetRespawnPoint(transform.position);
		}
	}
}