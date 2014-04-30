using UnityEngine;
using System.Collections;

public class grimInfo : MonoBehaviour
{
	public AudioClip hurtClip;			// clip to play when player gets hurt
	public AudioClip dieClip;			// clip to play when player dies
	public static float grimHP;         // The player's health.
	public float hurtForce = 20f;      // The force with which the player is pushed when hurt.
	public static float maxHP;                   // The player's mazx health.
	public static float keys;
	public static float maxKeys;
	private float lastHitTime;           // The time at which the player was last hit.
	private PlatformerController player;   // Reference to the PlatformerController script.

	//private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	//private Vector3 healthScale;				// The local scale of the health bar initially (with full health).

	public float invulPer = 2f; //Invulnerability period to prevent instant re-hit (NOTE: figure out what this timescale is...)
								//Development quandry -> would it be more efficient to have a bool vuln method that responded to triggering
								//damage scripts instead of handling it all here?
	public int soulCount;
	public int keys = 0;	//Whether or not the player has the first key
	public bool usedKey;
	public bool good = false;
	
	// Use this for initialization
	void Start ()
	{
		maxHP = 5;
		grimHP = maxHP;
		maxKeys = 4;
		player 					= GetComponent<PlatformerController>();
		soulCount = 0;
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
			AudioSource.PlayClipAtPoint(hurtClip, transform.position, 1f);
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
			AudioSource.PlayClipAtPoint(hurtClip, transform.position, 1f);
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
<<<<<<< HEAD
	/*public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}*/
=======
>>>>>>> parent of f9397d4... Revert "GUI Elements + Dash Cooldowns"

	//heuheuheu puns
	//Stolen from DeathVolume, moved here for centralized HP tracking/updating
	IEnumerator GrimDeath(GameObject player)
	{
		AudioSource.PlayClipAtPoint(dieClip, transform.position, 1f);
		player.GetComponent<PlatformerAnimation> ().PlayerDied ();
		player.GetComponent<PlatformerController> ().RemoveControl ();
		
		yield return new WaitForSeconds(2.5f);
		
		player.GetComponent<PlatformerPhysics>().Reset();
		player.GetComponent<PlatformerAnimation>().PlayerLives();
		player.GetComponent<PlatformerController>().GiveControl();
	}
	
}
