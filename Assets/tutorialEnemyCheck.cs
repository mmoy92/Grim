using UnityEngine;
using System.Collections;

public class tutorialEnemyCheck : MonoBehaviour {
	public GameObject otherSpawn;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void die()
	{
		spawnOnTouch previousSpawn = otherSpawn.GetComponent<spawnOnTouch>();
		previousSpawn.discard();
		Destroy (previousSpawn.gameObject);
	}
}
