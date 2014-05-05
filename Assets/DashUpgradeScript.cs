using UnityEngine;
using System.Collections;

public class DashUpgradeScript : MonoBehaviour {
	private bool entered = false;
	public Vector3 location;
	public GameObject goodSkully;
	public GameObject badSkully;
	void OnTriggerEnter(Collider other) {
		if (entered || other.tag != "Player")
			return;
		entered = true;
		PlatformerPhysics phys = other.gameObject.GetComponent<PlatformerPhysics>();
		grimInfo info = other.gameObject.GetComponent<grimInfo>();
		if (info.soulCount < 0) { // good
			phys.hasEvilDash = false;
			phys.hasGoodDash = true;
			Instantiate(goodSkully, location, Quaternion.Euler(new Vector3(0,0,0)));
			PlayerPrefs.SetInt("evilDash", 0);
			PlayerPrefs.SetInt("goodDash", 1);
		} else { // bad
			phys.hasEvilDash = true;
			phys.hasGoodDash = false;
			Instantiate(badSkully, location, Quaternion.Euler(new Vector3(0,0,0)));
			PlayerPrefs.SetInt("evilDash", 1);
			PlayerPrefs.SetInt("goodDash", 0);
		}
	}

}
