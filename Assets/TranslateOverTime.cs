using UnityEngine;
using System.Collections;

public class TranslateOverTime : MonoBehaviour {
	public float amount = 0.2f;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		transform.Translate( Vector3.right * amount);
		print (transform.rotation);
	}
}
