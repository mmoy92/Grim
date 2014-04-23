using UnityEngine;
using System.Collections;

public class grimInfo : MonoBehaviour
{

	public static float grimHP;         // The player's health.
	public float hurtForce = 20f;      // The force with which the player is pushed when hurt.
	public static float maxHP;                   // The player's mazx health.
	private float lastHitTime;           // The time at which the player was last hit.
	private PlatformerController player;   // Reference to the PlatformerController script.

	//private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	//private Vector3 healthScale;				// The local scale of the health bar initially (with full health).

	public float invulPer = 2f; //Invulnerability period to prevent instant re-hit (NOTE: figure out what this timescale is...)
								//Development quandry -> would it be more efficient to have a bool vuln method that responded to triggering
								//damage scripts instead of handling it all here?

	// Use this for initialization
	void Start ()
	{
		maxHP = 5;
		grimHP = maxHP;
		player 					= GetComponent<PlatformerController>();
		//healthScale = healthBar.transform.localScale;
		//healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();

	}

	public void Damage(int dam)
	{
		if (Time.time > lastHitTime + invulPer) 
		{
			lastHitTime = Time.time; 
			print ("Taking damage");
			grimHP -= dam;
			if (grimHP <= 0) 
			{
				StartCoroutine (GrimDeath (player.gameObject));
				grimHP = maxHP;
			}
		}
	}

	public void Damage(int dam, Transform enemy)
	{
		/*
		if (enemy.gameObject.tag == "Enemy") {
			Vector3 hurtVector = (transform.position - enemy.position);
			Vector3 standHurtVect = new Vector3(hurtVector.x, Mathf.Abs(hurtVector.y), 0.0f);
			print (standHurtVect * hurtForce);
			rigidbody.velocity = (standHurtVect * hurtForce);
		} 
		else {
			Vector3 hurtVector = new Vector3(6.0f, 0.0f, 0.0f);
			rigidbody.velocity = hurtVector;
		}
		*/

		if (Time.time > lastHitTime + invulPer) 
		{
			lastHitTime = Time.time; 
			print ("Taking damage");
			grimHP -= dam;
			//UpdateHealthBar ()

			if (grimHP <= 0) 
			{
				StartCoroutine (GrimDeath (player.gameObject));
				Invoke("Start", 2.5f);
			}
		}
	}
	public void setInvulnFor(float t)
	{
		invulPer = t;

		lastHitTime = Time.time; 
	}
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Spikes") {
			Damage (1, col.transform);
		}
	}

	/*public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}*/

	//heuheuheu puns
	//Stolen from DeathVolume, moved here for centralized HP tracking/updating
	IEnumerator GrimDeath(GameObject player)
	{
		player.GetComponent<PlatformerAnimation> ().PlayerDied ();
		player.GetComponent<PlatformerController> ().RemoveControl ();
		
		yield return new WaitForSeconds(2.5f);
		
		player.GetComponent<PlatformerPhysics>().Reset();
		player.GetComponent<PlatformerAnimation>().PlayerLives();
		player.GetComponent<PlatformerController>().GiveControl();
	}
}

