using UnityEngine;
using System.Collections;

public class grimInfo : MonoBehaviour
{

	private int grimHP;
	private int maxHP; 
	private double grimAlign;
	private PlatformerController player;
	private float lastHitTime;
	public float invulPer = 2f; //Invulnerability period to prevent instant re-hit (NOTE: figure out what this timescale is...)
								//Development quandry -> would it be more efficient to have a bool vuln method that responded to triggering
								//damage scripts instead of handling it all here?

		// Use this for initialization
		void Start ()
		{
			maxHP 					= 3; //3 at start
			grimHP 					= maxHP; //HP initial set
			grimAlign				= 0.5; //Grim's alignment. Starting Positive for bugs/stuff
			player 					= GetComponent<PlatformerController>();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	public int getHP()
	{
		return grimHP;
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

