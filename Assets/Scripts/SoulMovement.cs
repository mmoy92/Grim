using UnityEngine;
using System.Collections;

public class SoulMovement : MonoBehaviour {
	private GameObject player;                 
	//private PlayerInventory playerInventory; 
	private grimInfo info;
	private bool floatup;
	private int lifetime = 10;

	// Use this for initialization
	void Awake ()
	{
		floatup = false;
		player = GameObject.FindGameObjectWithTag("Player");
		//playerInventory = player.GetComponent<PlayerInventory>();
		info = player.GetComponent<grimInfo>();
		// Setting up the references.
		Destroy(gameObject, lifetime);
	}

	// Update is called once per frame
	void Update () 
	{

	}
	
	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.tag == player.tag) {
			info.soulCount += 1;
			Destroy (gameObject);
		}
	}

	void destroySoul ()
	{
		Destroy (gameObject);
	}
	
}
