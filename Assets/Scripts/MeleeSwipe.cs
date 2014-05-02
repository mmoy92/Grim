using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeSwipe : MonoBehaviour {
	public Transform slash;
	public float damage = 10; 
	private List<Collider> alreadyHit;
	void Start()
	{
		alreadyHit = new List<Collider> ();
	}
	void Update()
	{

	}

	private IEnumerator Pause(float p)
	{
		Time.timeScale = 0.0f;
		float pauseEndTime = Time.realtimeSinceStartup + p;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
		Time.timeScale = 1;

	}

	void OnTriggerStay(Collider other) {

		if(other.tag == "Enemy")
		{
			bool didHit = false;
			foreach(Collider col in alreadyHit)
			{
				if(other.Equals(col))
				{
					didHit = true;
					break;
				}
			}
			if(!didHit)
			{
				print (other);
				alreadyHit.Add(other);

				Object newInst = Instantiate(slash, other.transform.position, Quaternion.Euler(new Vector3(0,0,0)));
				other.gameObject.SendMessage("getHurt",damage);
				other.gameObject.SendMessage("knockBack");

				FollowCam2D camComponent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCam2D>();
				camComponent.SendMessage("Shake", 0.1);

				//Destroy(gameObject,0.1f);
			}
		}

		if (other.tag == "Soul" && alreadyHit.Count == 0)
		{
			SoulMovement soul = other.GetComponent<SoulMovement>();
			soul.destroySoul();

		}
	}
}
