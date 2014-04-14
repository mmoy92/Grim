using UnityEngine;
using System.Collections;

public class DestroyTimer : MonoBehaviour {
	public float secondsToDestroy;
	// Use this for initialization
	void Start () {
		Destroy(gameObject, secondsToDestroy);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
