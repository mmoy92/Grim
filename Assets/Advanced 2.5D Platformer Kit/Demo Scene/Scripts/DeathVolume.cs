using UnityEngine;
using System.Collections;

public class DeathVolume : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		PlatformerController controller = other.gameObject.GetComponent<PlatformerController>();
		PlayerCombat combat = other.GetComponent<PlayerCombat>();
		grimInfo info = other.GetComponent<grimInfo> ();
		int spikeDamage = 1; 
		if (controller && controller.HasControl() && combat.IsVulnerable())
		{
			info.Damage(spikeDamage);
		}
	}

}

