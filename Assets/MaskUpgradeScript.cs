using UnityEngine;
using System.Collections;

public class MaskUpgradeScript : MonoBehaviour {
	public GUIText guiText;
	private bool entered = false;
	
	void OnTriggerEnter(Collider other) {
		if (entered || other.name != "Player")
			return;
		entered = true;
		StartCoroutine(DisplayText());
		PlatformerPhysics phys = other.gameObject.GetComponent<PlatformerPhysics>();
		grimInfo info = other.gameObject.GetComponent<grimInfo>();
		if (info.soulCount < 0) { // good
			PlayerPrefs.SetFloat("soulCount", -0.5f); 
		} else { // bad
			PlayerPrefs.SetFloat ("soulCount", 0.5f);
		}
	}
	
	IEnumerator DisplayText() {
		guiText.text = "Congratulations!\nYou have upgraded your: Mask. \nIt is now glued to your face.\nI hope you like it.\n";
		yield return new WaitForSeconds(5);
		guiText.text = "";
	}
}
