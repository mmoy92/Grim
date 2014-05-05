using UnityEngine;
using System.Collections;

public class EnemySwipe : MonoBehaviour {
	public Transform slash;
	public float damage = 1; 
	private bool didHit = false;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 0.75f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {
		if(other.tag == "Player")
		{
			if(!didHit)
			{
				didHit = true;
				other.gameObject.GetComponent("grimInfo").SendMessage("Damage", damage);
			}
		}
	}
}
