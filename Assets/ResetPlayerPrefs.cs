using UnityEngine;
using System.Collections;

public class ResetPlayerPrefs : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
