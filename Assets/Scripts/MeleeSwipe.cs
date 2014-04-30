using UnityEngine;
using System.Collections;

public class MeleeSwipe : MonoBehaviour {
	public Transform slash;
	private bool didHit = false;
	public float damage = 10;
	void Update()
	{
		//transform.Translate(Vector3.right * 0.1f);
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
		if(other.tag == "Enemy" && !didHit)
		{
			didHit = true;
			Object newInst = Instantiate(slash, other.transform.position, Quaternion.Euler(new Vector3(0,0,0)));
			other.gameObject.SendMessage("getHurt",damage);
			other.gameObject.SendMessage("knockBack");

			FollowCam2D camComponent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCam2D>();
			//camComponent.SendMessage("Shake", 0.1);

			//StartCoroutine(Pause(0.1f));
			//Destroy(gameObject,0.1f);
		}
	}
}
