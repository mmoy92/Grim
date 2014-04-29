using UnityEngine;
using System.Collections;

public class ArrowBehavior : MonoBehaviour {

	int arrowDamage; 
	private int impact;
	private Vector3 vel; 
	// Use this for initialization
	void Start () {
		float spread = Random.value;
		if (spread < 0.33) {
			vel.y += 3;
		}
		else if (spread < 0.66)
		{
			vel.y -= 3;
		}

		arrowDamage = 1;
		impact = -1; 
		vel.x = 20.0f; 
		Destroy (gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		switch (impact) 
		{
			case 0: //Player
				//Can do some arrow sticking shenanigans later if time; now just stop&drop
				vel.x = 0;
				vel.y += 2; //So it'll fall until it hits level
				break;
			case 1: //Level
				vel = Vector3.zero; //Complete stop
				break;
			case 2: //??
				break;
			default: //Keep going  (-1)
				vel.y += 2;
				break;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		//other.tag

		if (collision.gameObject.tag == "level") {
			impact = 1;
		} 
		else if (collision.gameObject.tag == "Player") {
			impact = 2; 
			collision.gameObject.GetComponent<grimInfo>().Damage(arrowDamage);
		}
	}
}
