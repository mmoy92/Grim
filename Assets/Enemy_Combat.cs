using UnityEngine;
using System.Collections;

public class Enemy_Combat : MonoBehaviour {
	public int health = 50;

	public float deathDelay = 0.0f;

	public Transform deathEffect;
	public GameObject Soul;



	private bool isDead = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void getHurt(int amt)
	{
		health -= amt;
		if(health <=0 && !isDead)
		{
			die();
		}
		
	}
	void knockBack()
	{
		transform.Translate(Vector3.back * 0.5f);
		
	}
	
	void die()
	{
		isDead = true;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent("FollowCam2D").SendMessage("SlowMoShake");
		
		Instantiate(deathEffect, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
		Instantiate (Soul, transform.position, Quaternion.identity);

		SendMessage ("PlayDeathAnim");

		Destroy (gameObject, deathDelay);
	}
}
