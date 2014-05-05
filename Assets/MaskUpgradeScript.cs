using UnityEngine;
using System.Collections;

public class MaskUpgradeScript : MonoBehaviour {
	private bool entered = false;
	public Vector3 location;
	public GameObject goodSkully;
	public GameObject badSkully;
	void OnTriggerEnter(Collider other) {
		if (entered || other.tag!= "Player")
			return;
		entered = true;
		PlatformerPhysics phys = other.gameObject.GetComponent<PlatformerPhysics>();
		grimInfo info = other.gameObject.GetComponent<grimInfo>();
		if (info.soulCount < 0) { // good
			PlayerPrefs.SetFloat("soulCount", -0.5f); 
			Instantiate(goodSkully, location, Quaternion.Euler(new Vector3(0,0,0)));
		} else { // bad
			PlayerPrefs.SetFloat ("soulCount", 0.5f);
			Instantiate(badSkully, location, Quaternion.Euler(new Vector3(0,0,0)));
		}
	}
}
