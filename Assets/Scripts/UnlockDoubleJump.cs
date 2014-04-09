using UnityEngine;
using System.Collections;

public class UnlockDoubleJump : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		PlatformerPhysics physics = other.gameObject.GetComponent<PlatformerPhysics>();
		if (physics)
		{
			//unlock double jump and destroy ourselves
			physics.canDoubleJump = true;
			Destroy(gameObject);
		}
	}
}