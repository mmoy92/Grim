using UnityEngine;
using System.Collections;

public class MeleeSwipe : MonoBehaviour {
	public Transform slash;
	public AudioClip slashClip;
	private bool didHit = false;
	// Use this for initialization
	void Start () {
		Destroy(gameObject,0.3f);
	}
	void Update()
	{
		transform.Translate(Vector3.right * 0.2f);
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
			AudioSource.PlayClipAtPoint(slashClip, transform.position, 1.0f);
			Object newInst = Instantiate(slash, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
			other.gameObject.SendMessage("getHurt", 10);
			//StartCoroutine(Pause(0.1f));
			Destroy(gameObject,0.1f);
		}
	}
}
