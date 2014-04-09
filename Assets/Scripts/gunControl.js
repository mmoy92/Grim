#pragma strict

var projectile : GameObject;
var fireRate : float = 0.5;
internal var nextFire : float;
var speed : float = 5;
//var weaponFireFX : AudioClip;

function Start () {

}

function Update () {

	//fire1 button is left mouse or left control key
	
   if (Input.GetButton ("Fire2") && Time.time > nextFire) {
   		nextFire = Time.time + fireRate;
      var clone : GameObject = Instantiate (projectile, transform.position, transform.rotation);
      clone.rigidbody.velocity = transform.TransformDirection(Vector3 (0,0,speed));
		Physics.IgnoreCollision(clone.collider, transform.root.collider);
		Destroy(clone, 3);
		
		//gunshot
		//audio.PlayOneShot(weaponFireFX);

   }
}
