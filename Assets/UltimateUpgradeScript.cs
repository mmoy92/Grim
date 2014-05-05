using UnityEngine;
using System.Collections;

public class UltimateUpgradeScript : MonoBehaviour {
	public Vector3 location;
	public GameObject goodSkully;
	public GameObject badSkully;
	private bool entered = false;
	
	void OnTriggerEnter(Collider other) {
		if (entered || other.tag != "Player")
			return;
		entered = true;
		PlatformerPhysics phys = other.gameObject.GetComponent<PlatformerPhysics>();
		grimInfo info = other.gameObject.GetComponent<grimInfo>();
		if (info.soulCount < 0) { // good
						phys.hasEvilAttack = false;
						phys.hasGoodAttack = true;
						Instantiate(goodSkully, location, Quaternion.Euler(new Vector3(0,0,0)));
						PlayerPrefs.SetInt ("evilAttack", 0);
						PlayerPrefs.SetInt ("goodAttack", 1);
				} else { // bad
						phys.hasEvilAttack = true;
						phys.hasGoodAttack = false;
						Instantiate(badSkully, location, Quaternion.Euler(new Vector3(0,0,0)));
						PlayerPrefs.SetInt ("evilAttack", 1);
						PlayerPrefs.SetInt ("goodAttack", 0);
				}
	}
}
