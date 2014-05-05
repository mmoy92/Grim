#pragma strict

var rotateSpeed : float = 50.0f;
var vertBounce : float = 1.0f;
var rotationReference : float = 0.0f;

function Start () {

}

private var upDown : int = 180;

function Update () {
	transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
	//transform.RotateAround(collider.bounds.center, rotateSpeed * Time.deltaTime);  
	if (upDown >= 90)
	{
		transform.Translate(Vector3.up * vertBounce * Time.deltaTime);
		upDown--;
	}
	else if (upDown >= 0)
	{
		transform.Translate(Vector3.down * vertBounce * Time.deltaTime);
		upDown--;
	}
	else
	{
		upDown = 180; 
	}
	//Should count from 180(3sec @ 60 fps), first 90 up, second 90 down, reset at 0.
}