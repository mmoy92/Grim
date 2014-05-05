using UnityEngine;
using System.Collections;

public class FloatUp : MonoBehaviour {
	private float accel = 0.0f;
	private bool beginFloating = false;
	// Use this for initialization
	void Start () {
		Invoke ("setFloat", 0.7f);
	}
	void setFloat()
	{
		beginFloating = true;
	}
	// Update is called once per frame
	void Update () {
		if (beginFloating) {
			accel += 0.05f;
			Vector3 temp = transform.position;
			temp.y += accel;

			transform.position = temp;
		}
	}
}
