using UnityEngine;
using System.Collections;

public class PlatformerController : MonoBehaviour
{
	PlatformerPhysics mPlayer;
	bool mHasControl;
	float dashingTime = 5f;

	void Start () 
	{
		mHasControl = true;
		mPlayer = GetComponent<PlatformerPhysics>();
		if (mPlayer == null)
			Debug.LogError("This object also needs a PlatformerPhysics component attached for the controller to function properly");
	}

	void Update () 
	{
		dashingTime += Time.deltaTime;
		//here are the actions that are triggered by a press or a release
		if (mPlayer && mHasControl)
		{
<<<<<<< HEAD
			if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)){
				if(dashingTime >= 5f){
					dashingTime = 0;
					mPlayer.StartDash();
				}
			}
=======
			if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
				mPlayer.StartDash();
>>>>>>> 550af5a2f0a18856dc892149928ef60a65151888

			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
				mPlayer.Crouch();

			if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
				mPlayer.UnCrouch();
			if(Input.GetMouseButtonDown(0))
				mPlayer.Attack();
			if(Input.GetMouseButtonDown(1))
				mPlayer.SpecialAttack();

		}
	}

	void FixedUpdate()
	{
		//here are actions where the buttons can be held for a longer period
		if (mPlayer && mHasControl)
		{
			if (Input.GetButton("Jump"))
				mPlayer.Jump();

			mPlayer.Walk(Input.GetAxisRaw("Horizontal"));
		}
	}

	public void GiveControl() { mHasControl = true; }
	public void RemoveControl() { mHasControl = false; }
	public bool HasControl() { return mHasControl; }
}

