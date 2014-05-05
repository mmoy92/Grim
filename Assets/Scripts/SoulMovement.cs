using UnityEngine;
using System.Collections;

public class SoulMovement : MonoBehaviour {
	public GameObject soulFreed;
	public GameObject soulKilled;

	private GameObject player;                 
	private grimInfo info;
	private int lifetime = 5;
	private Vector3 pos1;
	private Vector3 pos2;
	private Vector3 moveTo;
	private Vector3 offset;
	private float speed = 0.01f;

	// Use this for initialization
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		info = player.GetComponent<grimInfo>();

		offset = Vector3.up;
		pos1 = transform.position;
		pos2 = transform.position + offset; 

		if (info.good) {
			lifetime = 10;
		}
		StartCoroutine(killNow(lifetime));
	}
	
	public IEnumerator killNow(float timeKill)
	{
		yield return new WaitForSeconds(timeKill);
		Destroy(gameObject);
		info.soulCount -= 1;

		Instantiate (soulFreed, transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
	}

	// Update is called once per frame
	void Update () 
	{
		if(transform.position == pos1)
		{
			moveTo = pos2;	
		}
		if(transform.position == pos2)	
		{
			moveTo = pos1;	
		}
		transform.position = Vector3.MoveTowards(transform.position, moveTo, speed);
	}

	void OnCollisionEnter (Collision other)
	{

	}

	public void destroySoul ()
	{
		GameObject player = GameObject.Find ("Player");
		grimInfo grim_info = player.GetComponent<grimInfo> ();
		grim_info.soulCount += 1;
		Destroy (this.gameObject);

		Instantiate (soulKilled, transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
	}
	
}
