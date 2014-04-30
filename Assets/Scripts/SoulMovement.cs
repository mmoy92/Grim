using UnityEngine;
using System.Collections;

public class SoulMovement : MonoBehaviour {
	private GameObject player;                 
	private grimInfo info;
	private int lifetime = 10;
	private Vector3 newPosition;
	private Vector3 startPosition;
	public float smooth = 2;
	public float offset = 2;

	// Use this for initialization
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		info = player.GetComponent<grimInfo>();

		startPosition = transform.position;

		if (info.good) {
			lifetime = 20;
		}
		StartCoroutine(killNow(lifetime));
	}
	
	public IEnumerator killNow(float timeKill)
	{
		yield return new WaitForSeconds(timeKill);
		Destroy(gameObject);
		info.soulCount -= 1;
	}

	// Update is called once per frame
	void Update () 
	{
		PositionChanging ();
	}

	void PositionChanging()
	{
		if(transform.position == startPosition)
		{
			transform.position = Vector3.Lerp(transform.position, 
			                                  new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), 
			                                  smooth * Time.deltaTime);
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, startPosition, smooth * Time.deltaTime);
		}

	}
	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.tag == player.tag) {
			info.soulCount += 1;
			Destroy (gameObject);
		}
	}

	public void destroySoul ()
	{
		//GameObject player = GameObject.Find ("Player");
		//grimInfo grim_info = player.GetComponent<grimInfo> ();
		//grim_info.soulCount += 1;
		//Destroy (this.gameObject);
	}
	
}
