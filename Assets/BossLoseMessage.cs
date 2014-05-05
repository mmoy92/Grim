using UnityEngine;
using System.Collections;

public class BossLoseMessage : MonoBehaviour {
	public Vector3 location;
	public GameObject goodSkully;
	public GameObject badSkully;

	private bool done = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void WinMessage()
	{
		if (!done) {
				done = true;
				grimInfo info = GameObject.FindGameObjectWithTag ("Player").GetComponent<grimInfo> ();
				if (info.soulCount < 0) { // good
						Instantiate (goodSkully, location, Quaternion.Euler (new Vector3 (0, 0, 0)));
				} else { // bad

						Instantiate (badSkully, location, Quaternion.Euler (new Vector3 (0, 0, 0)));
				}
		}

	}
}
