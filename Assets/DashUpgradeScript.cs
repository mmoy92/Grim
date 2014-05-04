using UnityEngine;
using System.Collections;

public class UpgradeScript : MonoBehaviour {
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
			phys.hasEvilDash = false;
			phys.hasGoodDash = true;
		} else { // bad
			phys.hasEvilDash = true;
			phys.hasGoodDash = false;
		}
	}

	IEnumerator DisplayText() {
		guiText.text = "Congratulations!\nYou have unlocked: Dash.\nPress Shift to Dash";
		yield return new WaitForSeconds(5);
		guiText.text = "";
	}
}
