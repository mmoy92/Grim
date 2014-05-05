using UnityEngine;
using System.Collections;

public class spawnOnTouch : MonoBehaviour {
	public Object objectToSpawn;
	public Vector3 location;
	public GameObject otherSpawn;
	
	private Object myObject;
	private GameObject player;
	private bool isShowing = false;
	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		// If the colliding gameobject is the player
		if (other.gameObject == player && !isShowing) {

			isShowing = true;
			if(objectToSpawn != null)
				myObject = Instantiate(objectToSpawn, location, Quaternion.Euler(new Vector3(0,0,0)));

			if(otherSpawn != null)
			{
				spawnOnTouch previousSpawn = otherSpawn.GetComponent<spawnOnTouch>();
				previousSpawn.discard();
				Destroy (previousSpawn.gameObject);
			}

			SendMessage("onTouch");
		}
	}
	public void discard()
	{
		Destroy (myObject);
	}
}
