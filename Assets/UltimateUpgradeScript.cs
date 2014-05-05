using UnityEngine;
using System.Collections;

public class UltimateUpgradeScript : MonoBehaviour {
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
						PlayerPrefs.SetInt ("evilAttack", 0);
						PlayerPrefs.SetInt ("goodAttack", 1);
				} else { // bad
						phys.hasEvilAttack = true;
						phys.hasGoodAttack = false;
						PlayerPrefs.SetInt ("evilAttack", 1);
						PlayerPrefs.SetInt ("goodAttack", 0);
				}
	}
	
	IEnumerator DisplayText() {
	    guiText.text = "Congratulations!\nYou have unlocked a new attack.\nRight Click for special attack";		yield return new WaitForSeconds(5);
		yield return new WaitForSeconds(5);
	    guiText.text = "";
	}
}
