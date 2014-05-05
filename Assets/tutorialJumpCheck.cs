using UnityEngine;
using System.Collections;

public class tutorialJumpCheck : MonoBehaviour {
	public float messageAppearTimer = 4.0f;
	public GameObject actualJumpMessage;
	public Vector3 location;

	private Object myObject;
	private bool touched = false;
	// Use this for initialization
	void Start () {

	}
	void onTouch()
	{
		touched = true;
	}
	// Update is called once per frame
	void Update () {
		if(touched)
		{
		messageAppearTimer -= Time.deltaTime;

		if (messageAppearTimer <= 0 && myObject == null) 
		{
			myObject = Instantiate(actualJumpMessage, location, Quaternion.Euler(new Vector3(0,0,0)));
			SendMessage("discard");
		}
		if (Input.GetButton("Jump"))
			{
				Destroy (myObject, 2.0f);
				SendMessage("discard");

				Destroy(gameObject);
			}
		}
	}
}
