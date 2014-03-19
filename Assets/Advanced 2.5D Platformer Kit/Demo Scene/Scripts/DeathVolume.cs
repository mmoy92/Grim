using UnityEngine;
using System.Collections;

public class DeathVolume : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		PlatformerController controller = other.gameObject.GetComponent<PlatformerController>();
		if (controller && controller.HasControl())
		{
			//let player die
			StartCoroutine(PlayerDeath(other.gameObject));
		}
	}

	IEnumerator PlayerDeath(GameObject player)
	{
		player.GetComponent<PlatformerAnimation>().PlayerDied();
		player.GetComponent<PlatformerController>().RemoveControl();

		yield return new WaitForSeconds(2.5f);

		player.GetComponent<PlatformerPhysics>().Reset();
		player.GetComponent<PlatformerAnimation>().PlayerLives();
		player.GetComponent<PlatformerController>().GiveControl();
	}
}

