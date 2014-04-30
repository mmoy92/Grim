using UnityEngine;
using System.Collections;

public class FollowCam2D : MonoBehaviour
{
	public Transform target;
	public float distance = 10.0f;
	public float extraHeight = 2.0f;

	public Transform skullyText;
	private float shake = 0;
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	private bool isSlowMo = false;

	float origDist;

	void Start () 
	{
		origDist = distance;
	}

	void FixedUpdate () 
	{
		if (target)
		{
			if (Input.GetKey(KeyCode.LeftControl)){
				distance = origDist * 5;
			} else if(isSlowMo){
				distance = origDist * 0.75f;
			} else {
				distance = origDist;
			}
			Vector3 targetPos = target.position + Vector3.up * extraHeight;
			targetPos.z = -distance;
			transform.position -= (transform.position - targetPos) * 0.25f;

			if (shake > 0) {
				transform.position += Random.insideUnitSphere * shakeAmount;
				shake -= Time.deltaTime * decreaseFactor;

			} else {
				shake = 0.0f;
				isSlowMo = false;
				Time.timeScale = 1.0f;
			}
		}
	}

	public void SetTarget(Transform inTarget)
	{
		target = inTarget;
	}

	public void Shake(float amt)
	{
		shake = amt;
	}

	public void SlowMoShake()
	{
		shake = 0.5f;
		Time.timeScale = 0.75f;
		isSlowMo = true;

		Vector3 pos = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position;
		Instantiate (skullyText, new Vector3(pos.x, pos.y + 5, pos.z), Quaternion.Euler (new Vector3 (0, 0, 0)));
	}
}

