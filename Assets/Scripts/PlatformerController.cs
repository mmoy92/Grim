using UnityEngine;
using System.Collections;

public class PlatformerController : MonoBehaviour
{
	PlatformerPhysics mPlayer;
	bool mHasControl;

	void Start () 
	{
		mHasControl = true;
		mPlayer = GetComponent<PlatformerPhysics>();
		if (mPlayer == null)
			Debug.LogError("This object also needs a PlatformerPhysics component attached for the controller to function properly");
	}

	void Update () 
	{
		//here are the actions that are triggered by a press or a release
		if (mPlayer && mHasControl)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
				mPlayer.StartDash();

			if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
				//mPlayer.StopSprint();

			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
				mPlayer.Crouch();

			if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
				mPlayer.UnCrouch();
			if(Input.GetMouseButtonDown(0))
				mPlayer.Attack();

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

