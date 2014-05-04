using UnityEngine;
using System.Collections;

public class SpeAttackUpgraderer : MonoBehaviour {
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
			phys.hasEvilAttack = false;
			phys.hasGoodAttack = true;
		} else { // bad
			phys.hasEvilAttack = true;
			phys.hasGoodAttack = false;
		}
	}
	
	IEnumerator DisplayText() {
		guiText.text = "Congratulations!\nYou have unlocked a new attack.\nRight Click for special attack";
		yield return new WaitForSeconds(5);
		guiText.text = "";
	}
}
